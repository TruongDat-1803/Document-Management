using Microsoft.Extensions.Configuration;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Upload.Services
{
    public interface IEmailService
    {
        Task SendEmail(string _email);
    }

    public class EmailService : IEmailService
    {

        public EmailService()
        {
        }
        public async Task SendEmail(string _email)
        {
            var email = "td18032003@gmail.com";
            var password = "Http0604";
            var host = "smtp.gmail.com";
            var port = 587;

            var smtpClient = new SmtpClient(host, port);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;

            smtpClient.Credentials = new System.Net.NetworkCredential(email, password);

            // Content
            var emailSubject = "Confirm your email address";
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);
            string randomString = randomNumber.ToString();

            var message = new MailMessage(email!, _email, emailSubject, randomString);
            await smtpClient.SendMailAsync(message);
        }
    }
}
