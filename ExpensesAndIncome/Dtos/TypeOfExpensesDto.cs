using System.ComponentModel.DataAnnotations;

namespace Dtos;

public class TypeOfExpensesDto
{
    [Required(ErrorMessage = "Type is required")]
    [StringLength(50,MinimumLength = 3, ErrorMessage ="Name must be from 3 to 50 letters")]
    public double NameOfType { get; set; }
}