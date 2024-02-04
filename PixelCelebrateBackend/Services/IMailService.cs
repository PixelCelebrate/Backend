using PixelCelebrateBackend.MailTrapConfig;

namespace PixelCelebrateBackend.Services
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);

        Task<bool> SendMailAsync(MailData mailData);
    }
}
