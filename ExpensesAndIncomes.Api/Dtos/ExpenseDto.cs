using System.ComponentModel.DataAnnotations;

namespace Dtos;

public class ExpenseDto
{
    [Required(ErrorMessage = "Amount is required")]
    [Range(0.0001, 1000000000, ErrorMessage = "Amount need to be positive and from 0.0001 to 1000000000")]
    public double Amount { get; set; }
    [Required(ErrorMessage = "Type is required")]
    public string TypeOfExpenses { get; set; } = null!;
    public string Comment { get; set; } = null!;
}