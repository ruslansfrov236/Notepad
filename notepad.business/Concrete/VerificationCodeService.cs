using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using notepad.app.Abstract;
using notepad.business.Abstract;
using notepad.business.Dto_s.Tokens;
using notepad.business.Validator;
using notepad.entity;
using notepad.entity.Entities.Identity;

public class VerificationCodeService:IVerificationCodeService
{
    private readonly IVerificationReadRepository _verificationReadRepository;
    private readonly IVerificationWriteRepository _verificationWriteRepository;
    private readonly UserManager<AppUser> _userManager;
 
    public VerificationCodeService(
        IVerificationReadRepository verificationReadRepository,
        IVerificationWriteRepository verificationWriteRepository,
        UserManager<AppUser> userManager)
    {
        _verificationReadRepository = verificationReadRepository;
        _verificationWriteRepository = verificationWriteRepository;
        _userManager = userManager;
    }
 
    public async Task VerificationCodeAsync(CreateVerificationDto model)
    {
        try
        {
          
            var cachedCode = await _verificationReadRepository.GetSingle(
                a => a.UserId == model.UserId && a.Code == model.Code);
           
            if (cachedCode == null)
            {
                throw new BadRequestException("The verification code is incorrect or does not exist.");
            }
 
            if (DateTime.UtcNow.ToLocalTime() > cachedCode.ExpiryTime)
            {
                
                
                await Remove(cachedCode.Id.ToString());
                
                throw new BadRequestException("The verification code has expired.");
            }
 
            var isConfirmed = await EmailConfiremed(cachedCode.UserId);
 
            if (isConfirmed)
            {
                await RemoveRange();
            }
        }
        catch (BadRequestException ex)
        {
            
            throw new BadRequestException(ex.Message);
        }
        catch (Exception ex)
        {
           
            throw new BadRequestException($"Error in VerificationCodeAsync: {ex.Message}");
          
        }
    }

    public async Task RemoveRange()
    {
        var filter = await _verificationReadRepository.GetWhere(a =>
           DateTime.UtcNow.ToLocalTime() > a.ExpiryTime).ToListAsync();

        await _verificationWriteRepository.DeleteRange(filter);
        await _verificationWriteRepository.SaveAsync();
    }

    public async Task<bool> EmailConfiremed(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
                   ?? throw new BadRequestException("User not found");
 
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
 
      
        var confirmResult = await _userManager.ConfirmEmailAsync(user, token);

      
        return true;
    }
 
    public async Task Remove(string id)
    {
        var values = await _verificationReadRepository.GetById(id);
 
        if (values == null)
        {
            throw new BadRequestException("Verification code not found.");
        }
 
        _verificationWriteRepository.Remove(values);
        await _verificationWriteRepository.SaveAsync();
    }
 
    public async Task SaveVerificationCodeAsync(string userId, string code)
    {
        VerificationCode verificationCode = new VerificationCode()
        {
            UserId = userId,
            Code = code,
            ExpiryTime = DateTime.UtcNow.ToLocalTime().AddMinutes(60) 
        };
 
        await _verificationWriteRepository.AddAsync(verificationCode);
        await _verificationWriteRepository.SaveAsync();
    }
 
    public string GenerateVerificationCode(int length = 6)
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}