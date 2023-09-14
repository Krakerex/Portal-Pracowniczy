namespace krzysztofb.Email
{
    /// <summary>
    /// Model zawierający dane do wysłania wiadomości email rozszerzony o możliwość dodania załącznika
    /// </summary>
    public class EmailDataWithAttachment : EmailData
    {
        public IFormFileCollection EmailAttachments { get; set; }

    }
}
