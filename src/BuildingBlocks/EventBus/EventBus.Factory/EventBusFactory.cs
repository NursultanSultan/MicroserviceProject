using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.RabbitMQ;


namespace EventBus.Factory
{
    public class EventBusFactory
    {
        public static IEventBus Create(EventBusConfig config, IServiceProvider serviceProvider)
        {
            // it is an another way for switch
            return config.EventBusType switch
            {
                EventBusType.RabbitMQ => new EventBusRabbitMQ(serviceProvider, config),
                
            };
        }
    }
}
