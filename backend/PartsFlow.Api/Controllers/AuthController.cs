using Microsoft.AspNetCore.Mvc;
using PartsFlow.Api.DTOs;
using PartsFlow.Api.Services;

namespace PartsFlow.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    IAuthService authService,
    IConfiguration configuration,
    IWebHostEnvironment environment) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        if (!IsRegistrationAllowed())
        {
            return StatusCode(StatusCodes.Status403Forbidden, new
            {
                message = "Public registration is disabled."
            });
        }

        try
        {
            var response = await authService.RegisterAsync(request);

            return Ok(response);
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        try
        {
            var response = await authService.LoginAsync(request);

            return Ok(response);
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(new { message = exception.Message });
        }
    }

    private bool IsRegistrationAllowed()
    {
        if (environment.IsDevelopment())
        {
            return true;
        }

        var value = configuration["ALLOW_REGISTRATION"];

        return string.Equals(value, "true", StringComparison.OrdinalIgnoreCase)
            || string.Equals(value, "1", StringComparison.OrdinalIgnoreCase)
            || string.Equals(value, "yes", StringComparison.OrdinalIgnoreCase);
    }
}
