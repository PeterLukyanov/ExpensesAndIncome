
using Db;
using Microsoft.EntityFrameworkCore;
using CSharpFunctionalExtensions;
using UoW;

namespace Services;

public class TotalSummService
{
    private readonly IUnitOfWork unit;
    public TotalSummService(IUnitOfWork _unit)
    {
        unit = _unit;
    }
    public async Task<Result<double>> TotalBalance()
    {
        bool existExpenses = await unit.expenseRepository.GetAll().AnyAsync();
        bool existIncomes = await unit.incomeRepository.GetAll().AnyAsync();
        if (existExpenses || existIncomes)
        {
            double totalIncomes = await unit.incomeRepository.GetAll().Select(e => e.Amount).SumAsync();
            double totalExpenses = await unit.expenseRepository.GetAll().Select(e => e.Amount).SumAsync();
            return Result.Success(totalIncomes - totalExpenses);
        }
        else
            return Result.Failure<double>("There are no expense or incomes");
    }

}