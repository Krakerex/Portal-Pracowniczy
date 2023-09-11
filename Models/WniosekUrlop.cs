using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace krzysztofb.Models;

[Keyless]
public partial class WniosekUrlop
{
    public int Id { get; set; }

    [Column("Id_wniosku")]
    public int IdWniosku { get; set; }

    [Column("Nr_ewidencyjny")]
    public int? NrEwidencyjny { get; set; }

    [Column("Ilosc_dni")]
    public int? IloscDni { get; set; }

    [Column("Poczatek_urlopu", TypeName = "date")]
    public DateTime? PoczatekUrlopu { get; set; }

    [Column("Koniec_urlopu", TypeName = "date")]
    public DateTime? KoniecUrlopu { get; set; }

    [Column("Data_wypelnienia", TypeName = "date")]
    public DateTime? DataWypelnienia { get; set; }

    [ForeignKey("IdWniosku")]
    public virtual Wniosek IdWnioskuNavigation { get; set; } = null!;
}
