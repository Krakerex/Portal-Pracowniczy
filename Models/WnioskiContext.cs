using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace krzysztofb.Models;

public partial class WnioskiContext : DbContext
{
    public WnioskiContext()
    {
    }

    public WnioskiContext(DbContextOptions<WnioskiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Role> Role { get; set; }

    public virtual DbSet<Uzytkownik> Uzytkownik { get; set; }

    public virtual DbSet<Wniosek> Wniosek { get; set; }

    public virtual DbSet<WniosekUrlop> WniosekUrlop { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=Wnioski");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC07F72E6973");
        });

        modelBuilder.Entity<Uzytkownik>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Uzytkown__3214EC07BE300B63");

            entity.HasOne(d => d.IdPrzelozonegoNavigation).WithMany(p => p.InverseIdPrzelozonegoNavigation).HasConstraintName("FK__Uzytkowni__Id_Pr__286302EC");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Uzytkownik).HasConstraintName("FK__Uzytkownik__Role__276EDEB3");
        });

        modelBuilder.Entity<Wniosek>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wniosek__3214EC07E28DE3C1");

            entity.HasOne(d => d.IdOsobyAkceptujacejNavigation).WithMany(p => p.WniosekIdOsobyAkceptujacejNavigation).HasConstraintName("FK__Wniosek__Id_Osob__2C3393D0");

            entity.HasOne(d => d.IdOsobyZglaszajacejNavigation).WithMany(p => p.WniosekIdOsobyZglaszajacejNavigation).HasConstraintName("FK__Wniosek__Id_Osob__2B3F6F97");
        });

        modelBuilder.Entity<WniosekUrlop>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.IdWnioskuNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WniosekUr__Id_wn__35BCFE0A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
