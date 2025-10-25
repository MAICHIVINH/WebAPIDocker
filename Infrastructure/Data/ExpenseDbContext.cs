
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ExpenseDbContext : DbContext
{
    
    public  ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options){}
    
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;
    public DbSet<Budget> Budgets { get; set; } = null!;
    
    public DbSet<User> Users { get; set; } = null!;
    
    public DbSet<Message> Messages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Id).HasColumnName("id");
            entity.Property(u => u.Username).HasColumnName("username").IsRequired();
            entity.Property(u => u.Password).HasColumnName("password").IsRequired();
            entity.Property(u => u.Email).HasColumnName("email").IsRequired();
            entity.Property(u => u.Role).HasColumnName("role").HasDefaultValue("USER");
            entity.Property(u => u.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
        
        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("messages");
            entity.HasKey(m => m.Id);

            entity.Property(m => m.Id).HasColumnName("id");
            entity.Property(m => m.FromUserId).HasColumnName("from_user_id");
            entity.Property(m => m.ToUserId).HasColumnName("to_user_id");
            entity.Property(m => m.Text).HasColumnName("text").IsRequired();
            entity.Property(m => m.Status).HasColumnName("status").HasDefaultValue("sent");
            entity.Property(m => m.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();

            // quan hệ với User
            entity.HasOne(m => m.FromUser)
                .WithMany() // có thể làm WithMany(u => u.SentMessages) nếu muốn
                .HasForeignKey(m => m.FromUserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(m => m.ToUser)
                .WithMany() // hoặc u.ReceivedMessages
                .HasForeignKey(m => m.ToUserId)
                .OnDelete(DeleteBehavior.Cascade);
        });


        
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories"); // tên table trong DB

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired();
            entity.Property(e => e.Type).HasColumnName("type").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("transactions");
            entity.HasKey(e => e.id);

            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.user_id).HasColumnName("user_id");
            entity.Property(e => e.category_id).HasColumnName("category_id");
            entity.Property(e => e.amount).HasColumnName("amount");
            entity.Property(e => e.type).HasColumnName("type");
            entity.Property(e => e.transaction_date).HasColumnName("transaction_date").HasColumnType("date");
            entity.Property(e => e.note).HasColumnName("note");
            entity.Property(e => e.created_at).HasColumnName("created_at");

            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.category_id)
                .OnDelete(DeleteBehavior.Cascade);
        });

        
        modelBuilder.Entity<Budget>(entity =>
        {
            entity.ToTable("budgets");
            entity.HasKey(b => b.id);

            entity.Property(b => b.id).HasColumnName("id");
            entity.Property(b => b.user_id).HasColumnName("user_id").IsRequired();
            entity.Property(b => b.category_id).HasColumnName("category_id").IsRequired();
            entity.Property(b => b.amount).HasColumnName("amount").HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(b => b.period).HasColumnName("period").HasMaxLength(20).IsRequired();
            entity.Property(b => b.start_date).HasColumnName("start_date").HasColumnType("date");
            entity.Property(b => b.end_date).HasColumnName("end_date").HasColumnType("date");
            entity.Property(b => b.created_at).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(b => b.Category)
                .WithMany()
                .HasForeignKey(b => b.category_id)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.Property(b => b.start_date)
                .HasColumnName("start_date")
                .HasColumnType("timestamptz");

            entity.Property(b => b.end_date)
                .HasColumnName("end_date")
                .HasColumnType("timestamptz");

        });

    }
        
}