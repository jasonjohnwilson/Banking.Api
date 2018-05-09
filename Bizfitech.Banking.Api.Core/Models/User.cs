using System.Collections.Generic;

namespace Bizfitech.Banking.Api.Core.Models
{
    public class User : Entity
    {
        public User()
        {
            Accounts = new HashSet<Account>();
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
