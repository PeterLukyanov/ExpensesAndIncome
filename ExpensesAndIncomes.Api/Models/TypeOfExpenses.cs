namespace Models;

//This model is needed to create and manage types in SQL.
public class TypeOfExpenses
{
    public string Name { get; private set; } = null!;
    public int Id { get; private set; }

    public TypeOfExpenses(string name)
    {
        Name = name;
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
}
    

    