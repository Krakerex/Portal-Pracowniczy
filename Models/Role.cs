using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace krzysztofb.Models;

/// <summary>
/// Model roli
/// </summary>
public partial class Role
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Nazwa { get; set; } = null!;

    [InverseProperty("RoleNavigation")]
    public virtual ICollection<Uzytkownik> Uzytkownik { get; set; } = new List<Uzytkownik>();
}
