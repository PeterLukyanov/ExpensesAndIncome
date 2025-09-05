using Models;
using Services;
using UoW;
using Repositorys;
using MockQueryable;
using Moq;

public class TotalSumServiceMoqTests
{
    [Fact]
    public async Task TotalBalance_ExpensesOrIncomesAreExists_ShouldShow()
    {
        var existExpense = new List<Expense>
        {
            new Expense(DateTime.Now, 300, "Food", "fsdfsdfdsdfsd")
        }.BuildMock().AsQueryable();

        var incomesExist = new List<Income>
        {
            
        }.BuildMock().AsQueryable();

        var existTypeOfExpenses = new List<TypeOfExpenses>
        {
            new TypeOfExpenses("Food")
        }.BuildMock().AsQueryable();

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {

        }.BuildMock().AsQueryable();

        var repoExpenseMock = new Mock<IExpenseRepository>();
        repoExpenseMock.Setup(e => e.GetAll()).Returns(existExpense);

        var repoIncomeMock = new Mock<IIncomeRepository>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(incomesExist);

        var repoOfTypeExpensesMock = new Mock<ITypeOfExpensesRepository>();
        repoOfTypeExpensesMock.Setup(t => t.GetAll()).Returns(existTypeOfExpenses);

        var repoOfTypeIncomesMock = new Mock<ITypeOfIncomesRepository>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes);

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(u => u.expenseRepository).Returns(repoExpenseMock.Object);
        unitMock.Setup(u => u.typeOfExpensesRepository).Returns(repoOfTypeExpensesMock.Object);
        unitMock.Setup(u => u.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(u => u.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new TotalSummService(unitMock.Object);

        var result = await service.TotalBalance();

        Assert.True(result.IsSuccess);
        Assert.Equal(-300, result.Value);
    }

    [Fact]
    public async Task TotalBalance_ExpensesOrIncomesDoesNotExist_ShouldFail()
    {
        var existExpense = new List<Expense>
        {
            
        }.BuildMock().AsQueryable();

        var incomesExist = new List<Income>
        {
            
        }.BuildMock().AsQueryable();

        var existTypeOfExpenses = new List<TypeOfExpenses>
        {
            new TypeOfExpenses("Food")
        }.BuildMock().AsQueryable();

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {

        }.BuildMock().AsQueryable();

        var repoExpenseMock = new Mock<IExpenseRepository>();
        repoExpenseMock.Setup(e => e.GetAll()).Returns(existExpense);

        var repoIncomeMock = new Mock<IIncomeRepository>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(incomesExist);

        var repoOfTypeExpensesMock = new Mock<ITypeOfExpensesRepository>();
        repoOfTypeExpensesMock.Setup(t => t.GetAll()).Returns(existTypeOfExpenses);

        var repoOfTypeIncomesMock = new Mock<ITypeOfIncomesRepository>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes);

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(u => u.expenseRepository).Returns(repoExpenseMock.Object);
        unitMock.Setup(u => u.typeOfExpensesRepository).Returns(repoOfTypeExpensesMock.Object);
        unitMock.Setup(u => u.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(u => u.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new TotalSummService(unitMock.Object);

        var result = await service.TotalBalance();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no expense or incomes", result.Error);
    }
}