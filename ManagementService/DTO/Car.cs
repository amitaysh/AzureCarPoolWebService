
namespace ManagementService.DTO
{
    // Car class
    public class Car
    {
        public Guid Id { get; set; }
        public string LicenseNumber { get; set; }
        public string Color { get; set; }
        public string Manufacturer { get; set; }
        public string PhotoName { get; set; }
        public Guid? UserId { get; set; }
    }
}