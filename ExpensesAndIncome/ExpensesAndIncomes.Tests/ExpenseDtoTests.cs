using System.ComponentModel.DataAnnotations;
using Dtos;
using Xunit;

public class ExpenseDtoTests
{
    private List<ValidationResult> ValidateDto(object dto)
    {
        var context = new ValidationContext(dto, null, null);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(dto, context, results, true);
        return results;
    }

    [Fact]
    public void ExpenseDto_ValidData_ShouldPassValidation()
    {
        var dto = new ExpenseDto { TypeOfExpenses = "Food", Amount = 100d };
        var results = ValidateDto(dto);
        Assert.Empty(results);
    }

    [Fact]
    public void ExpenseDto_MissingType_ShouldFailValidation()
    {
        var dto = new ExpenseDto { Amount = 150d };
        var results = ValidateDto(dto);
        Assert.Contains(results, r => r.MemberNames.Contains("TypeOfExpenses"));
    }

    [Fact]
    public void ExpenseDto_MissingAmount_ShouldFailValidation()
    {
        var dto = new ExpenseDto { TypeOfExpenses = "Food" };
        var results = ValidateDto(dto);
        Assert.Contains(results, r => r.MemberNames.Contains("Amount"));
    }

    [Fact]
    public void ExpenseDto_ZeroAmount_ShouldFailValidation()
    {
        var dto = new ExpenseDto { TypeOfExpenses = "Food", Amount = 0 };
        var results = ValidateDto(dto);
        Assert.Contains(results, r => r.MemberNames.Contains("Amount"));
    }
    [Fact]
    public void ExpenseDto_NegativeAmount_ShouldFailValidation()
    {
        var dto = new ExpenseDto { TypeOfExpenses = "Food", Amount = -150d };
        var results = ValidateDto(dto);
        Assert.Contains(results, r => r.MemberNames.Contains("Amount"));
    }

}
