using Db;
using Microsoft.EntityFrameworkCore;
using Services;
using Dtos;
using Models;
using UoW;
using Repositorys;

public class ExpensesManipulatorTests
{
    private ExpensesAndIncomesDb GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ExpensesAndIncomesDb>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var dbContext = new ExpensesAndIncomesDb(options);
        dbContext.Database.EnsureCreated();
        return dbContext;
    }

    [Fact]
    public async Task InfoOfExpenses_ExpensesExist_ShouldReturnList()
    {
        var unit = CreateUnit();
        var service = new ExpensesManipulator(unit);

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Expense expense = new Expense(DateTime.Now, dto.Amount, dto.TypeOfExpenses, dto.Comment);
        await unit.expenseRepository.AddAsync(expense);

        await unit.SaveChangesAsync();

        var result = await service.InfoOfExpenses();

        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task InfoOfExpenses_ExpensesDoesNotExist_ShouldFail()
    {
        var unit = CreateUnit();
        var service = new ExpensesManipulator(unit);

        var result = await service.InfoOfExpenses();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no Expenses for now", result.Error);
    }

    [Fact]
    public async Task AddNewExpense_ExpenseUniqe_ShouldAdd()
    {
        var unit = CreateUnit();
        var service = new ExpensesManipulator(unit);

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.AddNewExpense(dto);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task AddNewExpense_TypeDoesNotFound_ShouldFail()
    {
        var unit = CreateUnit();
        var service = new ExpensesManipulator(unit);

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Other",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.AddNewExpense(dto);

        Assert.True(result.IsFailure);
        Assert.Equal("This type of Expenses does not found", result.Error);
    }

    [Fact]
    public async Task Update_ExpensesIdExist_ShouldUpdate()
    {
        var unit = CreateUnit();
        var service = new ExpensesManipulator(unit);

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        TypeOfExpenses newTypeOfExpenses2 = new TypeOfExpenses("Other");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);
        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses2);

        await unit.SaveChangesAsync();

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Expense expense = new Expense(DateTime.Now, dto.Amount, dto.TypeOfExpenses, dto.Comment);
        await unit.expenseRepository.AddAsync(expense);

        await unit.SaveChangesAsync();

        var dto2 = new ExpenseDto
        {
            TypeOfExpenses = "Other",
            Amount = 450,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.Update(dto2, expense.Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Update_ExpensesIdDoesNotExist_ShouldFail()
    {
        var unit = CreateUnit();
        var service = new ExpensesManipulator(unit);

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        TypeOfExpenses newTypeOfExpenses2 = new TypeOfExpenses("Other");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);
        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses2);

        await unit.SaveChangesAsync();

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Expense expense = new Expense(DateTime.Now, dto.Amount, dto.TypeOfExpenses, dto.Comment);
        await unit.expenseRepository.AddAsync(expense);

        await unit.SaveChangesAsync();

        var dto2 = new ExpenseDto
        {
            TypeOfExpenses = "Other",
            Amount = 450,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.Update(dto2, 9999);

        Assert.True(result.IsFailure);
        Assert.Equal($"Expense whith this Id(9999) does not exist", result.Error);
    }

    [Fact]
    public async Task Delete_ExpenseIdExist_ShouldDelete()
    {
        var unit = CreateUnit();
        var service = new ExpensesManipulator(unit);

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        TypeOfExpenses newTypeOfExpenses2 = new TypeOfExpenses("Other");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);
        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses2);

        await unit.SaveChangesAsync();

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Expense expense = new Expense(DateTime.Now, dto.Amount, dto.TypeOfExpenses, dto.Comment);
        await unit.expenseRepository.AddAsync(expense);

        await unit.SaveChangesAsync();

        var result = await service.Delete(expense.Id);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Delete_ExpenseIdDoesNotExist_ShouldFail()
    {
        var unit = CreateUnit();
        var service = new ExpensesManipulator(unit);

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        TypeOfExpenses newTypeOfExpenses2 = new TypeOfExpenses("Other");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);
        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses2);

        await unit.SaveChangesAsync();

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Expense expense = new Expense(DateTime.Now, dto.Amount, dto.TypeOfExpenses, dto.Comment);
        await unit.expenseRepository.AddAsync(expense);

        await unit.SaveChangesAsync();

        var result = await service.Delete(99999999);

        Assert.True(result.IsFailure);
        Assert.Equal($"Expense whith this Id(99999999) does not exist", result.Error);
    }
    private UnitOfWork CreateUnit()
    {
        var dbContext = GetInMemoryDbContext();
        var repoIncome = new IncomeRepository(dbContext);
        var repoExpense = new ExpenseRepository(dbContext);
        var repoOfTypeIncomes = new TypeOfIncomesRepository(dbContext);
        var repoOfTypeExpenses = new TypeOfExpensesRepository(dbContext);
        var unit = new UnitOfWork(repoExpense, repoIncome, repoOfTypeExpenses, repoOfTypeIncomes, dbContext);
        return unit;
    }
}