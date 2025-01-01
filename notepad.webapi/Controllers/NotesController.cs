using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using notepad.business.Abstract;
using notepad.business.Dto_s.Notes;
using notepad.business.Validator;
using notepad.entity.Entities.Enum;
using NuGet.Protocol;

namespace notepad.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    [Authorize(AuthenticationSchemes = "Admin", Roles = "Admin, Manager, User")]


    public class NotesController: ControllerBase
    {
        readonly private INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        
        public async Task<IActionResult> Index()
        {
            var note = await _noteService.GetAllAsync();

            return Ok(note);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Index(string id)
        {
            try
            {
                var note = await _noteService.GetById(id);

                return Ok(note);
            }
            catch (NotFoundException e)
            {
                return NotFound( new {Error =e.Message}); 
            }
            catch (Exception e)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, new { Error = e.Message });
            }
        }


       [HttpPost]
        public async Task<IActionResult> Create(CreateNoteDto model)
        {
            try
            {
                await _noteService.Create(model);
                return StatusCode((int)HttpStatusCode.Created);
            }
            catch (NotFoundException e)
            {
                return NotFound(e);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

       [HttpPut]
        public async Task<IActionResult> Update(UpdateNoteDto model)
        {
            try
            {
                await _noteService.Update(model);
                return Ok();
            }
            catch (NotFoundException e)
            {
                return NotFound(e);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPut("deletedays/{id}")]
        public async Task<IActionResult> DeleteDays(string id)
        {
            await _noteService.DeleteDays(id);
            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpPut("updaterange")]
        public async Task<IActionResult> UpdateRange(bool isDeleted)
        {
            await _noteService.UpdateRange(isDeleted);
            return Ok();
        }
        [HttpDelete("deleterange")]
        public async Task<IActionResult> DeleteRange(bool isDeleted)
        {
            await _noteService.DeleteRange(isDeleted);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _noteService.Delete(id);
            return Ok();
        }
    }
}
