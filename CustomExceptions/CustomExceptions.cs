namespace krzysztofb.CustomExceptions
{
    public class CustomExceptions
    {
    }
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
    public class DatabaseValidationException : Exception
    {
        public DatabaseValidationException()
        {
        }

        public DatabaseValidationException(string message)
            : base(message)
        {

        }

        public DatabaseValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
