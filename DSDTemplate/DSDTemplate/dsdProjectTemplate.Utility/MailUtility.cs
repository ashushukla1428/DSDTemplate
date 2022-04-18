using System.Configuration;
using System.Net.Mail;

namespace dsdProjectTemplate.Utility
{
    public static class MailUtility
    {
        public static void SendEmail(string to, string body, string subject)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true; 
            SmtpClient smtp = new SmtpClient();
            //smtp.UseDefaultCredentials = false;
           // smtp.EnableSsl = true;
            smtp.Send(mail);
        }
        public static void SendEmailToMultipeRecipientsBCC(string[] to, string body, string subject)
        {
            MailMessage mail = new MailMessage();

            mail.To.Add(ConfigurationManager.AppSettings["NotificationEmails"]);

            foreach (var item in to)
            {
                mail.Bcc.Add(item);
            }

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Send(mail);
        }
        public static void SendEmailToMultipeRecipients(string[] to, string body, string subject)
        {
            MailMessage mail = new MailMessage();
            foreach (var item in to)
            {
                mail.To.Add(item);
            }

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Send(mail);
        }
    }
}
