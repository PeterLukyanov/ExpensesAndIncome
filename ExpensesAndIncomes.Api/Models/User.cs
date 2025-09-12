namespace Models;

public class User
{
    public int Id { get; private set; }
    public string Username { get; private set; } = null!;
    public string Password { get; private set; } = null!;
    public string Role { get; private set; } = "User";

    public User( string username, string password, string role)
    {
        Username = username;
        Password = password;
        Role = role;
    }

    public void ChangeUsername(string username)
    {
        Username = username;
    }

    public void ChangePassword(string password)
    {
        Password = password;
    }

    public void ChangeRole(string role)
    {
        Role = role;
    }
}