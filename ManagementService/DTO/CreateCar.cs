
namespace ManagementService.DTO
{
    // Create car request
    public class CreateCar
    {
        public string LicenseNumber { get; set; }
        public string Color { get; set; }
        public string Manufacturer { get; set; }
        public string PhotoName { get; set; }
    }
}