using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bizfitech.Banking.Api.Web.Models
{
    public class UsersPageGetDto
    {
        public int TotalRows { get; set; }

        public int CurrentPage { get; set; }

        public int RowsPerPage { get; set; }

        public IEnumerable<UserGetDto> Users { get; set; }
    }
}
