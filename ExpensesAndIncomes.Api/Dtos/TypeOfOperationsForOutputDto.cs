using Models;

namespace Dtos;

public class TypeOfOperationsForOutputDto<T> where T : Operation
{
    public double TotalSummOfType { get; set; }
    public List<T> listOfOperations { get; set; } = null!;
    public string NameOfType { get; set; } = null!;
    public TypeOfOperationsForOutputDto(string nameOfType)
    {
        TotalSummOfType = 0;
        listOfOperations = new List<T>();
        NameOfType = nameOfType;
    }
}