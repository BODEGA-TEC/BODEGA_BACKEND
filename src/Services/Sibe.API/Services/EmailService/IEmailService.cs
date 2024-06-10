
namespace Sibe.API.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, string? attachmentPath = null, byte[]? attachment = null, string? attachmentName = null);
        Task SendTemporaryCodeEmailAsync(string toEmail, string code);
    }
}