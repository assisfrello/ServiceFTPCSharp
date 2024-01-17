using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Ftp.CrossCutting.Email
{
    public class EmailNotification
    {
        public static async Task Send(Exception exception, string message, string title, string arquivo)
        {
            try
            {
                var destinatarios = ConfigurationManager.AppSettings["DESTINATION"];
                var emails = destinatarios?.Split(';').ToList();
                
                var email = new Email
                {
                    Body = $"Erro ao realizar leitura no arquivo: <b>{arquivo}</b> <br /><br /> <b>Motivo:</b> {message} <br /><br /> <b>Exception:</b> {exception}",
                    Priority = MailPriority.High,
                    Subject = "Notificação de erro de leitura de arquivo de FTP - " + title,
                    ToAddress = emails
                };
                
                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(ConfigurationManager.AppSettings["USERNAME"] ?? string.Empty, ConfigurationManager.AppSettings["DISPLAYNAME"]),
                    Priority = email.Priority,
                    IsBodyHtml = true,
                    Subject = email.Subject
                };

                if (email.Attachment != null)
                    mailMessage.Attachments.Add(email.Attachment);

                if (!string.IsNullOrEmpty(email.Body))
                    mailMessage.Body = email.Body;

                if (email.AlternateView != null)
                    mailMessage.AlternateViews.Add(email.AlternateView);

                if (email.ToAddress != null)
                    foreach (var toAddress in email.ToAddress)
                        if (!string.IsNullOrEmpty(toAddress))
                            mailMessage.To.Add(toAddress);

                if (email.ToHiddenAddress != null && email.ToHiddenAddress.Any())
                    foreach (var toHiddemAddress in email.ToHiddenAddress)
                        if (!string.IsNullOrEmpty(toHiddemAddress))
                            mailMessage.Bcc.Add(toHiddemAddress);

                using var smtpClient = new SmtpClient(ConfigurationManager.AppSettings["SMTP"])
                {
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["PORT"]),
                    EnableSsl = true,
                    Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["TIMEOUT"]),
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["USERNAME"], ConfigurationManager.AppSettings["PASSWORD"])
                };

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}