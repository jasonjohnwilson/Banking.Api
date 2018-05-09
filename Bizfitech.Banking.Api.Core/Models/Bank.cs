using System.Collections.Generic;

namespace Bizfitech.Banking.Api.Core.Models
{
    public class Bank : Entity
    {
        public Bank()
        {
            Accounts = new HashSet<Account>();
        }

        public string Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
