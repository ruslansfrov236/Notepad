using notepad.business.Dto_s.Auth;
using notepad.business.Dto_s.Tokens;

namespace notepad.business.Abstract;

public interface IVerificationCodeService
{
    Task SaveVerificationCodeAsync(string userId, string code);
    Task VerificationCodeAsync(CreateVerificationDto model);
    Task Remove(string id);
    Task RemoveRange();
    Task<bool>  EmailConfiremed(string userId);
    string GenerateVerificationCode(int length = 6);
}