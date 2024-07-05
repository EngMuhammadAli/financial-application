namespace financial_application.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
