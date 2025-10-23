using Microsoft.EntityFrameworkCore;
using Models;

namespace Db;

public class ExpensesAndIncomesDb : DbContext
{
    public ExpensesAndIncomesDb(DbContextOptions options) : base(options) { }
    public DbSet<Expense> Expenses { get; set; } = null!;
    public DbSet<Income> Incomes { get; set; } = null!;
    public DbSet<NameTypeOfExpenses> NamesTypesOfExpenses { get; set; } = null!;
    public DbSet<NameTypeOfIncomes> NamesTypesOfIncomes { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
}