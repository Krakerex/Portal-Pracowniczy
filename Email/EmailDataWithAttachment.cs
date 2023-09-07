namespace krzysztofb.Email
{
    public class EmailDataWithAttachment : EmailData
    {
        public IFormFileCollection EmailAttachments { get; set; }
    }
}
