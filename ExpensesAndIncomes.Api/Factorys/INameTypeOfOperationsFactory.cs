using Models;

namespace Factorys;

public interface INameTypeOfOperationsFactory<T> where T : NameTypeOfOperations
{
    T Create(string nameOfType);
}