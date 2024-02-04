using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using PixelCelebrateBackend.MailTrapConfig;

//using PixelCelebrateBackend.MailTrapConfig;

namespace PixelCelebrateBackend.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }


        //Send Sync: Cu data din SendEmail:
        public bool SendMail(MailData mailData)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    //De unde vine:
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);

                    //Id = email la cine se trimite / Name = nume la cine se trimite;
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    //Carbon copy:
                    //emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com"));
                    //emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

                    //Subiect:
                    emailMessage.Subject = mailData.EmailSubject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();

                    //Body:
                    emailBodyBuilder.TextBody = mailData.EmailBody;

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    //Email protocol: Smtp;
                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                //Verificare:
                Console.WriteLine(ex);
                return false;
            }
        }


        //Send Async:
        public async Task<bool> SendMailAsync(MailData mailData)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    //Analog:
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    //CCS / BCCS:
                    //emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com"));
                    //emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

                    //Analog:
                    emailMessage.Subject = mailData.EmailSubject;
                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.TextBody = mailData.EmailBody;
                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    //SMTP, no HTTP;
                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        await mailClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        await mailClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                        await mailClient.SendAsync(emailMessage);
                        await mailClient.DisconnectAsync(true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                //Verificare:
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
