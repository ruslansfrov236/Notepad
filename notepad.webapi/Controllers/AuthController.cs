using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using notepad.business.Abstract;
using notepad.business.Validator;

namespace notepad.webapi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    readonly private IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> Login(string password, string usernameOrEmail, int accessTokenLifeTime)
    {
        try
        {
            var user = await _authService.LoginAsync(password, usernameOrEmail, 900);

            return Ok(user);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
   [HttpPost("token")]
    public async Task<IActionResult> RefreshTokenLoginAsync(string refreshToken)
    {
        var user = await _authService.RefreshTokenLoginAsync(refreshToken);
        return Ok(user);
    }
    
    [HttpPost("PasswordResetAsnyc")]
    public async Task<IActionResult> PasswordResetAsnyc(string email)
    {
        try
        {
            await _authService.PasswordResetAsnyc(email);
            return Ok(new { Message = "Password reset email sent successfully." });
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { Error = ex.Message });
        }
    }

}