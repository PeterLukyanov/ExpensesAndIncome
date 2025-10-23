using Models;

namespace Factorys;

public interface IOperationFactory<T> where T : Operation
{
    T Create(DateTime dateOfAction, double amount, string type, string comment);
}