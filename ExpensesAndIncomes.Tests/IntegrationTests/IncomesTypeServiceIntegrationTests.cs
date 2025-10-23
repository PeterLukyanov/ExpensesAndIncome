using Db;
using Dtos;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;
using Repositorys;
using UoW;
using Microsoft.Extensions.Logging;
using Moq;
using Factorys;

public class IncomesTypesServiceTests
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
    public async Task LoadTypeOfIncomes_DataBaseDoesNotExist_ShouldLoad()
    {
        var (service, unit, factoryOperationsMock, factoryTypesMock) = CreateServiceAndUnit();

        await service.LoadTypesForStart();

        var result = await unit.typeOfIncomesRepository.GetAll().ToListAsync();

        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task LoadTypeOfIncomes_DataBaseExist_ShouldFail()
    {
        var (service, unit, factoryOperationsMock, factoryTypesMock) = CreateServiceAndUnit();

        NameTypeOfIncomes newTypeOfIncomes = factoryTypesMock.Object.Create("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        await service.LoadTypesForStart();

        var result = await unit.typeOfIncomesRepository.GetAll().ToListAsync();

        Assert.NotEmpty(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldShow()
    {
        var (service, unit, factoryOperationsMock, factoryTypesMock) = CreateServiceAndUnit();

        NameTypeOfIncomes newTypeOfIncomes = factoryTypesMock.Object.Create("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        var result = await service.InfoTypes();

        Assert.NotEmpty(result.Value);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldFail()
    {
        var (service, unit, factoryOperationsMock, factoryTypesMock) = CreateServiceAndUnit();

        var result = await service.InfoTypes();

        Assert.True(result.IsFailure);
        Assert.Equal($"There are no {nameof(NameTypeOfIncomes)} for now", result.Error);
    }

    [Fact]
    public async Task GetInfoOfType_TypeExist_ShouldShow()
    {
        var (service, unit, factoryOperationsMock, factoryTypesMock) = CreateServiceAndUnit();

        NameTypeOfIncomes newTypeOfIncomes = factoryTypesMock.Object.Create("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        var result = await service.GetInfoOfType(newTypeOfIncomes.Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task GetInfoOfType_TypeDoesNotExist_ShouldFail()
    {
        var (service, unit, factoryOperationsMock, factoryTypesMock) = CreateServiceAndUnit();

        NameTypeOfIncomes newTypeOfIncomes = factoryTypesMock.Object.Create("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        var result = await service.GetInfoOfType(9999);

        Assert.True(result.IsFailure);
        Assert.Equal("Such type of incomes does not exist", result.Error);
    }

    [Fact]
    public async Task TotalSumOfIncomes_IncomesExist_ShouldShow()
    {
        var (service, unit, factoryOperationsMock, factoryTypesMock) = CreateServiceAndUnit();

        NameTypeOfIncomes newTypeOfIncomes = factoryTypesMock.Object.Create("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        Income newIncome = factoryOperationsMock.Object.Create(DateTime.Now, 300, "Salary", "fsdfssadfsd");

        await unit.incomeRepository.AddAsync(newIncome);

        await unit.SaveChangesAsync();

        var result = await service.TotalSumOfOperations();

        Assert.True(result.IsSuccess);
        Assert.Equal(300, result.Value);
    }

    [Fact]
    public async Task TotalSumOfIncomes_IncomesDoesNotExist_ShouldFail()
    {
        var (service, unit, factoryOperationsMock, factoryTypesMock) = CreateServiceAndUnit();

        var result = await service.TotalSumOfOperations();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no incomes", result.Error);
    }

    [Fact]
    public async Task AddType_TypeIsUniqe_ShouldAdd()
    {
        var (service, unit, factoryOperationsMock, factoryTypesMock) = CreateServiceAndUnit();

        var dto = new NameTypeOfOperationsDto
        {
            NameOfType = "Salary"
        };

        var result = await service.AddType(dto);

        Assert.True(result.IsSuccess);
        Assert.Equal("Salary", result.Value.Name);
    }

    [Fact]
    public async Task AddType_TypeIsNotUniqe_ShouldFail()
    {
        var (service, unit, factoryOperationsMock, factoryTypesMock) = CreateServiceAndUnit();

        NameTypeOfIncomes newTypeOfIncomes = factoryTypesMock.Object.Create("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        var dto = new NameTypeOfOperationsDto
        {
            NameOfType = "Salary"
        };

        var result = await service.AddType(dto);

        Assert.True(result.IsFailure);
        Assert.Equal($"Name {dto.NameOfType} is already exists, try another name", result.Error);
    }

    [Fact]
    public async Task Update_TypeExist_ShouldUpdate()
    {
        var (service, unit, factoryOperationsMock, factoryTypesMock) = CreateServiceAndUnit();

        NameTypeOfIncomes newTypeOfIncomes = factoryTypesMock.Object.Create("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        Income newIncome = factoryOperationsMock.Object.Create(DateTime.Now, 300, "Salary", "fsdfssadfsd");

        await unit.incomeRepository.AddAsync(newIncome);

        await unit.SaveChangesAsync();

        var dto = new NameTypeOfOperationsDto
        {
            NameOfType = "My Salary"
        };

        var result = await service.Update(dto, newTypeOfIncomes.Id);

        var income = await unit.incomeRepository.GetAll().FirstAsync(i => i.Type == "My Salary");
        Assert.True(result.IsSuccess);
        Assert.Equal("My Salary", income.Type);
    }

    [Fact]
    public async Task Update_TypeIsNotExist_ShouldFail()
    {
        var (service, unit, factoryOperationsMock, factoryTypesMock) = CreateServiceAndUnit();

        NameTypeOfIncomes newTypeOfIncomes = factoryTypesMock.Object.Create("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        Income newIncome = factoryOperationsMock.Object.Create(DateTime.Now, 300, "Salary", "fsdfssadfsd");

        await unit.incomeRepository.AddAsync(newIncome);

        await unit.SaveChangesAsync();

        var dto = new NameTypeOfOperationsDto
        {
            NameOfType = "My Salary"
        };

        var result = await service.Update(dto, 9999);

        var income = await unit.incomeRepository.GetAll().FirstAsync(i => i.Type == "Salary");

        Assert.True(result.IsFailure);
        Assert.Equal("Salary", income.Type);
        Assert.Equal("Such type of incomes does not exist", result.Error);
    }

    [Fact]
    public async Task Delete_TypeExist_ShouldDelete()
    {
        var (service, unit, factoryOperationsMock, factoryTypesMock) = CreateServiceAndUnit();

        NameTypeOfIncomes newTypeOfIncomes = factoryTypesMock.Object.Create("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        Income newIncome = factoryOperationsMock.Object.Create(DateTime.Now, 300, "Salary", "fsdfssadfsd");

        await unit.incomeRepository.AddAsync(newIncome);

        await unit.SaveChangesAsync();

        var result = await service.Delete(newTypeOfIncomes.Id);

        var typeExist = await unit.typeOfIncomesRepository.GetAll().FirstOrDefaultAsync(t => t.Name == "Salary");
        var incomesExist = await unit.incomeRepository.GetAll().FirstOrDefaultAsync(e => e.Type == "Salary");

        Assert.True(result.IsSuccess);
        Assert.Null(typeExist);
        Assert.Null(incomesExist);
    }

    [Fact]
    public async Task Delete_TypeDoesNotExist_ShouldFail()
    {
        var (service, unit, factoryOperationsMock, factoryTypesMock) = CreateServiceAndUnit();

        NameTypeOfIncomes newTypeOfIncomes = factoryTypesMock.Object.Create("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        Income newIncome = factoryOperationsMock.Object.Create(DateTime.Now, 300, "Salary", "fsdfssadfsd");

        await unit.incomeRepository.AddAsync(newIncome);

        await unit.SaveChangesAsync();

        var result = await service.Delete(9999);

        var typeExist = await unit.typeOfIncomesRepository.GetAll().FirstOrDefaultAsync(t => t.Name == "Salary");
        var incomesExist = await unit.incomeRepository.GetAll().FirstOrDefaultAsync(e => e.Type == "Salary");

        Assert.True(result.IsFailure);
        Assert.NotNull(typeExist);
        Assert.NotNull(incomesExist);
        Assert.Equal("Such type of incomes does not exist", result.Error);
    }
    private (IncomesTypesService service, UnitOfWork unit, Mock<IOperationFactory<Income>> factoryOperationsMock, Mock<INameTypeOfOperationsFactory<NameTypeOfIncomes>> factoryTypesMock) CreateServiceAndUnit()
    {
        var loggerMock = new Mock<ILogger<IncomesTypesService>>();
        var dbContext = GetInMemoryDbContext();
        var factoryOperationsMock = new Mock<IOperationFactory<Income>>();
        factoryOperationsMock.Setup(f => f.Create(It.IsAny<DateTime>(), It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>()))
                            .Returns((DateTime dateOfAction, double amount, string type, string comment) => new Income(dateOfAction, amount, type, comment));
        var factoryTypesMock = new Mock<INameTypeOfOperationsFactory<NameTypeOfIncomes>>();
        factoryTypesMock.Setup(f => f.Create(It.IsAny<string>())).Returns((string name) => new NameTypeOfIncomes(name));
        var repoIncome = new IncomeRepository(dbContext);
        var repoExpense = new ExpenseRepository(dbContext);
        var repoOfTypeIncomes = new TypeOfIncomesRepository(dbContext);
        var repoOfTypeExpenses = new TypeOfExpensesRepository(dbContext);
        var repoUsers = new UserRepository(dbContext);
        var unit = new UnitOfWork(repoExpense, repoIncome, repoOfTypeExpenses, repoOfTypeIncomes, dbContext,repoUsers);
        var service = new IncomesTypesService(unit, loggerMock.Object, factoryTypesMock.Object);
        return (service, unit, factoryOperationsMock, factoryTypesMock);
    }
}