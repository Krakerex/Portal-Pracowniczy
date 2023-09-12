namespace krzysztofb.Services.Interfaces
{
    public interface IPdfToDatabase<T>
    {
        public T LoadPdf(IFormFile file);
        public T GetPdfData(StringReader doc);
    }
}
