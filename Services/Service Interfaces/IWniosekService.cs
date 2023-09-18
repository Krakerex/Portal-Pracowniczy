using krzysztofb.Models;
using krzysztofb.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace krzysztofb.Services
{
    public interface IWniosekService
    {
        WniosekDTO Accept(int idWniosek, int idKierownik);

        WniosekDTO AddFile(WniosekDTO wniosek, IFormFile file);

        Wniosek Create(IFormFile file);

        WniosekDTO Delete(int id);

        FileResult DownloadFile(int id);

        List<Wniosek> Read();

        IFormFile ReadFile(int id);
    }
}