using System.Net;
using Microsoft.AspNetCore.Mvc;
using notepad.business.Abstract;
using notepad.business.Validator;
using notepad.entity.Entities.Enum;

namespace notepad.webapi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RoleController : Controller
{
    readonly private IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var roles = await _roleService.GetAll();
        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string id)
    {
        try
        {
            var role = await _roleService.GetById(id);
            return Ok(role);
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
    [HttpPost]
    public async Task<IActionResult> Create(string name, RoleModel roleModel)
    {
        await _roleService.Create(name, roleModel);
        return StatusCode((int)HttpStatusCode.Created);
    }

    [HttpPut]
    public async Task<IActionResult> Update(string id, string name, RoleModel roleModel)
    {
        await _roleService.Update(id, name, roleModel);
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id )
    {
        try
        {
            await _roleService.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
}