using System.Net;
using System.Net.Mail;

namespace RESTApi.Utility.EmailService
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string to, string subject, string body)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Credentials = new NetworkCredential("harshvardhansingh458@gmail.com", "#Harsh1999"),
                EnableSsl = true
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("harshvardhansingh458@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);
            smtpClient.Send(mailMessage);
        }
    }
}