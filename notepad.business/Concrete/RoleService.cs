using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using notepad.business.Abstract;
using notepad.business.Validator;
using notepad.entity.Entities.Enum;
using notepad.entity.Entities.Identity;

namespace notepad.business.Concrete;

public class RoleService:IRoleService
{
    readonly private RoleManager<AppRole> _roleManager;

    public RoleService(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<ICollection<AppRole>> GetAll()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return roles;
    }

    public async Task<AppRole> GetById(string id)
    {
        var roles = await _roleManager.FindByIdAsync(id) ?? throw new BadRequestException("Roles values is not found");

        return roles;
    }

    public async Task<bool> Create(string name, RoleModel roleModel)
    {
        AppRole role = new AppRole()
        {
            Id = Guid.NewGuid().ToString(),
            Name = name,
            RoleModel = roleModel
        };

        await _roleManager.CreateAsync(role);
        return true;
    }

    public async Task<bool> Update(string id, string name, RoleModel roleModel)
    {
        var roles = await _roleManager.FindByIdAsync(id) ?? throw new BadRequestException("Roles values is not found");

        roles.Name = name;
        roles.RoleModel = roleModel;
        await _roleManager.UpdateAsync(roles);
        return true;
    }

    public async Task<bool> Delete(string id)
    {
        var roles = await _roleManager.FindByIdAsync(id) ?? throw new BadRequestException("Roles values is not found");
        await _roleManager.DeleteAsync(roles);
        return true;
    }
}