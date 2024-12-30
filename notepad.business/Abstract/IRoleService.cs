using notepad.entity.Entities.Enum;
using notepad.entity.Entities.Identity;

namespace notepad.business.Abstract;

public interface IRoleService
{
    Task<ICollection<AppRole>> GetAll();
    Task<AppRole> GetById(string id);

    Task<bool> Create(string name, RoleModel roleModel);
    Task<bool> Update(string id ,string name, RoleModel roleModel);
    Task<bool> Delete(string id);

}