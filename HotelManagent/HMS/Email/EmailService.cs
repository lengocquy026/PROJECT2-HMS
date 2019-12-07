using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HMS.Email
{
    public class EmailService : EmailConfig
    {
        public EmailService()
        {
        }
        public static bool Send(SendEmailRequest request)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(EmailConfig.senderEmail, EmailConfig.SenderPassword),
                    EnableSsl = true
                };
                MailMessage mail = new MailMessage(new MailAddress(EmailConfig.senderEmail, EmailConfig.NameEmail, System.Text.Encoding.UTF8),
                    new MailAddress(string.IsNullOrWhiteSpace(request.ToEmail) ? EmailConfig.senderEmail : request.ToEmail))
                {
                    Subject = request.Subject,
                    Body = request.Body,
                    IsBodyHtml = true,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
                    SubjectEncoding = System.Text.Encoding.UTF8
                };
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
