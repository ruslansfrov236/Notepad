using System.Net;
using Microsoft.AspNetCore.Mvc;
using notepad.business.Abstract;
using notepad.business.Validator;
using Exception = System.Exception;

namespace notepad.webapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VerificationController : ControllerBase
{


    readonly private IAuthService _authService;

    public VerificationController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> Index(string userId, string code)
    {
        try
        {
             await _authService.VerificationCodeAsync(userId, code);
            return Ok();
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

}