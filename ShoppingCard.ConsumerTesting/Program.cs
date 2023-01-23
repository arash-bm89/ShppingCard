// See https://aka.ms/new-console-template for more information


using EasyNetQ;
using ShoppingCard.Consumer;

IBus bus = RabbitHutch.CreateBus("host=localhost");

var input = string.Empty;

while (!string.Equals(input, "Quit"))
{

    if (!string.IsNullOrEmpty(input))
    {
        bus.PubSub.PublishAsync<MessageConsuming>(new MessageConsuming
        {
            Text = input
        });
    }

    input = Console.ReadLine();
}
