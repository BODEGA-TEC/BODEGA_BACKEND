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



        /// <summary>
        /// Envía un correo electrónico.
        /// </summary>
        /// <param name="toEmail">La dirección de correo electrónico del destinatario.</param>
        /// <param name="subject">El asunto del correo electrónico.</param>
        /// <param name="body">El cuerpo en HTML del correo electrónico.</param>
        /// <param name="attachmentPath">La ruta del archivo adjunto. (opcional)</param>
        /// <param name="attachment">La matriz de bytes que representa el archivo adjunto. (opcional)</param>
        /// <param name="attachmentName">El nombre del archivo adjunto. (opcional)</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task SendEmailAsync(string toEmail, string subject, string body, string? attachmentPath = null, byte[]? attachment = null, string? attachmentName = null)
        {
            var message = CreateMessage(toEmail, subject, body, attachmentPath, attachment, attachmentName);

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

        private MimeMessage CreateMessage(string toEmail, string subject, string body, string? attachmentPath, byte[]? attachment, string? attachmentName)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("SIBE", _smtpUser));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            var image = bodyBuilder.LinkedResources.Add(Path.Combine(Directory.GetCurrentDirectory(), _tecLogoPath));
            image.ContentId = "logo";
            bodyBuilder.HtmlBody = body;

            // Adjuntar archivos del path si existen
            if (!string.IsNullOrEmpty(attachmentPath) && File.Exists(attachmentPath))
            {
                bodyBuilder.Attachments.Add(attachmentPath);
            }

            // Adjuntar bytes del archivo con el nombre recibido
            if (attachment != null && !string.IsNullOrEmpty(attachmentName))
            {
                using (var attachmentStream = new MemoryStream(attachment))
                {
                    bodyBuilder.Attachments.Add(attachmentName, attachmentStream);
                }
            }

            message.Body = bodyBuilder.ToMessageBody();
            return message;
        }



        ///// <summary>
        ///// Envía un correo electrónico sin archivo adjunto.
        ///// </summary>
        ///// <param name="toEmail">La dirección de correo electrónico del destinatario.</param>
        ///// <param name="subject">El asunto del correo electrónico.</param>
        ///// <param name="body">El cuerpo en HTML del correo electrónico.</param>
        ///// <returns>Una tarea que representa la operación asíncrona.</returns>
        //public async Task SendEmailAsync(string toEmail, string subject, string body)
        //{
        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("SIBE", _smtpUser));
        //    message.To.Add(new MailboxAddress("", toEmail));
        //    message.Subject = subject;

        //    var bodyBuilder = new BodyBuilder();
        //    var image = bodyBuilder.LinkedResources.Add(Path.Combine(Directory.GetCurrentDirectory(), _tecLogoPath));
        //    image.ContentId = "logo";
        //    bodyBuilder.HtmlBody = body;
        //    message.Body = bodyBuilder.ToMessageBody();

        //    using (var client = new SmtpClient())
        //    {
        //        try
        //        {
        //            await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
        //            await client.AuthenticateAsync(_smtpUser, _smtpPass);
        //            await client.SendAsync(message);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Error sending email: {ex.Message}");
        //            throw;
        //        }
        //        finally
        //        {
        //            await client.DisconnectAsync(true);
        //        }
        //    }
        //}


        ///// <summary>
        ///// Envía un correo electrónico con un archivo adjunto desde una ruta de archivo.
        ///// </summary>
        ///// <param name="toEmail">La dirección de correo electrónico del destinatario.</param>
        ///// <param name="subject">El asunto del correo electrónico.</param>
        ///// <param name="body">El cuerpo en HTML del correo electrónico.</param>
        ///// <param name="attachmentPath">La ruta del archivo adjunto. (opcional)</param>
        ///// <returns>Una tarea que representa la operación asíncrona.</returns>
        //public async Task SendEmailAsync(string toEmail, string subject, string body, string attachmentPath)
        //{
        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("SIBE", _smtpUser));
        //    message.To.Add(new MailboxAddress("", toEmail));
        //    message.Subject = subject;

        //    var bodyBuilder = new BodyBuilder();
        //    var image = bodyBuilder.LinkedResources.Add(Path.Combine(Directory.GetCurrentDirectory(), _tecLogoPath));
        //    image.ContentId = "logo";
        //    bodyBuilder.HtmlBody = body;

        //    // Adjuntar archivos del path si existen
        //    if (!string.IsNullOrEmpty(attachmentPath) && File.Exists(attachmentPath))
        //    {
        //        bodyBuilder.Attachments.Add(attachmentPath);
        //    }
        //    message.Body = bodyBuilder.ToMessageBody();

        //    using (var client = new SmtpClient())
        //    {
        //        try
        //        {
        //            await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
        //            await client.AuthenticateAsync(_smtpUser, _smtpPass);
        //            await client.SendAsync(message);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Error sending email: {ex.Message}");
        //            throw;
        //        }
        //        finally
        //        {
        //            await client.DisconnectAsync(true);
        //        }
        //    }
        //}


        ///// <summary>
        ///// Envía un correo electrónico con un archivo adjunto en forma de bytes.
        ///// </summary>
        ///// <param name="toEmail">La dirección de correo electrónico del destinatario.</param>
        ///// <param name="subject">El asunto del correo electrónico.</param>
        ///// <param name="body">El cuerpo en HTML del correo electrónico.</param>
        ///// <param name="attachment">La matriz de bytes que representa el archivo adjunto. (opcional)</param>
        ///// <param name="attachmentName">El nombre del archivo adjunto. (opcional)</param>
        ///// <returns>Una tarea que representa la operación asíncrona.</returns>
        //public async Task SendEmailAsync(string toEmail, string subject, string body, byte[] attachment, string attachmentName)
        //{
        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("SIBE", _smtpUser));
        //    message.To.Add(new MailboxAddress("", toEmail));
        //    message.Subject = subject;

        //    var bodyBuilder = new BodyBuilder();
        //    var image = bodyBuilder.LinkedResources.Add(Path.Combine(Directory.GetCurrentDirectory(), _tecLogoPath));
        //    image.ContentId = "logo";
        //    bodyBuilder.HtmlBody = body;

        //    // Adjuntar bytes del archivo con el nombre recibido
        //    if (attachment != null)
        //    {
        //        var attachmentStream = new MemoryStream(attachment);
        //        bodyBuilder.Attachments.Add(attachmentName, attachmentStream);
        //    }
        //    message.Body = bodyBuilder.ToMessageBody();

        //    using (var client = new SmtpClient())
        //    {
        //        try
        //        {
        //            await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
        //            await client.AuthenticateAsync(_smtpUser, _smtpPass);
        //            await client.SendAsync(message);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Error sending email: {ex.Message}");
        //            throw;
        //        }
        //        finally
        //        {
        //            await client.DisconnectAsync(true);
        //        }
        //    }
        //}

    }

}
