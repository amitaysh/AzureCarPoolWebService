namespace TransactionsService.DTO
{
    public class CreateTransaction
    {
        public Guid CarId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartDate { get; set; }
    }
}