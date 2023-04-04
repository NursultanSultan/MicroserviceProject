using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;


namespace EventBus.RabbitMQ
{
    public class RabbitMQPersistentConnection : IDisposable
    {
        private IConnection connection;
        private readonly IConnectionFactory _connectionFactory;
        private readonly int retryCount;
        private object lock_object = new object();
        private bool _disposed = false;

        public bool IsConnected => connection != null && connection.IsOpen;

        public RabbitMQPersistentConnection(IConnectionFactory connectionFactory , int retryCount = 5)
        {
            _connectionFactory = connectionFactory;
            this.retryCount = retryCount;
        }


        public IModel CreateModel()
        {
            return connection.CreateModel();
        }

        public void Dispose()
        {
            _disposed = true;
            connection?.Dispose();
        }

        public bool TryConnect()
        {
            lock (lock_object)
            {
                var policy = Policy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(retryCount , retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),(ex, time) => 
                    { 

                    }
                );

                policy.Execute(() =>
                {
                    connection = _connectionFactory.CreateConnection();
                });

                if (IsConnected)
                {
                    connection.ConnectionShutdown += Connection_ConnectionShutdown;
                    connection.CallbackException += Connection_CallbackException;
                    connection.ConnectionBlocked += Connection_ConnectionBlocked;
                    //log

                    return true;
                }

                return false;
            }
        }

        private void Connection_CallbackException(object? sender, global::RabbitMQ.Client.Events.CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            TryConnect();
        }

        private void Connection_ConnectionBlocked(object? sender, global::RabbitMQ.Client.Events.ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            TryConnect();
        }

        private void Connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            if (_disposed) return;

            TryConnect();
        }
    }
}
