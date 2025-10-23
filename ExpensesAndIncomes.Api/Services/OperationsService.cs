using Models;
using UoW;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Repositorys;
using Dtos;
using Factorys;

namespace Services;

public abstract class OperationsService<TType, TOperation> 
                        where TOperation : Operation
                        where TType : NameTypeOfOperations
{
    protected readonly IUnitOfWork _unit;
    protected readonly ILogger<OperationsService<TType, TOperation>> _logger;
    protected readonly IOperationFactory<TOperation> _factory; 
    public OperationsService(IUnitOfWork unit, ILogger<OperationsService<TType, TOperation>> logger, IOperationFactory<TOperation> factory)
    {
        _unit = unit;
        _logger = logger;
        _factory = factory;
    }
    protected abstract IOperationRepository<TOperation> operationRepository { get; }
    protected abstract ITypeOfOperationRepository<TType> typeOfOperationRepository{ get; }
    string nameOfOperation = typeof(TOperation).Name.ToLower();

    //Displays a list of all expenses
    public async Task<Result<List<TOperation>>> InfoOfOperations()
    {
        _logger.LogInformation($"Executing a query to list all {nameOfOperation}s");
        var list = await operationRepository.GetAll().ToListAsync();

        if (list.Count == 0)
        {
            _logger.LogWarning($"There are no {nameOfOperation}s yet");
            return Result.Failure<List<TOperation>>($"There are no {nameOfOperation}s for now");
        }
        else
        {
            return Result.Success(list);
        }
    }

    //Adds expense to existing expense categories
    public async Task<Result<TOperation>> AddNewOperation(OperationDto dto)
    {
        _logger.LogInformation($"Executing a query to add new {nameOfOperation} with {dto.Type} type");
        var typeList = await typeOfOperationRepository.GetAll().Select(t => t.Name).ToListAsync();
        bool typeExist = typeList.Any(t => t == dto.Type);
        if (typeExist)
        {
            TOperation newOperation = _factory.Create(DateTime.Now, dto.Amount, dto.Type, dto.Comment);

            await operationRepository.AddAsync(newOperation);
            await _unit.SaveChangesAsync();
            return Result.Success(newOperation);
        }
        else
        {
            _logger.LogWarning($"{dto.Type} type of {nameOfOperation}s was not found");
            return Result.Failure<TOperation>($"{dto.Type} type of {nameOfOperation}s was not found");
        }
    }

    //Update a specific expense by Id
    public async Task<Result<TOperation>> Update(OperationDto dto, int Id)
    {
        _logger.LogInformation($"Executing a query to update {nameOfOperation} by ({Id}) ID");
        var operation = await operationRepository.GetAll().FirstOrDefaultAsync(e => e.Id == Id);
        if (operation == null)
        {
            _logger.LogWarning($"There is no {nameOfOperation} with this ({Id})ID");
            return Result.Failure<TOperation>($"There is no {nameOfOperation} with this ({Id})ID");
        }
        operation.UpdateAmount(dto.Amount);
        operation.UpdateComment(dto.Comment);
        operation.UpdateType(dto.Type);

        await _unit.SaveChangesAsync();

        return Result.Success(operation);
    }

    //Deletes a specific expense by ID
    public async Task<Result<TOperation>> Delete(int id)
    {
        _logger.LogInformation($"Executing a query to delete {nameOfOperation} by ({id})ID");
        var operation = await operationRepository.GetAll().FirstOrDefaultAsync(o => o.Id == id);
        if (operation == null)
        {
            _logger.LogWarning($"There is no {nameOfOperation} with this ({id})ID");
            return Result.Failure<TOperation>($"There is no {nameOfOperation} with this ({id})ID");
        }
        operationRepository.Remove(operation);
        await _unit.SaveChangesAsync();
        return Result.Success(operation);
    }
}