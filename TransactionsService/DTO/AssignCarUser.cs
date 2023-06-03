
namespace TransactionsService.DTO
{
    // assign car and user class when getting message about new assignment
    public class AssignCarUser
    {
        public Guid CarId { get; set; }
        public Guid UserId { get; set; }
    }
}