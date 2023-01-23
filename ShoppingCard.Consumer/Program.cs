using Athena.RabbitMQHelper;
using EasyNetQ;
using Newtonsoft.Json;
using ShoppingCard.Consumer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {

        services.AddRabbit(context.Configuration.GetSection("Rabbit"));
        services.AddHostedService<Worker>();

    })
    .UseConsoleLifetime()
    .Build();

host.Run();

//List<ISubscriptionResult> list = new List<ISubscriptionResult>();

//var bus = RabbitHutch.CreateBus("host=localhost");

//list.Add(bus.SubscribeAsync<MessageConsuming>("test", m =>
//{
//    Console.WriteLine(JsonConvert.SerializeObject(m, Formatting.None));
//    return Task.CompletedTask;
//}));

//Console.ReadLine();


//bus.Dispose();