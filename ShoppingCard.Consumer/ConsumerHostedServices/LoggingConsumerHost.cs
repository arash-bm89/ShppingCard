using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athena.RabbitMQHelper;
using ShoppingCard.BrokerMessage;

namespace ShoppingCard.Consumer.ConsumerHostedServices;

public class LoggingConsumerHost : BaseConsumerHostedService<LogMessage>
{
    public LoggingConsumerHost(IAsyncJobConsumer<LogMessage> consumer) : base(consumer)
    {
    }
}