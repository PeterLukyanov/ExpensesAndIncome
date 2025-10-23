using Db;
using Dtos;
using Microsoft.EntityFrameworkCore;
using Models;
using CSharpFunctionalExtensions;
using UoW;
using Factorys;
using Repositorys;

//This service is for working with a type: it allows you to load start types,
// add a new one, view all types, view all information about a specific type,
// view the total amount of expenses, change a type, delete a type

namespace Services;

public abstract class OperationsTypesService<TType, TOperation>
    where TType : NameTypeOfOperations
    where TOperation : Operation
{
    protected readonly IUnitOfWork _unit;
    protected readonly ILogger<OperationsTypesService<TType, TOperation>> _logger;
    protected readonly INameTypeOfOperationsFactory<TType> _factory;
    public OperationsTypesService(IUnitOfWork unit, ILogger<OperationsTypesService<TType, TOperation>> logger, INameTypeOfOperationsFactory<TType> factory)
    {
        _logger = logger;
        _unit = unit;
        _factory = factory;
    }

    protected abstract ITypeOfOperationRepository<TType> typeOfOperationRepository { get; }
    protected abstract IOperationRepository<TOperation> operationRepository{ get; }

    string nameOfType = typeof(TType).Name;
    string nameOfOperation = typeof(TOperation).Name.ToLower();

    //Checks if there are any expenses in the program, if not, it will load the starting types
    public async Task LoadTypesForStart()
    {
        _logger.LogInformation($"Check if there are {nameOfType}");
        bool operationsList = await typeOfOperationRepository.GetAll().AnyAsync();
        if (!operationsList)
        {
            _logger.LogInformation("Types not found, new ones being created");
            if (typeof(TType).Name == "NameTypeOfExpenses")
            {
                await typeOfOperationRepository.AddAsync(_factory.Create("Food"));
                await typeOfOperationRepository.AddAsync(_factory.Create("Relax"));
                await _unit.SaveChangesAsync();
            }
            else
            {
                await typeOfOperationRepository.AddAsync(_factory.Create("Salary"));
                await typeOfOperationRepository.AddAsync(_factory.Create("Other"));
                await _unit.SaveChangesAsync();
            }
        }
        _logger.LogInformation("Types exist");
    }

    //Displays all types of expenses that are in the program
    public async Task<Result<List<TType>>> InfoTypes()
    {
        _logger.LogInformation("Executing a query to list all types");
        var nameOfTypes = await typeOfOperationRepository.GetAll().ToListAsync();
        if (nameOfTypes.Count == 0)
        {
            _logger.LogWarning($"{nameOfType} not found");
            return Result.Failure<List<TType>>($"There are no {nameOfType} for now");
        }
        return Result.Success(nameOfTypes);
    }

    //Displays information about the type by name, namely: the sum of all expenses by type,
    // and a list of all expense objects
    public async Task<Result<TypeOfOperationsForOutputDto<TOperation>>> GetInfoOfType(int id)
    {
        _logger.LogInformation($"Executing a query to get to type of {nameOfOperation}s with {id} id");
        var nameOfType = await typeOfOperationRepository.GetAll().FirstOrDefaultAsync(t => t.Id == id);
        if (nameOfType != null)
        {
            _logger.LogInformation("Type exist");
            TypeOfOperationsForOutputDto<TOperation> listOfOperations = new TypeOfOperationsForOutputDto<TOperation>(nameOfType.Name);
            listOfOperations.TotalSummOfType=await operationRepository.GetAll()
                .Where(o => o.Type == nameOfType.Name)
                .SumAsync(o => o.Amount);
            listOfOperations.listOfOperations.AddRange(await operationRepository.GetAll()
                .Where(o => o.Type == nameOfType.Name)
                .ToListAsync());
            return Result.Success(listOfOperations);
        }
        else
        {
            _logger.LogWarning($"Type of {nameOfOperation}s with {id} id not found");
            return Result.Failure<TypeOfOperationsForOutputDto<TOperation>>($"Such type of {nameOfOperation}s does not exist");
        }
    }

    //Displays the total amount of all expenses
    public async Task<Result<double>> TotalSumOfOperations()
    {
        _logger.LogInformation($"Executing a query to output the total sum of {nameOfOperation}s");
        bool operationsExist = await operationRepository.GetAll().AnyAsync();
        if (operationsExist)
        {
            return Result.Success(await operationRepository.GetAll().Select(e => e.Amount)
                                                    .SumAsync());
        }
        else
        {
            _logger.LogWarning($"There are no {nameOfOperation}s");
            return Result.Failure<double>($"There are no {nameOfOperation}s");
        }
    }

    //Created types will be stored in a separate SQL table.
    public async Task<Result<TType>> AddType(NameTypeOfOperationsDto typeOfOperationsDto)
    {
        _logger.LogInformation($"Executing a query to add {typeOfOperationsDto.NameOfType} type of {nameOfOperation}s");
        var typeOfOperationsExist = await typeOfOperationRepository.GetAll().FirstOrDefaultAsync(t => t.Name.ToLower() == typeOfOperationsDto.NameOfType.Trim().ToLower());
        if (typeOfOperationsExist == null)
        {
            TType typeOfOperations = _factory.Create(typeOfOperationsDto.NameOfType);
            await typeOfOperationRepository.AddAsync(typeOfOperations);

            await _unit.SaveChangesAsync();

            return Result.Success(typeOfOperations);
        }
        else
        {
            _logger.LogWarning($"Name {typeOfOperationsDto.NameOfType} is already exists, try another name");
            return Result.Failure<TType>($"Name {typeOfOperationsDto.NameOfType} is already exists, try another name");
        }
    }

    //Updates the name of the specified type,
    // while overwriting the name of all expense objects that were marked with the current expense type
    public async Task<Result<TType>> Update(NameTypeOfOperationsDto typeOfOperationsDto, int id)
    {
        _logger.LogInformation($"Executing a query to update a type of {nameOfOperation}s with {id}id");
        var typeExist = await typeOfOperationRepository.GetAll().FirstOrDefaultAsync(t => t.Id == id);
        if (typeExist != null)
        {
            if (typeOfOperationsDto.NameOfType.Trim().ToLower() != typeExist.Name.ToLower())
            {
                _logger.LogInformation($"Type of {nameOfOperation}s exist");
                var listOfOperations = await operationRepository.GetAll().Where(o => o.Type == typeExist.Name).ToListAsync();

                for (int i = 0; i < listOfOperations.Count; i++)
                {
                    listOfOperations[i].UpdateType(typeOfOperationsDto.NameOfType);
                }

                typeExist.UpdateName(typeOfOperationsDto.NameOfType.Trim());
                await _unit.SaveChangesAsync();
                return Result.Success(typeExist);
            }
            else
            {
                _logger.LogWarning($"Type of {operationRepository}s with {typeOfOperationsDto.NameOfType.Trim()} name exist");
                return Result.Failure<TType>($"Type of {operationRepository}s with {typeOfOperationsDto.NameOfType.Trim()} name exist, try another name");
            }
        }
        else
        {
            _logger.LogWarning($"Type of {nameOfOperation}s with {id} id does not exist");
            return Result.Failure<TType>($"Such type of {nameOfOperation}s does not exist");
        }
    }

    //Deletes an expense type, which also deletes all expense objects associated with that type.
    public async Task<Result<TType>> Delete(int id)
    {
        _logger.LogInformation($"Executing a query to delete type of {nameOfOperation}s with {id} id and all {nameOfOperation}s with this type");
        var typeOfOperations = await typeOfOperationRepository.GetAll().FirstOrDefaultAsync(t => t.Id == id);
        if (typeOfOperations == null)
        {
            _logger.LogWarning($"Type of {nameOfOperation}s with {id} id does not exist");
            return Result.Failure<TType>($"Such type of {nameOfOperation}s does not exist");
        }
        var list = await operationRepository.GetAll().Where(o => o.Type == typeOfOperations.Name).ToListAsync();

        operationRepository.RemoveRange(list);
        typeOfOperationRepository.Remove(typeOfOperations);

        await _unit.SaveChangesAsync();

        return Result.Success(typeOfOperations);
    }
}
