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

        public void StartServiceBusReceiver(CancellationToken cancellationToken)
        {
            // Start the Service Bus receiver to listen for new assignment messages
            _newAssignmentProcessor.ProcessMessageAsync += ProcessNewAssignmentMessages;
            _newAssignmentProcessor.ProcessErrorAsync += ProcessServiceBusErrorAsync;
            _newAssignmentProcessor.StartProcessingAsync(cancellationToken).GetAwaiter().GetResult();
        }

        public async Task StopServiceBusReceiverAsync(CancellationToken cancellationToken)
        {
            // Stop the Service Bus receiver
            await _newAssignmentProcessor.StopProcessingAsync(cancellationToken);
            await _newAssignmentProcessor.DisposeAsync();
        }

        private async Task ProcessNewAssignmentMessages(ProcessMessageEventArgs args)
        {
            string messageBody = args.Message.Body.ToString();

            Request request = JsonConvert.DeserializeObject<Request>(messageBody);
            await _garageService.DigestMessage(request.CarId, request.UserId);

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