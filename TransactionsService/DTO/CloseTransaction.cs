namespace TransactionsService.DTO
{
    // close transaction request class
    public class CloseTransaction
    {
        public Guid CarId { get; set; }
        public DateTime EndDate { get; set; }
    }
}