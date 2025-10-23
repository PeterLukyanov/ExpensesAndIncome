using UoW;
using Models;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Dtos;

namespace Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unit;
    private readonly JwtService _jwtService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUnitOfWork unit, JwtService jwtService, ILogger<AuthService> logger)
    {
        _unit = unit;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<Result<User>> AuthenticateAsync(UserDtoForAuthentication userDto)
    {
        _logger.LogInformation("Executing a query to authenticate a user");
        var userExist = await _unit.userRepository.GetAll().FirstOrDefaultAsync(u => u.Username == userDto.Username && u.Password == userDto.Password);
        if (userExist == null)
        {
            _logger.LogWarning("Username or password is incorrect");
            return Result.Failure<User>("Username or password is incorrect");
        }
        return Result.Success(userExist);
    }

    public Task<string> GenerateTokenAsync(User user)
    {
        _logger.LogInformation("Token is generated");
        return Task.FromResult(_jwtService.GenerateToken(user));
    }
}