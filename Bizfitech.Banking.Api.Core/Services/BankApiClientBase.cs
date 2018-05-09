using Bizfitech.Banking.Api.Core.Dtos;
using Bizfitech.Banking.Api.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bizfitech.Banking.Api.Core.Services
{
    public abstract class BankApiClientBase
    {
        protected readonly BankApiClientConfiguration _configuration;

        public BankApiClientBase(BankApiClientConfiguration configuration)
        {
            _configuration = configuration;
        }

        public abstract Task<BankAccountReadDto> GetAccountAsync(string accountNumber);
        public abstract IEnumerable<BankTransactionDto> GetTransactions(string accountNumber);
    }
}
