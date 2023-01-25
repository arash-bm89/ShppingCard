using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.RabbitMQHelper
{
    public interface IAsyncJobConsumer<in TMessage> : IDisposable
    where TMessage : Message
    {
        /// <summary>
        /// using inside an BaseConsumerHostedService instance in StartAsync method.
        /// </summary>
        Task Subscribe();


        /// <summary>
        /// an abstract method that going to implement in the implementations of this interface and AsyncJobConsumer
        /// </summary>
        /// <param name="message">Message to Subscribing</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task OnMessage(TMessage message, CancellationToken cancellationToken);

    }
}
