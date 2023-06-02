
namespace ManagementService.DomainModels
{
    public class Car
    {
        public Guid Id { get; set; }
        public string LicenseNumber { get; set; }
        public string Color { get; set; }
        public string Manufacturer { get; set; }
        public string PhotoUrl { get; set; }
        public Guid? UserId { get; set; }
    }
}