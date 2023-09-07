using Microsoft.AspNetCore.Mvc;

namespace krzysztofb.Interfaces
{
    public interface IDatabaseFileRead
    {
        public IFormFile ReadFile(int id);
        public FileResult DownloadFile(int id);
    }
}
