using Microsoft.AspNetCore.Mvc;

namespace krzysztofb.Interfaces
{
    public interface IDatabaseDownloadFile<T>
    {
        public FileResult DownloadFile(int id);
    }
}
