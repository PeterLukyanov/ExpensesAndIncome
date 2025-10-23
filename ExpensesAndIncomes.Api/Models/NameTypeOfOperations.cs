namespace Models;

public abstract class NameTypeOfOperations
{
    public string Name { get; private set; } = null!;
    public int Id { get; private set; }

    public NameTypeOfOperations(string name)
    {
        Name = name;
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
}