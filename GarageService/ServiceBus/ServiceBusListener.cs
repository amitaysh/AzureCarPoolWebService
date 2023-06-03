using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using GarageService.Services;
using GarageService.DTO;

namespace GarageService.ServiceBus
{
    public class ServiceBusListener : IServiceBusListener
    {
        private readonly ServiceBusProcessor _newAssignmentProcessor;
        private readonly IGarageService _garageService;

        public ServiceBusListener(IConfiguration configuration, IGarageService garageService)
        {
            var serviceBusConnectionString = configuration.GetConnectionString("ServiceBusConnection");
            var newAssignmentTopic = configuration["ServiceBus:NewAssignmentTopicName"];
            var newAssignmentSubscription = configuration["ServiceBus:NewAssignmentSubscription"];

            _newAssignmentProcessor = new ServiceBusClient(serviceBusConnectionString).CreateProcessor(newAssignmentTopic, newAssignmentSubscription);

            _garageService = garageService;
        }

        // Start the Service Bus receiver to listen for new assignment messages
        public void StartServiceBusReceiver(CancellationToken cancellationToken)
        {
            _newAssignmentProcessor.ProcessMessageAsync += ProcessNewAssignmentMessages;
            _newAssignmentProcessor.ProcessErrorAsync += ProcessServiceBusErrorAsync;
            _newAssignmentProcessor.StartProcessingAsync(cancellationToken).GetAwaiter().GetResult();
        }

        // Stop the Service Bus receiver
        public async Task StopServiceBusReceiverAsync(CancellationToken cancellationToken)
        {
            await _newAssignmentProcessor.StopProcessingAsync(cancellationToken);
            await _newAssignmentProcessor.DisposeAsync();
        }

        // Process messages to create new request
        private async Task ProcessNewAssignmentMessages(ProcessMessageEventArgs args)
        {
            string messageBody = args.Message.Body.ToString();

            Request request = JsonConvert.DeserializeObject<Request>(messageBody);
            await _garageService.DigestMessage(request.CarId, request.UserId);

            await args.CompleteMessageAsync(args.Message);
        }

        // Handle any errors that occur during message processing
        private Task ProcessServiceBusErrorAsync(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Service Bus message processing error: {args.Exception}");
            return Task.CompletedTask;
        }
    }
}