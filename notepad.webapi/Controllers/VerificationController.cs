using System.Net;
using Microsoft.AspNetCore.Mvc;
using notepad.business.Abstract;
using notepad.business.Dto_s.Tokens;
using notepad.business.Validator;
using Exception = System.Exception;

namespace notepad.webapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VerificationController : ControllerBase
{


    readonly private IVerificationCodeService _verificationCodeService;

    public VerificationController(IVerificationCodeService verificationCodeService)
    {
        _verificationCodeService = verificationCodeService;
    }


    [HttpPost]
    public async Task<IActionResult> Index([FromBody]CreateVerificationDto model)
    {
        try
        {
             await _verificationCodeService.VerificationCodeAsync(model);
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