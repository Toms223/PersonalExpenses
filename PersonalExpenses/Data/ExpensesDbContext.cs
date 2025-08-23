using Microsoft.EntityFrameworkCore;
using PersonalExpenses.Model;

namespace PersonalExpenses.Data;

public class ExpensesDbContext : DbContext
{
    public ExpensesDbContext(DbContextOptions<ExpensesDbContext> options) : base(options){}
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<User> Users { get; set; }
    
    public DbSet<Category> Categories { get; set; }
}