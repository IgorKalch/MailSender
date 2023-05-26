using System.Net;
using System.Net.Mail;

namespace RabbitServerLibrary.SendingMethods
{
    public class SmtpMethod : IMethod
    {
        public async Task Send(Mail mail, Settings settings)
        {
            await Task.Run(() =>
            {
                using (SmtpClient client = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = settings.SmtpMethodSettings.Login,
                        Password = settings.SmtpMethodSettings.Password
                    };

                    client.Credentials = credential;
                    client.Host = settings.SmtpMethodSettings.Host;
                    client.Port = settings.SmtpMethodSettings.Port;
                    client.EnableSsl = settings.SmtpMethodSettings.EnableSsl;

                    MailMessage mailMessage = new MailMessage();

                    mailMessage.From = new MailAddress(mail.From);
                    mailMessage.To.Add(mail.To);
                    mailMessage.Subject = mail.Subject;
                    mailMessage.Body = mail.Body;

                    client.Send(mailMessage);
                }
            });
        }
    }
}
