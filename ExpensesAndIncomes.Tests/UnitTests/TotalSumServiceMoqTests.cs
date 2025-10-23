using Models;
using Services;
using UoW;
using Repositorys;
using MockQueryable;
using Moq;
using Microsoft.Extensions.Logging;
using Factorys;

public class TotalSumServiceMoqTests
{
    [Fact]
    public async Task TotalBalance_ExpensesOrIncomesAreExists_ShouldShow()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();
        
        var existExpense = new List<Expense>
        {
            factoryOperationMock.Object.Create(DateTime.Now, 300, "Food", "fsdfsdfdsdfsd")
        }.BuildMock().AsQueryable();

        var incomesExist = new List<Income>
        {
            
        }.BuildMock().AsQueryable();

        var existTypeOfExpenses = new List<NameTypeOfExpenses>
        {
            factoryTypeMock.Object.Create("Food")
        }.BuildMock().AsQueryable();

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {

        }.BuildMock().AsQueryable();

        var loggerMock = new Mock<ILogger<TotalSumService>>();

        var repoExpenseMock = new Mock<IOperationRepository<Expense>>();
        repoExpenseMock.Setup(e => e.GetAll()).Returns(existExpense);

        var repoIncomeMock = new Mock<IOperationRepository<Income>>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(incomesExist);

        var repoOfTypeExpensesMock = new Mock<ITypeOfOperationRepository<NameTypeOfExpenses>>();
        repoOfTypeExpensesMock.Setup(t => t.GetAll()).Returns(existTypeOfExpenses);

        var repoOfTypeIncomesMock = new Mock<ITypeOfOperationRepository<NameTypeOfIncomes>>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes);

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(u => u.expenseRepository).Returns(repoExpenseMock.Object);
        unitMock.Setup(u => u.typeOfExpensesRepository).Returns(repoOfTypeExpensesMock.Object);
        unitMock.Setup(u => u.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(u => u.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new TotalSumService(unitMock.Object,loggerMock.Object);

        var result = await service.TotalBalance();

        Assert.True(result.IsSuccess);
        Assert.Equal(-300, result.Value);
    }

    [Fact]
    public async Task TotalBalance_ExpensesOrIncomesDoesNotExist_ShouldFail()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpense = new List<Expense>
        {

        }.BuildMock().AsQueryable();

        var incomesExist = new List<Income>
        {

        }.BuildMock().AsQueryable();

        var existTypeOfExpenses = new List<NameTypeOfExpenses>
        {
            factoryTypeMock.Object.Create("Food")
        }.BuildMock().AsQueryable();

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {

        }.BuildMock().AsQueryable();

        var loggerMock = new Mock<ILogger<TotalSumService>>();

        var repoExpenseMock = new Mock<IOperationRepository<Expense>>();
        repoExpenseMock.Setup(e => e.GetAll()).Returns(existExpense);

        var repoIncomeMock = new Mock<IOperationRepository<Income>>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(incomesExist);

        var repoOfTypeExpensesMock = new Mock<ITypeOfOperationRepository<NameTypeOfExpenses>>();
        repoOfTypeExpensesMock.Setup(t => t.GetAll()).Returns(existTypeOfExpenses);

        var repoOfTypeIncomesMock = new Mock<ITypeOfOperationRepository<NameTypeOfIncomes>>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes);

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(u => u.expenseRepository).Returns(repoExpenseMock.Object);
        unitMock.Setup(u => u.typeOfExpensesRepository).Returns(repoOfTypeExpensesMock.Object);
        unitMock.Setup(u => u.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(u => u.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new TotalSumService(unitMock.Object, loggerMock.Object);

        var result = await service.TotalBalance();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no expense and incomes", result.Error);
    }
    
    private(Mock<IOperationFactory<Expense>> factoryOperationMock,
            Mock<INameTypeOfOperationsFactory<NameTypeOfExpenses>> factoryTypeMock)
            Create_type_and_operations_factorys()
    
    {
        var factoryOperationMock = new Mock<IOperationFactory<Expense>>();
        factoryOperationMock.Setup(f => f.Create(It.IsAny<DateTime>(), It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>()))
                            .Returns((DateTime dateOfAction, double amount, string type, string comment) => new Expense(dateOfAction, amount, type, comment));
        var factoryTypeMock = new Mock<INameTypeOfOperationsFactory<NameTypeOfExpenses>>();
        factoryTypeMock.Setup(f => f.Create(It.IsAny<string>())).Returns((string nameOfType) => new NameTypeOfExpenses(nameOfType));
        return (factoryOperationMock, factoryTypeMock);

    }
}