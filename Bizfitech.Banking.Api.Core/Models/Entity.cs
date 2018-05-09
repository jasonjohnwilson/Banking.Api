using System;
using System.Collections.Generic;
using System.Text;

namespace Bizfitech.Banking.Api.Core.Models
{
    public class Entity
    {
        public Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}
