namespace Models;

////This model is needed to create and manage types in SQL.
public class TypeOfIncomes
{
    public string Name { get; private set; }
    public int Id { get; private set; }
    public TypeOfIncomes(string name)
    {
        Name = name;
    }
}