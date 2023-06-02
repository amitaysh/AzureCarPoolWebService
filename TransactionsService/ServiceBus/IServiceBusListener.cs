using TransactionsService.DomainModels;

namespace TransactionsService.ServiceBus
{
    public interface IServiceBusListener
    {
        void StartServiceBusReceiver(CancellationToken cancellationToken);
        Task StopServiceBusReceiverAsync(CancellationToken cancellationToken);

    }
}
