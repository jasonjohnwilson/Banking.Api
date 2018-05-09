using System;

namespace Bizfitech.Banking.Api.Web.Models
{
    public class UserGetDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }        
    }
}
