
namespace Bizfitech.Banking.Api.Core.Dtos
{
    public class UserCreateDto
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public BankAccountCreateDto BankAccount { get; set; }
    }
}
