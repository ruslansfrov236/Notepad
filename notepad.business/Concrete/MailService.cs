using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;
using notepad.business.Abstract;

namespace notepad.business.Concrete;

public class MailService : IMailService
{
    private readonly IConfiguration _configuration;

    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
    {
        await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
    }

    public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
    {
        MailMessage mail = new();
        mail.IsBodyHtml = isBodyHtml;


        foreach (var to in tos)
            mail.To.Add(to);


        mail.Subject = subject;
        mail.Body = body;


        mail.From = new MailAddress(_configuration["Mail:Username"], "Notepad", System.Text.Encoding.UTF8);


        using (SmtpClient smtp = new())
        {
            smtp.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Host = _configuration["Mail:Host"];


            await smtp.SendMailAsync(mail);
        }
    }


    public async Task SendVerificationCodeEmailAsync(string to, string code, string name)
    {
        string emailContent = $@"
        <html>
        <body>
            <p>Dear {name},</p>
            <p>Your login verification code is:</p>
            <h2>{code}</h2>
            <p>Please use this code within 60 minutes to complete your login.</p>
            <p>Best regards,<br>Your Team</p>
       
 
        </body>
        </html>";

        await SendMailAsync(to, "Login Verification Code", emailContent);
    }

    public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
    {
        var resetLink = $"{_configuration["AngularClientUrl"]}/update-password/{userId}/{resetToken}";

        // HTML email content
        var mailContent = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    line-height: 1.6;
                }}
                .email-container {{
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    border: 1px solid #e0e0e0;
                    border-radius: 8px;
                    background-color: #f9f9f9;
                }}
                .email-header {{
                    text-align: center;
                    font-size: 18px;
                    color: #333;
                    margin-bottom: 20px;
                }}
                .email-body {{
                    font-size: 14px;
                    color: #555;
                }}
                .email-link {{
                    display: inline-block;
                    padding: 10px 20px;
                    background-color: #007bff;
                    color: #fff;
                    text-decoration: none;
                    border-radius: 4px;
                    font-weight: bold;
                    margin-top: 20px;
                }}
                .email-footer {{
                    margin-top: 20px;
                    font-size: 12px;
                    color: #888;
                    text-align: center;
                }}
            </style>
        </head>
        <body>
            <div class='email-container'>
                <div class='email-header'>
                    Password Reset Request
                </div>
                <div class='email-body'>
                    Dear User,<br><br>
                    If you requested a password reset, please click the link below to reset your password:<br><br>
                    <a href='{resetLink}' target='_blank' class='email-link'>Reset My Password</a><br><br>
                    If you did not make this request, you can safely ignore this email.
                </div>
                <div class='email-footer'>
                    Best regards,<br>
                    Notepad MMC
                </div>
            </div>
        </body>
        </html>";

        await SendMailAsync(to, "Password Reset Request", mailContent);
    }
}