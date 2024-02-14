using System.ComponentModel.DataAnnotations;

namespace Chatter.WebApp.Models.Account
{
    public class RegisterDto
    {
        [Required]
        public string FullName { get; set; }
        
        [Required]
        public string UserName { get; set; }    

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string RePassword { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}