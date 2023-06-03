using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using TransactionsService.DomainModels;

namespace TransactionsService.ServiceBus
{
    public class ServiceBusPublisher : IServiceBusPublisher
    {
        private readonly ServiceBusSender _serviceBusSender;

        // service bus publisher to queue ctor
        public ServiceBusPublisher(IConfiguration configuration)
        {
            var serviceBusConnectionString = configuration.GetConnectionString("ServiceBusConnection");
            var transactionQueue = configuration["ServiceBus:TransactionQueueName"];

            _serviceBusSender = new ServiceBusClient(serviceBusConnectionString).CreateSender(transactionQueue);
        }

        // publish transaction messages to queue
        public async Task PublishTransaction(Transaction transaction)
        {
            // Publish the transaction to the Service Bus
            string messageBody = JsonConvert.SerializeObject(transaction);
            ServiceBusMessage message = new ServiceBusMessage(messageBody);
            await _serviceBusSender.SendMessageAsync(message);
        }
    }
}