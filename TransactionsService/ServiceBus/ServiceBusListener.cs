using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using AssignCarUser = TransactionsService.DTO.AssignCarUser;
using TransactionsService.Services;
using TransactionsService.DTO;

namespace TransactionsService.ServiceBus
{
    public class ServiceBusListener : IServiceBusListener
    {
        private readonly ServiceBusProcessor _newAssignmentProcessor;
        private readonly ServiceBusProcessor _closeTransactionProcessor;
        private readonly ITransactionService _transactionService;

        public ServiceBusListener(IConfiguration configuration, ITransactionService transactionService)
        {
            var serviceBusConnectionString = configuration.GetConnectionString("ServiceBusConnection");
            var newAssignmentTopic = configuration["ServiceBus:NewAssignmentTopicName"];
            var closeTransactionQueue = configuration["ServiceBus:CloseTransactionQueueName"];
            var newAssignmentSubscription = configuration["ServiceBus:NewAssignmentSubscription"];

            _newAssignmentProcessor = new ServiceBusClient(serviceBusConnectionString).CreateProcessor(newAssignmentTopic, newAssignmentSubscription);
            _closeTransactionProcessor = new ServiceBusClient(serviceBusConnectionString).CreateProcessor(closeTransactionQueue);
            _transactionService = transactionService;
        }

        public void StartServiceBusReceiver(CancellationToken cancellationToken)
        {
            // Start the Service Bus receiver to listen for new assignment messages
            _newAssignmentProcessor.ProcessMessageAsync += ProcessNewAssignmentMessages;
            _newAssignmentProcessor.ProcessErrorAsync += ProcessServiceBusErrorAsync;
            _newAssignmentProcessor.StartProcessingAsync(cancellationToken).GetAwaiter().GetResult();

            // Start the Service Bus receiver to listen for close transaction messages
            _closeTransactionProcessor.ProcessMessageAsync += ProcessCloseTransactionMessages;
            _closeTransactionProcessor.ProcessErrorAsync += ProcessServiceBusErrorAsync;
            _closeTransactionProcessor.StartProcessingAsync(cancellationToken).GetAwaiter().GetResult();
        }

        public async Task StopServiceBusReceiverAsync(CancellationToken cancellationToken)
        {
            // Stop the Service Bus receiver
            await _newAssignmentProcessor.StopProcessingAsync(cancellationToken);
            await _newAssignmentProcessor.DisposeAsync();
            await _closeTransactionProcessor.StopProcessingAsync(cancellationToken);
            await _closeTransactionProcessor.DisposeAsync();
        }

        private async Task ProcessNewAssignmentMessages(ProcessMessageEventArgs args)
        {
            string messageBody = args.Message.Body.ToString();

            AssignCarUser assignCarUser = JsonConvert.DeserializeObject<AssignCarUser>(messageBody);
            await _transactionService.DigestMessage(assignCarUser.CarId, assignCarUser.UserId);

            await args.CompleteMessageAsync(args.Message);
        }

        private async Task ProcessCloseTransactionMessages(ProcessMessageEventArgs args)
        {
            string messageBody = args.Message.Body.ToString();

            CloseTransaction closeTransaction = JsonConvert.DeserializeObject<CloseTransaction>(messageBody);
            await _transactionService.CloseTransactionAsync(closeTransaction);

            await args.CompleteMessageAsync(args.Message);
        }

        private Task ProcessServiceBusErrorAsync(ProcessErrorEventArgs args)
        {
            // Handle any errors that occur during message processing
            Console.WriteLine($"Service Bus message processing error: {args.Exception}");
            return Task.CompletedTask;
        }
    }
}