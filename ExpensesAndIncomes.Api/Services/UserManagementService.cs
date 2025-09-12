using UoW;
using Dtos;
using Models;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class UserManagementService : IUserManagementService
{
    private readonly IUnitOfWork unit;
    private readonly ILogger<UserManagementService> logger;

    public UserManagementService(IUnitOfWork _unit, ILogger<UserManagementService> _logger)
    {
        unit = _unit;
        logger = _logger;
    }

    public async Task LoadSuperUser()
    {
        logger.LogInformation("Check if there is a superuser in the database");
        var superUserExist = await unit.userRepository.GetAll().AnyAsync();
        if (!superUserExist)
        {
            logger.LogInformation("Superuser is created");
            await unit.userRepository.AddAsync(
                new User("SuperUser", "11111111", "SuperUser"));
            await unit.SaveChangesAsync();
        }
        else
            logger.LogInformation("SuperUser exist");
    }

    public async Task<Result<IEnumerable<User>>> GetAllUsersAsync()
    {
        logger.LogInformation("Executing a query to show all users");
        if (await unit.userRepository.GetAll().AnyAsync())
        {
            return Result.Success<IEnumerable<User>>(await unit.userRepository.GetAll().ToListAsync());
        }
        else
        {
            logger.LogWarning("There are no users");
            return Result.Failure<IEnumerable<User>>("There are no users");
        }
    }

    public async Task<Result<User>> CreateUserAsync(UserDtoForSuperUser userDto)
    {
        logger.LogInformation("Executing a query to create user");
        var userExist = await unit.userRepository.GetAll().FirstOrDefaultAsync(u => u.Username.ToLower() == userDto.Username.Trim().ToLower());
        if (userExist != null)
        {
            logger.LogWarning($"A user with {userDto.Username} name is already exists");
            return Result.Failure<User>($"A user with {userDto.Username} name is already exists");
        }
        else if (userDto.Role != "User" && userDto.Role != "SuperUser")
        {
            logger.LogWarning("Select the User or SuperUser role");
            return Result.Failure<User>("Select the User or SuperUser role");
        }
        User newUser = new User(userDto.Username, userDto.Password, userDto.Role);
        await unit.userRepository.AddAsync(newUser);
        await unit.SaveChangesAsync();
        return Result.Success(newUser);
    }

    public async Task<Result<User>> UpdateUserAsync(UserDtoForSuperUser userDto, int id)
    {
        logger.LogInformation("Executing a query to update user information");
        var userExist = await unit.userRepository.GetAll().FirstOrDefaultAsync(u => u.Id == id);
        if (userExist == null)
        {
            logger.LogWarning($"A user with {id} ID does not exist");
            return Result.Failure<User>($"A user with {id} ID does not exist");
        }
        userExist.ChangeUsername(userDto.Username);
        userExist.ChangePassword(userDto.Password);
        userExist.ChangeRole(userDto.Role);
        await unit.SaveChangesAsync();
        return Result.Success(userExist);
    }

    public async Task<Result<User>> DeleteUserAsync(int id)
    {
        logger.LogInformation("Executing a query to delete user");
        var userExist = await unit.userRepository.GetAll().FirstOrDefaultAsync(u => u.Id == id);
        if (userExist == null)
        {
            logger.LogWarning($"A user with {id} ID does not exist");
            return Result.Failure<User>($"A user with {id} ID does not exist");
        }
        else
        {
            unit.userRepository.Remove(userExist);
            await unit.SaveChangesAsync();
            return Result.Success(userExist);
        }
    }
}