using System.ComponentModel.DataAnnotations;

namespace Bizfitech.Banking.Api.Web.Models
{
    public class UserPostDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Email { get; set; }
        //todo: this needs to handle multiple accounts
        [Required]
        public BankAccountDto BankAccount { get; set; }
    }
}
