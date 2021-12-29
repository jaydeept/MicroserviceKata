using System.ComponentModel.DataAnnotations;

namespace AccountService.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
