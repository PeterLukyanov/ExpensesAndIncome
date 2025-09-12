using Microsoft.AspNetCore.Mvc;
using Services;
using Models;
using Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserManagementService userService;
    private readonly ILogger<UsersController> logger;

    public UsersController(IUserManagementService _userService, ILogger<UsersController> _logger)
    {
        userService = _userService;
        logger = _logger;
    }

    [Authorize(Roles = "SuperUser")]
    [HttpGet("All Users")]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        logger.LogInformation("The service is called to display the entire list of users");
        var result = await userService.GetAllUsersAsync();
        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }
        else
            return Ok(result.Value);
    }

    [Authorize(Roles = "SuperUser")]
    [HttpPost]
    public async Task<IActionResult> CreateUser(UserDtoForSuperUser userDto)
    {
        logger.LogInformation("The service is called to create a new user");
        var result = await userService.CreateUserAsync(userDto);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        else
            return Ok(result.Value);
    }

    [Authorize(Roles = "SuperUser")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser([FromBody] UserDtoForSuperUser userDto, int id)
    {
        logger.LogInformation("The service is called to update user information.");
        var result = await userService.UpdateUserAsync(userDto, id);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        else
        {
            return Ok(result.Value);
        }
    }

    [Authorize(Roles = "SuperUser")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        logger.LogInformation("The service is called to delete the user");
        var result = await userService.DeleteUserAsync(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        else
        {
            return Ok(result.Value);
        }
    }
}