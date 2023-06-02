using GarageService.DomainModels;

namespace GarageService.ServiceBus
{
    public interface IServiceBusListener
    {
        void StartServiceBusReceiver(CancellationToken cancellationToken);
        Task StopServiceBusReceiverAsync(CancellationToken cancellationToken);

    }
}
