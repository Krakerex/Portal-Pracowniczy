using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace krzysztofb.Models;

[Index("Email", Name = "UQ__Uzytkown__A9D10534D16A30AB", IsUnique = true)]
public partial class Uzytkownik
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Imie { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string Nazwisko { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    public int? Role { get; set; }

    [Column("Id_Przelozonego")]
    public int? IdPrzelozonego { get; set; }

    [Unicode(false)]
    public string? PasswordHash { get; set; }

    [ForeignKey("IdPrzelozonego")]
    [InverseProperty("InverseIdPrzelozonegoNavigation")]
    public virtual Uzytkownik? IdPrzelozonegoNavigation { get; set; }

    [InverseProperty("IdPrzelozonegoNavigation")]
    public virtual ICollection<Uzytkownik> InverseIdPrzelozonegoNavigation { get; set; } = new List<Uzytkownik>();

    [ForeignKey("Role")]
    [InverseProperty("Uzytkownik")]
    public virtual Role? RoleNavigation { get; set; }

    [InverseProperty("IdOsobyAkceptujacejNavigation")]
    public virtual ICollection<Wniosek> WniosekIdOsobyAkceptujacejNavigation { get; set; } = new List<Wniosek>();

    [InverseProperty("IdOsobyZglaszajacejNavigation")]
    public virtual ICollection<Wniosek> WniosekIdOsobyZglaszajacejNavigation { get; set; } = new List<Wniosek>();
}
