using Bizfitech.Banking.Api.Core.Interfaces;
using Bizfitech.Banking.Api.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bizfitech.Banking.Api.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IBankApiClientProvider _clientProvider;

        public AccountService(
            IBankApiClientProvider clientProvider)
        {
            _clientProvider = clientProvider;
        }

        public IEnumerable<Account> GetUserAccounts(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsAccountValidAsync(string bankName, string accountNumber)
        {
            if(string.IsNullOrEmpty(bankName))
            {
                throw new ArgumentNullException($"{nameof(bankName)} cannot be null or empty");
            }

            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentNullException($"{nameof(accountNumber)} cannot be null or empty");
            }

            var client = _clientProvider.Get(bankName);

            return await client.GetAccountAsync(accountNumber) != null;
        }
    }
}
