using Azure.Messaging.ServiceBus;
using ManagementService.DTO;
using Newtonsoft.Json;

namespace ManagementService.ServiceBus
{
    public class ServiceBusPublisher : IServiceBusPublisher
    {
        private readonly ServiceBusSender _newAssignmentPublisher;
        private readonly ServiceBusSender _closeTransactionPublisher;

        public ServiceBusPublisher(IConfiguration configuration)
        {
            var serviceBusConnectionString = configuration.GetConnectionString("ServiceBusConnection");
            var newAssignmentTopic = configuration["ServiceBus:NewAssignmentTopicName"];
            var closeTransactionQueue = configuration["ServiceBus:CloseTransactionQueueName"];

            _newAssignmentPublisher = new ServiceBusClient(serviceBusConnectionString).CreateSender(newAssignmentTopic);
            _closeTransactionPublisher = new ServiceBusClient(serviceBusConnectionString).CreateSender(closeTransactionQueue);

        }

        // publish message to service bus topic upon new assignment of user and car
        public async Task PublishNewAssignment(AssignCarUser newAssignment)
        {
            // Publish the new assignment to the Service Bus
            string messageBody = JsonConvert.SerializeObject(newAssignment);
            ServiceBusMessage message = new ServiceBusMessage(messageBody);
            await _newAssignmentPublisher.SendMessageAsync(message);
        }

        // publish message to service bus queue upon returning a car (closing transaction)
        public async Task PublishCloseTransaction(CloseTransaction closeTransaction)
        {
            // Publish the close transaction to the Service Bus
            string messageBody = JsonConvert.SerializeObject(closeTransaction);
            ServiceBusMessage message = new ServiceBusMessage(messageBody);
            await _closeTransactionPublisher.SendMessageAsync(message);
        }
    }
}