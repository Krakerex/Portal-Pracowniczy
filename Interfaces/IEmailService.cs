using krzysztofb.Email;

namespace krzysztofb.Interfaces
{
    public interface IEmailService
    {
        bool SendEmail(EmailData emailData);
        bool SendEmailWithAttachment(EmailDataWithAttachment emailData);
    }
}

