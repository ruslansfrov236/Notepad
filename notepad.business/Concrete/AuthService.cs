using Microsoft.AspNetCore.Identity;
using notepad.business.Abstract;
using notepad.business.Dto_s.Auth;
using notepad.business.Dto_s.Tokens;
using notepad.business.Validator;
using notepad.entity.Entities.Identity;

namespace notepad.business.Concrete;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMailService _mailService;
    private readonly ITokenHandler _tokenHandler;
    private readonly SignInManager<AppUser> _signInManager;


    public AuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenHandler tokenHandler,
        IUserService userService, IMailService mailService)
    {
        _userManager = userManager;
       
       
        _signInManager = signInManager;
        _tokenHandler = tokenHandler;
        _userService = userService;
        _mailService = mailService;
    }
    
    public async Task<Token> LoginAsync(string password, string usernameOrEmail, int accessTokenLifeTime)
    {
        AppUser? user = await _userManager.FindByNameAsync(usernameOrEmail) ??
            await _userManager.FindByEmailAsync(usernameOrEmail)??
            throw new NotFoundException("User or email not found.");

       
        var emailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        if (!emailConfirmed)
            throw new BadRequestException("Email not verified");

        SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (result.Succeeded)
        {
            Token token = await _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
            await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 900);

            return token;
        }

        throw new PasswordChangeFailedException("Invalid username, email address, or password");
    }

    public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
    {
        AppUser? user = _userManager.Users.FirstOrDefault(u => u.RefreshToken == refreshToken);

        if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
        {

            Token token = await _tokenHandler.CreateAccessToken(15, user);
            await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 1500);
            return token;
        }

        throw new NotFoundException("not found");
    }

    public async Task PasswordResetAsnyc(string email)
    {
        AppUser? user = await _userManager.FindByEmailAsync(email) ?? throw new BadRequestException("Not found users");

        if(user!=null){
            string resetToken =await _userManager.GeneratePasswordResetTokenAsync(user);
            await _mailService.SendPasswordResetMailAsync(email,  user.Id , resetToken);
        }
    }
}
