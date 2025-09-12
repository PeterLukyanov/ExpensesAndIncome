using Models;
using CSharpFunctionalExtensions;
using Dtos;

namespace Services;

public interface IUserManagementService
{
    Task LoadSuperUser();
    Task<Result<IEnumerable<User>>> GetAllUsersAsync();
    Task<Result<User>> CreateUserAsync(UserDtoForSuperUser userDto);
    Task<Result<User>> UpdateUserAsync(UserDtoForSuperUser userDto, int id);
    Task<Result<User>> DeleteUserAsync(int id);
}