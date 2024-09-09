using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System;

namespace CitasMedicas.Middleware
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Citas Médicas", _configuration["Smtp:Username"]));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = body };

            using (var client = new SmtpClient())
            {
                client.CheckCertificateRevocation = false;
                await client.ConnectAsync(_configuration["Smtp:Server"], int.Parse(_configuration["Smtp:Port"]), MailKit.Security.SecureSocketOptions.StartTls);

                // Note: only needed if the SMTP server requires authentication
                await client.AuthenticateAsync(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);

                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        public string GenerateRandomPassword(int length)
        {
            Random random = new Random();
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string allChars = upper + lower + digits;

            // Ensure at least one of each required character
            var passwordChars = new char[length];
            passwordChars[0] = upper[random.Next(upper.Length)];
            passwordChars[1] = lower[random.Next(lower.Length)];
            passwordChars[2] = digits[random.Next(digits.Length)];

            // Fill the rest of the password with random characters
            for (int i = 3; i < length; i++)
            {
                passwordChars[i] = allChars[random.Next(allChars.Length)];
            }

            // Shuffle the characters to ensure randomness
            return new string(passwordChars.OrderBy(c => random.Next()).ToArray());
        }

    }
}
