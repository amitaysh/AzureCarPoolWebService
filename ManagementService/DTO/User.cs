
namespace ManagementService.DTO
{
    // User class
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? CarId { get; set; }
    }
}