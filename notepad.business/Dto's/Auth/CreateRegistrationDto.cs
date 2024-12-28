using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using notepad.entity.Entities;
using notepad.entity.Entities.Enum;

namespace notepad.business.Dto_s.Auth;

public class CreateRegistrationDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string? ProfilePhoto { get; set; }
   
    public Gender Gender { get; set; }
    public DateOnly  Birthday { get; set;  }
    public string Password { get; set; }
    public string RePassword { get; set; }

   
    [NotMapped]
    public IFormFile?   FormFile { get; set; }
}