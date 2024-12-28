using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using notepad.app.Abstract;
using notepad.app.Concrete;
using notepad.app.Context;
using notepad.entity.Entities.Identity;

namespace notepad.app;

public static class ServiceRegistration
{
    public static void ApplicationServiceRegistration(this IServiceCollection service)
    {
        service.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.ConnectionString));

        service.AddIdentity<AppUser, AppRole>(policy =>
            {
                // Parol siyasəti
                policy.Password.RequiredLength = 6;
                policy.Password.RequireLowercase = true;
                policy.Password.RequireNonAlphanumeric = false;
                policy.Password.RequireDigit = true;
                policy.Password.RequireUppercase = true;

                // İstifadəçi siyasəti
                policy.User.RequireUniqueEmail = true;


                // Giriş və təsdiq siyasəti
                policy.SignIn.RequireConfirmedAccount = true;
                policy.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        service.AddScoped<IVerificationReadRepository, VerificationCodeReadRepository>();
        service.AddScoped<IVerificationWriteRepository, VerificationCodeWriteRepository>();
        service.AddScoped<INoteReadRepository, NoteReadRepository>();
        service.AddScoped<INoteWriteRepository, NoteWriteRepository>();
    }


   
}