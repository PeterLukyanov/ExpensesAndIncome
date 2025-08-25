using Microsoft.EntityFrameworkCore;
using Models;

namespace Db;

public class ExpensesAndIncomesDb : DbContext
{
    public ExpensesAndIncomesDb(DbContextOptions options) : base(options) { }
    public DbSet<Expense> Expenses { get; set; } = null!;
    public DbSet<Income> Incomes { get; set; } = null!;
    public DbSet<TypeOfExpenses> TypesOfExpenses { get; set; } = null!;
    public DbSet<TypeOfIncomes> TypesOfIncomes { get; set; } = null!;
}