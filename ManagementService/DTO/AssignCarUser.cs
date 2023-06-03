
namespace ManagementService.DTO
{
    // Assign car to user request
    public class AssignCarUser
    {
        public Guid CarId { get; set; }
        public Guid UserId { get; set; }
    }
}