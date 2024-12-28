using notepad.business.Dto_s.Auth;
using notepad.entity.Entities.Identity;

namespace notepad.business.Abstract;

public interface IUserService
{
    Task<CreateUserResponse> CreateAsync(CreateRegistrationDto model);
    Task<bool>  UpdateRefreshToken (string refreshToken, AppUser user, DateTime refreshTokenDate ,int addOnAccessTokenDate);
    Task UpdatedPassword(string userId , string resetToken , string newPassword);
    Task<List<AppUser>> GetAllUsersAsync(int page, int size);
   
    int TotalUsersCount {get;}
    Task AssignRoleToUserAsync( string userId , string[] Roles);
    Task<string[]> GetRoleToUserAsync( string userIdOrName);
    Task<bool> AssignRoleDeleteUser(string userId);

}