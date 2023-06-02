using ManagementService.DTO;

namespace ManagementService.ServiceBus
{
    public interface IServiceBusPublisher
    {
        Task PublishNewAssignment(AssignCarUser newAssignment);
        Task PublishCloseTransaction(CloseTransaction closeTransaction);
    }
}
