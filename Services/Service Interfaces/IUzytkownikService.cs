using krzysztofb.Models.DTO;

namespace krzysztofb.Services
{
    public interface IUzytkownikService
    {
        UzytkownikDTO Create(UzytkownikDTO obj);

        UzytkownikDTO Delete(int id);

        List<UzytkownikDTO> Read();

        UzytkownikDTO Read(int id);

        UzytkownikDTO Update(int id, UzytkownikDTO obj);
    }
}