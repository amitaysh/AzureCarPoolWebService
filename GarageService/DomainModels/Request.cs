namespace GarageService.DomainModels
{
    public class Request
    {
        public string Id { get; set; }
        public Guid CarId { get; set; }
        public Guid UserId { get; set; }
    }
}