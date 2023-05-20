using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MoscowArts.Entities
{
    public class UserDTO
    {
        [EmailAddress] public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        [Required] public string Name { get; set; } = "";
        [Required] public string Surname { get; set; } = "";
        public string? Patronymic { get; set; }
        public string? Phone { get; set; }
        public int? Age { get; set; }
        public string? Photo { get; set; }
    }
}
