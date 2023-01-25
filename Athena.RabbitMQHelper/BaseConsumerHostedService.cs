using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Athena.RabbitMQHelper
{
    public class BaseConsumerHostedService<TMessage> : IHostedService
    where TMessage : Message, new()
    {
        private readonly IAsyncJobConsumer<TMessage> _consumer;

        public BaseConsumerHostedService(IAsyncJobConsumer<TMessage> consumer)
        {
            _consumer = consumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Dispose();

            return Task.CompletedTask;
        }
    }
}
