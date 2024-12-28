using notepad.business.Dto_s.Tokens;

namespace notepad.business.Abstract;

public interface IAuthService
{
    Task<Token> LoginAsync(string password, string usernameOrEmail, int accessTokenLifeTime);
    Task<Token> RefreshTokenLoginAsync(string refreshToken);
 
    Task PasswordResetAsnyc(string email);
 
}