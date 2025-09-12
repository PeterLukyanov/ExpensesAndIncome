using Models;
using CSharpFunctionalExtensions;
using Dtos;

namespace Services;

public interface IAuthService
{
    Task<Result<User>> AuthenticateAsync(UserDtoForAuthentication userDto);
    Task<string> GenerateTokenAsync(User user);
}