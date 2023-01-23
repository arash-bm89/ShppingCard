using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athena.RabbitMQHelper;
using EasyNetQ;

namespace ShoppingCard.Consumer
{
    [Queue("message", ExchangeName = "message")]
    public class MessageConsuming : Message
    {
        public string Text { get; set; }
    }
}
