using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace krzysztofb.Models;

public partial class Role
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Nazwa { get; set; } = null!;

    [InverseProperty("RoleNavigation")]
    public virtual ICollection<Uzytkownik> Uzytkownik { get; } = new List<Uzytkownik>();
}
