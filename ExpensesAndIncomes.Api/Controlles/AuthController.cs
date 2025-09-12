using Dtos;
using Microsoft.AspNetCore.Mvc;
using Services;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;
    private readonly ILogger<AuthController> logger;

    public AuthController(IAuthService _authService, ILogger<AuthController> _logger)
    {
        authService = _authService;
        logger = _logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDtoForAuthentication userDto)
    {
        logger.LogInformation("Calling a service to perform an authentication request");
        var result = await authService.AuthenticateAsync(userDto);
        if (result.IsFailure)
        {
            logger.LogWarning(result.Error);
            return Unauthorized(result.Error);
        }
        else
        {
            logger.LogInformation("Call the service to create a token for the user");
            var token = await authService.GenerateTokenAsync(result.Value);
            return Ok(new{Token = token});
        }
    }
}