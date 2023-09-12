namespace krzysztofb.Services.Interfaces
{
    public interface IPdfToDatabaseRead<T>
    {
        static abstract StringReader LoadPdf(IFormFile file);
        static abstract T GetPdfData(StringReader doc);
    }
}
