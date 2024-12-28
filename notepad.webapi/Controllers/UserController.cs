using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using notepad.business.Abstract;
using notepad.business.Dto_s.Auth;
using notepad.business.Validator;

namespace notepad.webapi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    readonly private IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page , int size)
    {
        var user = await _userService.GetAllUsersAsync(page, size);
        return Ok(user);
    }

    [HttpGet("GetRoleToUserAsync")]
    public async Task<IActionResult> Index(string userIdOrName)
    {
        var roles = await _userService.GetRoleToUserAsync(userIdOrName);
        return Ok(roles);
    }
    [HttpPost("UpdatedPassword")]
    public async Task<IActionResult> UpdatedPassword(string userId, string resetToken, string newPassword)
    {
        var user =  _userService.UpdatedPassword(userId , resetToken, newPassword);
        return Ok(user);
    }

    [HttpPost("registration")]
    public async Task<IActionResult> Index([FromForm]CreateRegistrationDto model)
    {
        try
        {
            await _userService.CreateAsync(model);
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