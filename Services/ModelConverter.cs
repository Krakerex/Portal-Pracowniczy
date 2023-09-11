using krzysztofb.Interfaces;
using krzysztofb.Models;
using krzysztofb.Models.DTO;

namespace krzysztofb.Services
{
    /// <summary>
    /// Konwerter modelu na DTO i odwrotnie
    /// </summary>
    public class ModelConverter : IModelConverter<Wniosek, WniosekDTO>, IModelConverter<Uzytkownik, UzytkownikDTO>
    {
        public static WniosekDTO ConvertToDTO(Wniosek wniosek)
        {
            return new WniosekDTO
            {
                Id = wniosek.Id,
                Plik = wniosek.Plik,
                Nazwa = wniosek.Nazwa,
                IdOsobyZglaszajacej = wniosek.IdOsobyZglaszajacej,
                IdOsobyAkceptujacej = wniosek.IdOsobyAkceptujacej
            };
        }

        public static UzytkownikDTO ConvertToDTO(Uzytkownik uzytkownik)
        {
            return new UzytkownikDTO
                (
                uzytkownik.Id,
                uzytkownik.Imie,
                uzytkownik.Nazwisko,
                uzytkownik.Email,
                uzytkownik.Role,
                uzytkownik.IdPrzelozonego
                );
        }

        public static Wniosek ConvertToModel(WniosekDTO wniosek)
        {
            return new Wniosek
            {
                Id = wniosek.Id,
                Plik = wniosek.Plik,
                Nazwa = wniosek.Nazwa,
                IdOsobyZglaszajacej = wniosek.IdOsobyZglaszajacej,
                IdOsobyAkceptujacej = wniosek.IdOsobyAkceptujacej
            };
        }

        public static Uzytkownik ConvertToModel(UzytkownikDTO uzytkownik)
        {
            return new Uzytkownik
            {
                Id = uzytkownik.Id,
                Imie = uzytkownik.Imie,
                Nazwisko = uzytkownik.Nazwisko,
                Email = uzytkownik.Email,
                Role = uzytkownik.Role,
                IdPrzelozonego = uzytkownik.IdPrzelozonego
            };
        }
    }
}
