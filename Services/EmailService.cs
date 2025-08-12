using MailKit.Net.Smtp;
using MimeKit;

namespace Automotive_Project.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("Automotive@gmail.com"));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = htmlMessage };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.yourmailserver.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("nikicha1pk@gmail.com", "xycmsudmuilfzoqq");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
