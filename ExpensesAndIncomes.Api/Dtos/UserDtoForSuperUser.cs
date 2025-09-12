using System.ComponentModel.DataAnnotations;

namespace Dtos;

public class UserDtoForSuperUser
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(30,MinimumLength =3,ErrorMessage ="Username is too short or too long")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(30,MinimumLength =8,ErrorMessage ="Password is too short or too long")]
    public string Password { get; private set; } = null!;
    
    [Required(ErrorMessage = "Role is required")]
    public string Role { get; set; } = "User";
    public UserDtoForSuperUser(string username, string password, string role)
    {
        Username = username;
        Password = password;
        Role = role;
    }
}