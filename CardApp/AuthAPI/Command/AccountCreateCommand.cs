using System;
using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Command
{
    public class AccountCreateCommand
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; } 
    }
}
