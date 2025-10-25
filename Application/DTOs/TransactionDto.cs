namespace Application.DTOs
{

    public class CreateTransactionDto
    {
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = "EXPENSE";
        public DateTime TransactionDate { get; set; }
        public string? Note { get; set; }
    }

    public  class TransactionDto : CreateTransactionDto
    {
        public int Id { get; set; }
    }

}