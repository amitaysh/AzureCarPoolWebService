namespace ManagementService.DTO
{
    // Close Transaction request, when user returns a car
    public class CloseTransaction
    {
        public Guid CarId { get; set; }
        public DateTime EndDate { get; set; }
    }
}