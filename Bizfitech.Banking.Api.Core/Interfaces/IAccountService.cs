using Bizfitech.Banking.Api.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bizfitech.Banking.Api.Core.Interfaces
{
    public interface IAccountService
    {
        Task<bool> IsAccountValidAsync(string bankName, string accountNumber);

        IEnumerable<Account> GetUserAccounts(Guid userId);
    }
}
