
namespace Sibe.API.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, string? attachmentPath = null);
        Task SendTemporaryCodeEmailAsync(string toEmail, string code);
    }
}