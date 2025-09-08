using ExpenseTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

public class ExpenseDbContext : DbContext
{
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> opts) : base(opts) { }

    public DbSet<Expense> Expenses { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Expense>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            e.Property(x => x.Date).HasColumnType("TEXT");
            e.Property(x => x.Category).HasMaxLength(100).IsRequired();

        });
    }
}