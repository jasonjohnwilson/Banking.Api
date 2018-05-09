using System;
using System.Collections.Generic;
using System.Text;

namespace Bizfitech.Banking.Api.Core.Models
{
    public class BankApiClientConfiguration : Entity
    {
        public string TypeName { get; set; }
        public string AssemblyFullName { get; set; }
        public string ApiHost { get; set; }
        public Guid BankId { get; set; }

        public virtual Bank Bank { get; set; }
    }
}
