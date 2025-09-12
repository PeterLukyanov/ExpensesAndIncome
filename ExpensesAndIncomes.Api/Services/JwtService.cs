using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Models;

namespace Services;

public class JwtService
{
    private readonly IConfiguration configuration;

    public JwtService(IConfiguration _configuration)
    {
        configuration = _configuration;
    }

    public string GenerateToken(User user)
    {
        //Читаем информацию из файла 
        var jwtSettings = configuration.GetSection("JwtSettings");
        //читаем конкретное поле из файла 
        var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]!);
        //создаём настройщик токена
        var tokenHandler = new JwtSecurityTokenHandler();
        //задаём параметры токена
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            //задаём какие свойства пользователя будут записаны в токен
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            //устанавливаем время действия токена
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiryMinutes"]!)),
            //Создаём криптографический ключ, за счёт секретного ключа из сеттингс
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature)
        };
        //Создаём токен с определёнными настройками
        var token = tokenHandler.CreateToken(tokenDescriptor);
        //Выдаём токен в виде стринга
        return tokenHandler.WriteToken(token);
    }
}

                