using Microsoft.Extensions.DependencyInjection;
using notepad.business.Abstract;
using notepad.business.Concrete;

namespace notepad.business;

public static class ServiceRegistration
{
    public static void BusinessServiceRegistration(this IServiceCollection service)
    {
        service.AddMemoryCache();

        service.AddScoped<IAuthService, AuthService>();
        service.AddScoped<IFileService, FileService>();
        service.AddScoped<INoteService, NoteService>();
    
        service.AddScoped<IVerificationCodeService, VerificationCodeService>();

        service.AddScoped<IUserService, UserService>();
        service.AddTransient<IMailService, MailService>();
     
        service.AddTransient<ITokenHandler, TokenHandler>();
    }
}