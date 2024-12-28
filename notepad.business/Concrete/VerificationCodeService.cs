using System.Net;
using System.Runtime.Caching;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using notepad.app.Abstract;
using notepad.business.Abstract;
using notepad.business.Dto_s.Auth;
using notepad.business.Dto_s.Tokens;
using notepad.business.Validator;
using notepad.entity;
using notepad.entity.Entities.Identity;

namespace notepad.business.Concrete;

public class VerificationCodeService:IVerificationCodeService
{
    readonly private UserManager<AppUser> _userManager;
    readonly private ITokenHandler _tokenHandler;
    readonly private IVerificationReadRepository _verificationReadRepository;
    readonly private IVerificationWriteRepository _verificationWriteRepository;
   
    public VerificationCodeService(UserManager<AppUser> userManager, ITokenHandler tokenHandler, IVerificationReadRepository verificationReadRepository, IVerificationWriteRepository verificationWriteRepository)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _verificationReadRepository = verificationReadRepository;
        _verificationWriteRepository = verificationWriteRepository;
      
    }


public async Task<CreateUserResponse> VerificationCodeAsync(string userId, string enteredCode)
{
    var cachedCode = await _verificationReadRepository.GetSingle(a => a.UserId == userId && a.Code == enteredCode);

    if (cachedCode == null)
        throw new BadRequestException("The verification code is incorrect or does not exist.");

    if (DateTime.UtcNow.ToLocalTime() > cachedCode.ExpiryTime)
    {
        await Remove(cachedCode.Id.ToString());
        throw new BadRequestException("The verification code has expired.");
    }
    else
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            throw new BadRequestException("User not found.");

       
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

       
        var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
        if (!confirmResult.Succeeded)
        {
            foreach (var error in confirmResult.Errors)
            {
                throw new BadRequestException(error.Description);
            }
        }
        else
        {
            await Remove(cachedCode.Id.ToString());
     
        }

        return new CreateUserResponse
        {
            Succeeded = true,
            Message = "Email created successfully."
        };
    }
   
  
}


    public async Task Remove(string id)
    {
        var values = await _verificationReadRepository.GetById(id);

        _verificationWriteRepository.Remove(values);
     await   _verificationWriteRepository.SaveAsync();
    }


    public async Task SaveVerificationCodeAsync(string userId, string code)
    {
      
        VerificationCode verificationCode = new VerificationCode()
        {
            UserId = userId,
            Code = code,
            ExpiryTime = DateTime.UtcNow.AddMinutes(60).ToLocalTime()
        };
        
       


    await    _verificationWriteRepository.AddAsync(verificationCode);
    await    _verificationWriteRepository.SaveAsync();

     

    }

   

    public string GenerateVerificationCode(int length = 6)
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}