using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Infrastructure
{
    public class ExpenseDbContextFactory : IDesignTimeDbContextFactory<ExpenseDbContext>
    {
        public ExpenseDbContext CreateDbContext(string[] args)
        {
            // Lấy connection string từ appsettings.json của WebAPI
            var config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "../WebAPIDocker"))
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ExpenseDbContext>();
            optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));

            return new ExpenseDbContext(optionsBuilder.Options);
        }
    }
}
