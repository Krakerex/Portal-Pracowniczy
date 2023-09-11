namespace krzysztofb.Models.DTO
{
    /// <summary>
    /// Model DTO wniosku
    /// </summary>
    public class WniosekDTO
    {
        public int Id { get; set; }
        public byte[]? Plik { get; set; }
        public string Nazwa { get; set; }
        public int? IdOsobyZglaszajacej { get; set; }
        public int? IdOsobyAkceptujacej { get; set; }
        public WniosekDTO(int id, byte[]? plik, string nazwa, int id_Osoby_Zglaszajacej, int id_Osoby_Akceptujacej)
        {
            Id = id;
            Plik = plik;
            Nazwa = nazwa;
            IdOsobyZglaszajacej = id_Osoby_Zglaszajacej;
            IdOsobyAkceptujacej = id_Osoby_Akceptujacej;
        }
        public WniosekDTO()
        {

        }
    }
}
