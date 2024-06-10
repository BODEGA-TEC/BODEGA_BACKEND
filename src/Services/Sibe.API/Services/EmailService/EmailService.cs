using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Sibe.API.Utils;

namespace Sibe.API.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.office365.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser = "s.jeykime@outlook.com";
        private readonly string _smtpPass = "hsrndydbaormnsbh"; // Reemplaza con tu contraseña
        private readonly string _tecLogoPath = "wwwroot/images/teclogo.png";
        private readonly string _templatePath = "wwwroot/html/TemporaryCodeTemplate.html"; // Ruta a la plantilla


        private string GenerateTemporaryCodeBodyFromTemplate(string toEmail, string code)
        {
            // Cargar y reemplazar los marcadores de la plantilla
            string template = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), _templatePath));
            string obfuscatedEmail = EmailHelper.ObfuscateEmail(toEmail);
            template = template.Replace("{{email}}", obfuscatedEmail);
            template = template.Replace("{{code}}", code);
            return template;
        }

        public async Task SendTemporaryCodeEmailAsync(string toEmail, string code)
        {
            string body = GenerateTemporaryCodeBodyFromTemplate(toEmail, code);
            await SendEmailAsync(toEmail, "Generación de Nueva Contraseña", body);
        }


        public async Task SendEmailAsync(string toEmail, string subject, string body, string? attachmentPath = null)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("SIBE", _smtpUser));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            var image = bodyBuilder.LinkedResources.Add(Path.Combine(Directory.GetCurrentDirectory(), _tecLogoPath));
            image.ContentId = "logo";

            bodyBuilder.HtmlBody = body;

            if (!string.IsNullOrEmpty(attachmentPath) && File.Exists(attachmentPath))
            {
                bodyBuilder.Attachments.Add(attachmentPath);
            }

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_smtpUser, _smtpPass);
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }

        //public string GenerateEmailBodyWithPdf(string userName)
        //{
        //    return $"<h1>Hello {userName},</h1><p>Please find the attached document.</p>";
        //}
    }

}
