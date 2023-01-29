using System.Net;
using Athena.RabbitMQHelper;
using EasyNetQ;

namespace ShoppingCard.BrokerMessage;

[Queue("Logging", ExchangeName = "Logging")]
public class LogMessage : Message
{
    public int? Id { get; set; }
    public string? Ip { get; set; }
    public string? Body { get; set; }
    public string HttpMethod { get; set; }
    public int StatusCode { get; set; }
    public bool HasException { get; set; }
    public string? ErrorMessage { get; set; }
}