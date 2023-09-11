using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace krzysztofb.Models;
/// <summary>
/// Model wniosku
/// </summary>
public partial class Wniosek
{
    [Key]
    public int Id { get; set; }

    public byte[]? Plik { get; set; }

    [Unicode(false)]
    public string? Nazwa { get; set; }

    [Column("Id_Osoby_Zglaszajacej")]
    public int? IdOsobyZglaszajacej { get; set; }

    [Column("Id_Osoby_Akceptujacej")]
    public int? IdOsobyAkceptujacej { get; set; }

    [ForeignKey("IdOsobyAkceptujacej")]
    [InverseProperty("WniosekIdOsobyAkceptujacejNavigation")]
    public virtual Uzytkownik? IdOsobyAkceptujacejNavigation { get; set; }

    [ForeignKey("IdOsobyZglaszajacej")]
    [InverseProperty("WniosekIdOsobyZglaszajacejNavigation")]
    public virtual Uzytkownik? IdOsobyZglaszajacejNavigation { get; set; }
}
