using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athena.RabbitMQHelper;
using EasyNetQ;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using ShoppingCard.BrokerMessage;

namespace ShoppingCard.Consumer.Consumers;

public class LoggingConsumer : BaseAsyncJobConsumer<LogMessage>
{
    private readonly ILogger<LoggingConsumer> _logger;

    public LoggingConsumer(ILogger<LoggingConsumer> logger,
        IBus bus,
        string subscriptionId = "",
        string routingKey = "#",
        ushort prefetchCount = 10)
        : base(logger, bus, subscriptionId, routingKey, prefetchCount)
    {
        _logger = logger;
    }

    public override async Task OnMessage(LogMessage message, CancellationToken cancellationToken)
    {
        message.Body = JsonConvert.SerializeObject(message.Body, Formatting.None);

        var log = JsonConvert.SerializeObject(message, Formatting.None);

        if (message.HasException)
        {

        }
        _logger
            .LogInformation
                ($"{DateTime.UtcNow}  IP={message.Ip}  HttpMethod={message.HttpMethod}  StatusCode={message.StatusCode}  Body={message.Body}  HasException={message.HasException}  Exception={message.ErrorMessage}");

        await Task.CompletedTask;
    }
}