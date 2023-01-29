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
    private readonly ApplicationDbContext _db;

    public LoggingConsumer(ILogger<LoggingConsumer> logger,
        IBus bus,
        ApplicationDbContext db,
        string subscriptionId = "",
        string routingKey = "#",
        ushort prefetchCount = 10)
        : base(logger, bus, subscriptionId, routingKey, prefetchCount)
    {
        _logger = logger;
        _db = db;
    }

    public override async Task OnMessage(LogMessage message, CancellationToken cancellationToken)
    {
        await _db.LogMessages.AddAsync(message, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }
}