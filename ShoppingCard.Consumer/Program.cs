using Athena.RabbitMQHelper;
using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using ShoppingCard.BrokerMessage;
using ShoppingCard.Consumer;
using ShoppingCard.Consumer.ConsumerHostedServices;
using ShoppingCard.Consumer.Consumers;
using ILogger = Microsoft.Extensions.Logging.ILogger;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(builder =>
    {
        builder.Sources.Clear();
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("./appsettings.json", true, true)
            .AddJsonFile("./appsettings.development.json", true, true);
    })
    .ConfigureLogging((context, builder) =>
    {
        builder.AddConsole()
            .AddConfiguration(context.Configuration.GetSection("Logging"));
    })
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(context.Configuration.GetConnectionString("basketDb"));
        });
        services.AddRabbit(context.Configuration.GetSection("Rabbit"));
        services.AddSingleton<IAsyncJobConsumer<LogMessage>, LoggingConsumer>();
        services.AddHostedService<LoggingConsumerHost>();
    })
    .UseConsoleLifetime()
    .Build();

host.Run();