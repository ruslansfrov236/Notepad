using notepad.entity.Entities;

namespace notepad.business.Abstract;

public interface IMailService
{
    Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true);
    Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
    Task SendVerificationCodeEmailAsync(string to, string code, string name);

    Task NotificationEmailAsync(string[] tos, ICollection<Note> notes);
    Task SendPasswordResetMailAsync(string to, string userId, string resetToken);
}