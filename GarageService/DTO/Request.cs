namespace GarageService.DTO
{
    // Request class handles requests received from other services to inform garage about a car request
    public class Request
    {
        public string Id { get; set; }
        public Guid CarId { get; set; }
        public Guid UserId { get; set; }
    }
}