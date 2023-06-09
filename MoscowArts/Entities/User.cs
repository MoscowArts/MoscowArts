﻿using System.ComponentModel.DataAnnotations;

namespace MoscowArts.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; } = "";
        [Required] public string Surname { get; set; } = "";
        public string? Patronymic { get; set; }
        [EmailAddress] public string Email { get; set; }
        public string? Phone { get; set; } 
        public int? Age { get; set; }
        public string? Photo { get; set; }
        [Required] public DateTime RegistrationDate { get; set; }

        public int Rating { get; set; }

        //[Required] public string Username { get; set; } = "";
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public string RefreshToken { get; set; } = String.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}
