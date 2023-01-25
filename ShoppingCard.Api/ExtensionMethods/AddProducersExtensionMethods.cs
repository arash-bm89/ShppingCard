using Athena.RabbitMQHelper;
using ShoppingCard.BrokerMessage;

namespace ShoppingCard.Api.ExtensionMethods
{
    public static class AddProducersExtensionMethods
    {
        public static void AddAsyncPublishers(this IServiceCollection services)
        {
            services.AddSingleton<IAsyncJobProducer<LogMessage>, AsyncJobProducer<LogMessage>>();
        }

    }
}
