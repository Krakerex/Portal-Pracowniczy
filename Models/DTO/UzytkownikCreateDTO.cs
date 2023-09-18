﻿namespace krzysztofb.Models.DTO
{
    /// <summary>
    /// Model DTO użytkownika
    /// </summary>
    public record UzytkownikCreateDTO
    {
        public int? Id
        {
            get;
        }
        public string? Imie
        {
            get; init;
        }
        public string? Nazwisko
        {
            get; init;
        }
        public string? Email
        {
            get; init;
        }
        public int? Role
        {
            get; init;
        }
        public int? IdPrzelozonego
        {
            get; init;
        }
        public string Password
        {
            get; set;
        }
        public UzytkownikCreateDTO(int id, string imie, string nazwisko, string email, int? role, int? id_Przelozonego)
        {
            Id = id;
            Imie = imie;
            Nazwisko = nazwisko;
            Email = email;
            Role = role;
            IdPrzelozonego = id_Przelozonego;
        }
        public UzytkownikCreateDTO()
        {
        }
    }
}