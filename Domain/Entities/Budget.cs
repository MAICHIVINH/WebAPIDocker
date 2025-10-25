namespace Domain.Entities;

public class Budget
{
    public int id { get; set; }
    public int user_id { get; set; }
    public int category_id { get; set; }
    public decimal amount { get; set; }
    public string period { get; set; } = "MONTHLY"; 
    public DateTime start_date { get; set; }
    public DateTime end_date { get; set; }
    public DateTime created_at { get; set; } = DateTime.UtcNow;

    public Category Category { get; set; } = null!;
}