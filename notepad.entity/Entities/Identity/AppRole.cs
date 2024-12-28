
using Microsoft.AspNetCore.Identity;
using notepad.entity.Entities.Enum;

namespace notepad.entity.Entities.Identity;

public class AppRole:IdentityRole<string>
{
    public RoleModel RoleModel { get; set; }
}