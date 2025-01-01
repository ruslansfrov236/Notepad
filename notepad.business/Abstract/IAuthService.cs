using notepad.business.Dto_s.Auth;
using notepad.business.Dto_s.Tokens;
using notepad.entity.Entities.Identity;

namespace notepad.business.Abstract;

public interface IAuthService
{
    Task<Token> LoginAsync(LoginDto model);
    Task<Token> RefreshTokenLoginAsync(string refreshToken);
    Task PasswordResetAsnyc(string email);
 
}