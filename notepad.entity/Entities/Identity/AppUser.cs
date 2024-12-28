using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using notepad.entity.Entities.Enum;

namespace notepad.entity.Entities.Identity;

public class AppUser:IdentityUser<string>
{
    public string FullName { get; set; }
    
    public string? ProfilePhoto { get; set; }
  
    public Gender Gender { get; set; }
    public DateOnly  Birthday { get; set;  }
    public ICollection<Note> Notes { get; set; }
    [NotMapped]
    public IFormFile?   FormFile { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenEndDate { get; set; }
}