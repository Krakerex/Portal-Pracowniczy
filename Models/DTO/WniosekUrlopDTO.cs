namespace krzysztofb.Models.DTO
{
    public class WniosekUrlopDTO
    {
        public int Id { get; set; }

        public int IdWniosku { get; set; }

        public int? NrEwidencyjny { get; set; }

        public int? IloscDni { get; set; }

        public DateTime? PoczatekUrlopu { get; set; }

        public DateTime? KoniecUrlopu { get; set; }

        public DateTime? DataWypelnienia { get; set; }
    }
}
