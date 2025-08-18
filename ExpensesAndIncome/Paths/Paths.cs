namespace PathsFile;

public static class Paths
{
    public static readonly string ExpensesFileName = "Expenses.json";
    public static readonly string IncomesFileName = "Incomes.json";
    public static readonly string TypeOfExpensesName = "TypeOfExpenses.json";
    public static readonly string TypeOfIncomesName = "TypeOfIncomes.json";
    public static readonly string TypesOfIncomesName = "TypesOfIncomes.json";
    public static readonly string TypesOfExpensesName = "TypesOfExpenses.json";
    public static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;
    public static readonly string FolderForData = Path.Combine(BasePath, "Data");
    public static readonly string CounterFileName = "Counter.json";
}