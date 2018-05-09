
using System;

namespace Bizfitech.Banking.Api.Core.Dtos
{
    public class UserReadDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public BankAccountReadDto BankAccount { get; set; }
    }
}
