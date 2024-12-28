using notepad.entity.Entities.Common;

namespace notepad.entity;
public class VerificationCode:BaseEntity
{
    public string UserId { get; set; }
    
    public string Code { get; set; }
    public DateTime ExpiryTime { get; set; }
}