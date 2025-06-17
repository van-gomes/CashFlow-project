using CashFlow.Domain;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess;

public class CashFlowDbContext : DbContext
{
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Server=localhost;Database=cashFlowdb;User ID=;Password=;TrustServerCertificate=True;";
        optionsBuilder.UseSqlServer(connectionString);
    }
}