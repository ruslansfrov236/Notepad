using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using notepad.entity.Entities.Identity;

namespace notepad.business.Dto_s.Notes;

public class CreateNoteDto
{
    public string Title { get; set;  }
    public string Description { get; set;  }
    public DateTime NotifiedDateTime { get; set; }
    public bool isNotified { get; set; }
  
    public string AppUserId { get; set; }
    
    public string? VideoName { get; set;  }
    public string? PhotoName { get; set; }
    
    [NotMapped]
    public IFormFile? FormFile { get; set; }
}