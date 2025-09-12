using UoW;
using Models;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Dtos;

namespace Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork unit;
    private readonly JwtService jwtService;
    private readonly ILogger<AuthService> logger;

    public AuthService(IUnitOfWork _unit, JwtService _jwtService, ILogger<AuthService> _logger)
    {
        unit = _unit;
        jwtService = _jwtService;
        logger = _logger;
    }

    public async Task<Result<User>> AuthenticateAsync(UserDtoForAuthentication userDto)
    {
        logger.LogInformation("Executing a query to authenticate a user");
        var userExist = await unit.userRepository.GetAll().FirstOrDefaultAsync(u => u.Username == userDto.Username && u.Password == userDto.Password);
        if (userExist == null)
        {
            logger.LogWarning("Username or password is incorrect");
            return Result.Failure<User>("Username or password is incorrect");
        }
        return Result.Success(userExist);
    }

    public Task<string> GenerateTokenAsync(User user)
    {
        logger.LogInformation("Token is generated");
        return Task.FromResult(jwtService.GenerateToken(user));
    }
}