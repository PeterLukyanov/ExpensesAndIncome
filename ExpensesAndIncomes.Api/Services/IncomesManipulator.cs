using Dtos;
using Microsoft.EntityFrameworkCore;
using Models;
using CSharpFunctionalExtensions;
using UoW;

namespace Services;

//This service is for getting information on ID, deleting, or creating new income.

public class IncomesManipulator
{
    private readonly IUnitOfWork unit;
    private readonly ILogger<IncomesManipulator> logger;

    public IncomesManipulator(IUnitOfWork _unit, ILogger<IncomesManipulator> _logger)
    {
        logger = _logger;
        unit = _unit;
    }

    //Displays a list of all incomes
    public async Task<Result<List<Income>>> InfoOfIncomes()
    {
        logger.LogInformation("Executing a query to list all incomes");
        var list = await unit.incomeRepository.GetAll().ToListAsync();

        if (list.Count == 0)
        {
            logger.LogWarning("There are no incomes yet");
            return Result.Failure<List<Income>>("There are no Incomes for now");
        }
        else
        {
            return Result.Success(list);
        }
    }

    //Adds income to existing income categories
    public async Task<Result<Income>> AddNewIncome(IncomeDto dto)
    {
        logger.LogInformation($"Executing a query to add new income with {dto.TypeOfIncomes} type");
        var typeList = await unit.typeOfIncomesRepository.GetAll().FirstOrDefaultAsync(t => t.Name == dto.TypeOfIncomes);
        if (typeList == null)
        {
            logger.LogWarning($"{dto.TypeOfIncomes} type of incomes was not found");
            return Result.Failure<Income>("This type of Incomes was not found");
        }

        Income newIncome = new Income(DateTime.Now, dto.Amount, dto.TypeOfIncomes, dto.Comment);

        await unit.incomeRepository.AddAsync(newIncome);
        await unit.SaveChangesAsync();
        return Result.Success(newIncome);
    }

    //Update a specific income by Id
    public async Task<Result<Income>> Update(IncomeDto dto, int Id)
    {
        logger.LogInformation($"Executing a query to update income by ({Id})ID");
        var income = await unit.incomeRepository.GetAll().FirstOrDefaultAsync(e => e.Id == Id);
        if (income == null)
        {
            logger.LogWarning($"There is no income with this ({Id})ID");
            return Result.Failure<Income>($"There is no income with this ({Id})ID");
        }
        income.UpdateAmount(dto.Amount);
        income.UpdateComment(dto.Comment);
        income.UpdateTypeOfIncomes(dto.TypeOfIncomes);

        await unit.SaveChangesAsync();

        return Result.Success(income);
    }

    //Deletes a specific income by ID
    public async Task<Result<Income>> Delete(int id)
    {
        logger.LogInformation($"Executing a query to delete income by ({id})ID");
        var item = await unit.incomeRepository.GetAll().FirstOrDefaultAsync(c => c.Id == id);
        if (item == null)
        {
            logger.LogWarning($"There is no income with this ({id})ID");
            return Result.Failure<Income>($"There is no income with this ({id})ID");
        }
        unit.incomeRepository.Remove(item);
        await unit.SaveChangesAsync();
        return Result.Success(item);
    }
}