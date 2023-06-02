namespace TransactionsService.DTO
{
    public class CloseTransaction
    {
        public Guid CarId { get; set; }
        public DateTime EndDate { get; set; }
    }
}