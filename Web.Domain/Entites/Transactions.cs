namespace Web.Domain.Entites
{
    public class Transactions : BaseEntity<int>
    {

        public string UserId { get; set; }
        public AppUser User { get; set; }
        public int TransactionId { get; set; }

    }
}
