namespace krzysztofb.Models.DTO
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
                IdOsobyAkceptujacej = wniosek.IdOsobyAkceptujacej,
                Data_rozpoczecia = DateOnly.FromDateTime(wniosek.DataRozpoczecia.Value),
                Data_zakonczenia = DateOnly.FromDateTime(wniosek.DataZakonczenia.Value),
                Data_wypelnienia = DateOnly.FromDateTime(wniosek.DataWypelnienia.Value),
                Nr_ewidencyjny = wniosek.NumerEwidencyjny,
                Ilosc_dni = wniosek.IloscDni
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
                IdOsobyAkceptujacej = wniosek.IdOsobyAkceptujacej,
                //convert DateOnly to DateTime
                DataRozpoczecia = wniosek.Data_rozpoczecia.Value.ToDateTime(TimeOnly.MinValue),
                DataWypelnienia = wniosek.Data_wypelnienia.Value.ToDateTime(TimeOnly.MinValue),
                DataZakonczenia = wniosek.Data_zakonczenia.Value.ToDateTime(TimeOnly.MinValue),
                NumerEwidencyjny = wniosek.Nr_ewidencyjny,
                IloscDni = wniosek.Ilosc_dni
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
