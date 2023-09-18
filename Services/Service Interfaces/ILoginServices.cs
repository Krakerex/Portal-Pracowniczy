using krzysztofb.Models.DTO;

namespace krzysztofb.Services
{
    public interface ILoginServices
    {
        string Login(UzytkownikLoginDTO uzytkownik);
    }
}