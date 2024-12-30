using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using notepad.business.Abstract;
using notepad.business.Dto_s.Auth;
using notepad.business.Validator;
using notepad.entity.Entities.Enum;
using notepad.entity.Entities.Identity;

namespace notepad.business.Concrete;

public class UserService : IUserService
{
    readonly private UserManager<AppUser> _userManager;
    readonly private RoleManager<AppRole> _roleManager;
    readonly private IFileService _fileService;
    readonly private IMailService _mailService;
    readonly private IVerificationCodeService _verificationCodeService;

    public UserService(UserManager<AppUser> userManager, IMailService mailService, RoleManager<AppRole> roleManager,
        IFileService fileService, IVerificationCodeService verificationCodeService)
    {
        _userManager = userManager;
        _mailService = mailService;
        _roleManager = roleManager;
        _fileService = fileService;
        _verificationCodeService = verificationCodeService;
    }

    public async Task<CreateUserResponse> CreateAsync(CreateRegistrationDto model)
    {
        if (string.IsNullOrWhiteSpace(model.Email))
            throw new ValidationException("Email cannot be empty.");


        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
            throw new AuthenticationException("Email already exists.");


        AppUser user = new AppUser
        {
            Id = Guid.NewGuid().ToString(),
            FullName = model.FullName,
            UserName = model.FullName,
            Gender = model.Gender,
            Birthday = model.Birthday,
            Email = model.Email
        };


        if (model.FormFile == null)
        {
            user.ProfilePhoto = model.Gender == Gender.Male ? "male.png" :
                model.Gender == Gender.Female ? "female.png" : "default.png";
        }
        else
        {
            if (!_fileService.IsImage(model.FormFile))
            {
                throw new InvalidFileTypeException("The uploaded file is not a valid image.");
            }

            if (_fileService.CheckSize(model.FormFile, 256))
            {
                throw new InvalidDataException("The uploaded file exceeds the maximum size of 256KB.");
            }

            var newImage = await _fileService.UploadAsync(model.FormFile);
            user.ProfilePhoto = newImage;
        }

      
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                throw new InvalidOperationException(error.Description);
            }
        }


        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            var code = _verificationCodeService.GenerateVerificationCode();
   
            await _verificationCodeService.SaveVerificationCodeAsync(user.Id, code);
            await _mailService.SendVerificationCodeEmailAsync(user.Email, code, user.FullName);
            
        }

        return new CreateUserResponse { Succeeded = true, Message = "User created successfully." };
    }


    public async Task<bool> UpdateRefreshToken(string refreshToken, AppUser user, DateTime refreshTokenDate,
        int addOnAccessTokenDate)
    {
        if (user != null)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenEndDate = refreshTokenDate.AddSeconds(addOnAccessTokenDate);

            await _userManager.UpdateAsync(user);
        }
        else
        {
            throw new NotFoundException("Users not found");
        }


        return true;
    }

    public async Task UpdatedPassword(string userId, string resetToken, string newPassword)
    {
        AppUser user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user);
            }
            else
            {
                throw new PasswordChangeFailedException();
            }
        }
    }

    public async Task<List<AppUser>> GetAllUsersAsync(int page, int size)
    {
        var user = await _userManager.Users.Skip(page * size).Take(size).ToListAsync();

        return user;
    }

    public int TotalUsersCount => _userManager.Users.Count();

    public async Task AssignRoleToUserAsync(string userId, string[] Roles)
    {
        AppUser user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, Roles);
        }
    }

    public async Task<string[]> GetRoleToUserAsync(string userIdOrName)
    {
        AppUser? user = await _userManager.FindByIdAsync(userIdOrName);
        if (user == null)
            user = await _userManager.FindByNameAsync(userIdOrName);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.ToArray();
        }

        throw new NotFoundException("User not found ");
    }


    public async Task<bool> AssignRoleDeleteUser(string userId)
    {
        AppUser? user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);


            foreach (var roleName in userRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, roleName);
            }

            return true;
        }

        return false;
    }
}