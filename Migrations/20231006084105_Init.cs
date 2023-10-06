using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal_Pracownika_API.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__3214EC07F72E6973", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Uzytkownik",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Nazwisko = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: true),
                    Id_Przelozonego = table.Column<int>(type: "int", nullable: true),
                    PasswordHash = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Uzytkown__3214EC07BE300B63", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Uzytkowni__Id_Pr__286302EC",
                        column: x => x.Id_Przelozonego,
                        principalTable: "Uzytkownik",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Uzytkownik__Role__276EDEB3",
                        column: x => x.Role,
                        principalTable: "Role",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Wniosek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Osoby_Zglaszajacej = table.Column<int>(type: "int", nullable: true),
                    Nazwa = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Numer_ewidencyjny = table.Column<int>(type: "int", nullable: true),
                    Plik = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Data_rozpoczecia = table.Column<DateTime>(type: "date", nullable: true),
                    Data_zakonczenia = table.Column<DateTime>(type: "date", nullable: true),
                    Ilosc_dni = table.Column<int>(type: "int", nullable: true),
                    Data_wypelnienia = table.Column<DateTime>(type: "date", nullable: true),
                    Id_Osoby_Akceptujacej = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Wniosek__3214EC07E28DE3C1", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Wniosek__Id_Osob__2B3F6F97",
                        column: x => x.Id_Osoby_Zglaszajacej,
                        principalTable: "Uzytkownik",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Wniosek__Id_Osob__2C3393D0",
                        column: x => x.Id_Osoby_Akceptujacej,
                        principalTable: "Uzytkownik",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Uzytkownik_Id_Przelozonego",
                table: "Uzytkownik",
                column: "Id_Przelozonego");

            migrationBuilder.CreateIndex(
                name: "IX_Uzytkownik_Role",
                table: "Uzytkownik",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "UQ__Uzytkown__A9D10534D16A30AB",
                table: "Uzytkownik",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wniosek_Id_Osoby_Akceptujacej",
                table: "Wniosek",
                column: "Id_Osoby_Akceptujacej");

            migrationBuilder.CreateIndex(
                name: "IX_Wniosek_Id_Osoby_Zglaszajacej",
                table: "Wniosek",
                column: "Id_Osoby_Zglaszajacej");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wniosek");

            migrationBuilder.DropTable(
                name: "Uzytkownik");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
