namespace Dtos;

public class UserDtoForAuthentication
{
    public string Username { get; set; } = null!;
    public string Password { get; private set; } = null!;
    public UserDtoForAuthentication( string username, string password)
    {
        Username = username;
        Password = password;
    }
}