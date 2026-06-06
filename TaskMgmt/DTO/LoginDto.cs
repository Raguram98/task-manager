using System.ComponentModel.DataAnnotations;

namespace TaskMgmt.DTO
{
    public class LoginDto
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(7)]
        public string Password { get; set; } = string.Empty;
    }
}
