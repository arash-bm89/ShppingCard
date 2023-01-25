using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.RabbitMQHelper
{
    public interface IAsyncJobProducer<in TMessage> : IDisposable
    where TMessage : Message, new()
    {

        Task PublishAsync(TMessage body, byte priority = 0, string routingKey = "#", CancellationToken cancellationToken = default(CancellationToken));

        Task PublishAsync(TMessage body, TimeSpan expireAfter, byte priority = 0, string routingKey = "#", CancellationToken cancellationToken = default(CancellationToken));
    }
}
