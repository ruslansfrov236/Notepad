using notepad.business.Dto_s.Tokens;
using notepad.entity.Entities.Identity;

namespace notepad.business.Abstract;

public interface ITokenHandler
{
    Task<Token> CreateAccessToken(int second, AppUser appUser);
    string CreateRefreshToken();
}