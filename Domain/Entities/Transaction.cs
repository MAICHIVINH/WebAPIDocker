namespace Domain.Entities;

public class Transaction
{
    public int id { get; set; }
    public int user_id { get; set; }
    public int category_id { get; set; }
    public decimal amount { get; set; }
    public string type { get; set; } = "EXPENSE";
    public DateTime transaction_date { get; set; }
    public string? note { get; set; }
    public DateTime created_at { get; set; } = DateTime.UtcNow;

    public Category Category { get; set; } = null!;
}