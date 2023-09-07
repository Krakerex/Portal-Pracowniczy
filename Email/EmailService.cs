using krzysztofb.Configuration;
using krzysztofb.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace krzysztofb.Email
{
    public class EmailService : IEmailService
    {
        EmailSettings _emailSettings = null;
        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }
        public bool SendEmail(EmailData emailData)
        {
            try
            {
                MimeMessage emailMessage = new MimeMessage();
                MailboxAddress emailFrom = new MailboxAddress(_emailSettings.Name, _emailSettings.EmailId);
                emailMessage.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(emailData.EmailToName, emailData.EmailToId);
                emailMessage.To.Add(emailTo);
                emailMessage.Subject = emailData.EmailSubject;
                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.TextBody = emailData.EmailBody;
                emailMessage.Body = emailBodyBuilder.ToMessageBody();
                SmtpClient emailClient = new SmtpClient();
                emailClient.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                emailClient.Authenticate(_emailSettings.EmailId, _emailSettings.Password);
                emailClient.Send(emailMessage);
                emailClient.Disconnect(true);
                emailClient.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SendEmailWithAttachment(EmailDataWithAttachment emailData)
        {
            try
            {
                MimeMessage emailMessage = new MimeMessage();
                MailboxAddress emailFrom = new MailboxAddress(_emailSettings.Name, _emailSettings.EmailId);
                emailMessage.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(emailData.EmailToName, emailData.EmailToId);
                emailMessage.To.Add(emailTo);
                emailMessage.Subject = emailData.EmailSubject;
                BodyBuilder emailBodyBuilder = new BodyBuilder();
                if (emailData.EmailAttachments != null)
                {
                    byte[] attachmentFileByteArray;
                    foreach (IFormFile attachmentFile in emailData.EmailAttachments)
                    {
                        if (attachmentFile.Length > 0)
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                attachmentFile.CopyTo(memoryStream);
                                attachmentFileByteArray = memoryStream.ToArray();
                            }
                            emailBodyBuilder.Attachments.Add(attachmentFile.FileName, attachmentFileByteArray, ContentType.Parse(attachmentFile.ContentType));
                        }
                    }
                }
                emailBodyBuilder.TextBody = emailData.EmailBody;
                emailMessage.Body = emailBodyBuilder.ToMessageBody();
                SmtpClient emailClient = new SmtpClient();
                emailClient.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                emailClient.Authenticate(_emailSettings.EmailId, _emailSettings.Password);
                emailClient.Send(emailMessage);
                emailClient.Disconnect(true);
                emailClient.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
