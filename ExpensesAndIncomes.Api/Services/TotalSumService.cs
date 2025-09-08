using Microsoft.EntityFrameworkCore;
using CSharpFunctionalExtensions;
using UoW;

namespace Services;

public class TotalSumService
{
    private readonly IUnitOfWork unit;
    private readonly ILogger<TotalSumService> logger;
    public TotalSumService(IUnitOfWork _unit, ILogger<TotalSumService> _logger)
    {
        logger = _logger;
        unit = _unit;
    }
    public async Task<Result<double>> TotalBalance()
    {
        logger.LogInformation("Executing a query to calculate total balance");
        bool existExpenses = await unit.expenseRepository.GetAll().AnyAsync();
        bool existIncomes = await unit.incomeRepository.GetAll().AnyAsync();
        if (existExpenses || existIncomes)
        {
            double totalIncomes = await unit.incomeRepository.GetAll().Select(e => e.Amount).SumAsync();
            double totalExpenses = await unit.expenseRepository.GetAll().Select(e => e.Amount).SumAsync();
            return Result.Success(totalIncomes - totalExpenses);
        }
        else
        {
            logger.LogWarning("There are no expense and incomes");
            return Result.Failure<double>("There are no expense and incomes");
        }
    }

}