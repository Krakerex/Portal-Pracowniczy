namespace krzysztofb.CustomExceptions
{
    public class UploadException : Exception
    {
        public UploadException()
        {
        }

        public UploadException(string message)
            : base(message)
        {

        }

        public UploadException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
