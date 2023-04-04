using EventBus.Base;
using EventBus.Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : BaseEventBus
    {

        public EventBusRabbitMQ(IServiceProvider serviceProvider, EventBusConfig config) : base(serviceProvider, config)
        {

        }

        public override void Publish(IntegrationEvent @event)
        {
            throw new NotImplementedException();
        }

        public override void Subscribe<T, TH>()
        {
            throw new NotImplementedException();
        }

        public override void Unsubscribe<T, TH>()
        {
            throw new NotImplementedException();
        }
    }
}
