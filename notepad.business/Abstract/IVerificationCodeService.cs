using notepad.business.Dto_s.Auth;

namespace notepad.business.Abstract;

public interface IVerificationCodeService
{
    Task SaveVerificationCodeAsync(string userId, string code);
    Task<CreateUserResponse> VerificationCodeAsync(string userId, string enteredCode);
    Task Remove(string id);
    string GenerateVerificationCode(int length = 6);
}