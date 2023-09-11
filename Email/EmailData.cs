namespace krzysztofb.Email
{
    /// <summary>
    /// Model zawierający dane do wysłania wiadomości email
    /// </summary>
    public class EmailData
    {
        public string EmailToId { get; set; }
        public string EmailToName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }
}
