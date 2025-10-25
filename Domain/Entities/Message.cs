using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Message
{
    [Column("id")]
    public int Id { get; set; }

    [Column("from_user_id")]
    public int FromUserId { get; set; }

    [Column("to_user_id")]
    public int ToUserId { get; set; }

    [Column("text")]
    public string Text { get; set; }

    [Column("status")]
    public string Status { get; set; } = "sent"; // sent, delivered, seen, failed

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } 

    // Navigation properties
    public User FromUser { get; set; }
    public User ToUser { get; set; }
}