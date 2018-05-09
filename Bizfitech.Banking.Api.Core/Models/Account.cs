using System;

namespace Bizfitech.Banking.Api.Core.Models
{
    public class Account : Entity
    {
        public string AccountNumber { get; set; }
        public Guid UserId { get; set; }
        public Guid BankId { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual User User { get; set; }
    }
}
