namespace krzysztofb.CustomExceptions
{
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
