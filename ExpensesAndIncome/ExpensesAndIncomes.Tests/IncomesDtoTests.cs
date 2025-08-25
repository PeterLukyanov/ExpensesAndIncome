using System.ComponentModel.DataAnnotations;
using Dtos;
using Xunit;

public class IncomesDtoTests
{
    private List<ValidationResult> ValidateDto(object dto)
    {
        var context = new ValidationContext(dto, null, null);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(dto, context, results, true);
        return results;
    }

    [Fact]
    public void IncomesDto_ValidData_ShouldPassValidation()
    {
        var dto = new IncomeDto { TypeOfIncomes = "Salary", Amount = 100d };
        var results = ValidateDto(dto);
        Assert.Empty(results);
    }

    [Fact]
    public void IncomesDto_MissingType_ShouldFailValidation()
    {
        var dto = new IncomeDto { Amount = 150d };
        var results = ValidateDto(dto);
        Assert.Contains(results, r => r.MemberNames.Contains("TypeOfIncomes"));
    }

    [Fact]
    public void IncomesDto_MissingAmount_ShouldFailValidation()
    {
        var dto = new IncomeDto { TypeOfIncomes = "Salary" };
        var results = ValidateDto(dto);
        Assert.Contains(results, r => r.MemberNames.Contains("Amount"));
    }

    [Fact]
    public void IncomesDto_ZeroAmount_ShouldFailValidation()
    {
        var dto = new IncomeDto { TypeOfIncomes = "Salary", Amount = 0 };
        var results = ValidateDto(dto);
        Assert.Contains(results, r => r.MemberNames.Contains("Amount"));
    }
    [Fact]
    public void IncomesDto_NegativeAmount_ShouldFailValidation()
    {
        var dto = new IncomeDto { TypeOfIncomes = "Salary", Amount = -150d };
        var results = ValidateDto(dto);
        Assert.Contains(results, r => r.MemberNames.Contains("Amount"));
    }
}