namespace krzysztofb.CustomExceptions
{
    public class PdfToDatabaseException : Exception
    {
        public PdfToDatabaseException()
        {
        }

        public PdfToDatabaseException(string message)
            : base(message)
        {

        }

        public PdfToDatabaseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
