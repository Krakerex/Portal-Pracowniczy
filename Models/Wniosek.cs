﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace krzysztofb.Models;

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
