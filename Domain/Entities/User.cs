using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class User
{
    [Column("id")]
    public int Id { get; set; }
    [Column("username")]
    public string Username { get; set; }
    [Column("password")]
    public string Password { get; set; }
    [Column("email")]
    public string Email { get; set; }
    [Column("role")]
    public string Role { get; set; } = "USER";
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

}