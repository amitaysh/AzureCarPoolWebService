
namespace ManagementService.DTO
{
    // Clear\remove car from user request
    public class ClearCarUser
    {
        public Guid? CarId { get; set; }
        public Guid? UserId { get; set; }
    }
}