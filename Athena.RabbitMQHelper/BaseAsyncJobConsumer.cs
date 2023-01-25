using System.Runtime.CompilerServices;
using EasyNetQ;
using EasyNetQ.Internals;
using Microsoft.Extensions.Logging;

namespace Athena.RabbitMQHelper
{
    public abstract class BaseAsyncJobConsumer<TMessage> : IAsyncJobConsumer<TMessage>
    where TMessage : Message, new()
    {
        private readonly ILogger<BaseAsyncJobConsumer<TMessage>> _logger;
        private readonly IBus _bus;
        private readonly string _subscriptionId;
        private readonly string _routingKey;
        private readonly ushort _prefetchCount;
        private SubscriptionResult _subscription;
        protected BaseAsyncJobConsumer(ILogger<BaseAsyncJobConsumer<TMessage>> logger,
            IBus bus,
            string subscriptionId = "", string routingKey = "#",
            ushort prefetchCount = 10)
        {
            _logger = logger;
            _bus = bus;
            _subscriptionId = subscriptionId;
            _routingKey = routingKey;
            _prefetchCount = prefetchCount;
        }

        public async Task Subscribe()
        {
            _subscription = await _bus.PubSub
                .SubscribeAsync<TMessage>(_subscriptionId, OnMessageWrapper,
                        x =>
                    {
                        x.WithTopic(_routingKey);
                        x.WithPrefetchCount(_prefetchCount);
                    });
        }


        public async Task OnMessageWrapper(TMessage message, CancellationToken cancellationToken)
        {
            try
            {
                await OnMessage(message, cancellationToken);
            }
            catch (Exception e)
            {
                e.Data.Add("MessageType", typeof(TMessage));
                _logger.LogCritical(e, e.Message ?? "Throw unknown exception while processing OnMessage message");

                throw;
            }
        }

        public abstract Task OnMessage(TMessage message, CancellationToken cancellationToken);


        public void Dispose()
        {
            _subscription.Dispose();
        }
    }
}
