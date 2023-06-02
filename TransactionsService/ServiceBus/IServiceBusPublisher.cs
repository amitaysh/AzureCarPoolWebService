using TransactionsService.DomainModels;

namespace TransactionsService.ServiceBus
{
    public interface IServiceBusPublisher
    {
        Task PublishTransaction(Transaction transaction);

    }
}
