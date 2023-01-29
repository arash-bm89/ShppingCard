using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;

namespace Athena.RabbitMQHelper;

// todo: implement can connect
public class AsyncJobProducer<TMessage> : IAsyncJobProducer<TMessage>
    where TMessage : Message, new()
{
    private readonly IBus _bus;

    public AsyncJobProducer(IBus bus)
    {
        _bus = bus;
    }

    public Task PublishAsync(TMessage body, byte priority = 0, string routingKey = "#",
        CancellationToken cancellationToken = default)
    {
        return _bus.PubSub.PublishAsync(body, x =>
        {
            x.WithTopic(routingKey);
            x.WithPriority(priority);
        }, cancellationToken);
    }

    public Task PublishAsync(TMessage body, TimeSpan expireAfter, byte priority = 0, string routingKey = "#",
        CancellationToken cancellationToken = default)
    {
        return _bus.PubSub.PublishAsync(body, x =>
        {
            x.WithTopic(routingKey);
            x.WithPriority(priority);
            x.WithExpires(expireAfter);
        }, cancellationToken);
    }

    public void Dispose()
    {
    }
}