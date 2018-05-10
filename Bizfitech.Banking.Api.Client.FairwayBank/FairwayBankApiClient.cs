using Bizfitech.Banking.Api.Core.Dtos;
using Bizfitech.Banking.Api.Core.Models;
using Bizfitech.Banking.Api.Core.Services;
using IO.Swagger.Api;
using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bizfitech.Banking.Api.Client.FairwayBank
{
    public class FairwayBankApiClient : BankApiClientBase
    {
        public FairwayBankApiClient(BankApiClientConfiguration configuration) : base(configuration)
        {
        }

        public override async Task<BankAccountReadDto> GetAccountAsync(string accountNumber)
        {
            var accountViewModel = await GetAccountDetailsAsync(accountNumber);

            if (accountViewModel == null || accountViewModel.Identifier == null)
            {
                return null;
            }

            var balanceViewModel = await GetBalanceDetailsAsync(accountViewModel.Identifier.AccountNumber);

            //todo:  check calculation for balance and extract into strategy class and inject, so it is testable
            var accountDto = new BankAccountReadDto
            {
                AccountName = accountViewModel.Name,
                AccountNumber = accountViewModel.Identifier.AccountNumber,
                Bank = _configuration.Bank.Name,
                SortCode = accountViewModel.Identifier.SortCode,
                AvailableBalance = CalculateAvailableBalance(balanceViewModel),
                Balance = CalculateBalance(balanceViewModel),
                Overdraft = balanceViewModel.Overdraft != null ? balanceViewModel.Overdraft.Amount.Value : 0
            };

            return accountDto;
        }

        private static double CalculateBalance(BalanceViewModel balanceViewModel)
        {
            if (balanceViewModel.Type == BalanceViewModel.TypeEnum.Debit)
            {
                return 0 - balanceViewModel.Amount.Value;
            }
            else
            {
                return balanceViewModel.Amount.Value;
            }                
        }

        private static double CalculateAvailableBalance(BalanceViewModel balanceViewModel)
        {
            double availableBalance = 0;
            double overdraftAmount = GetOverdraftAmount(balanceViewModel.Overdraft);

            if (balanceViewModel.Type == BalanceViewModel.TypeEnum.Debit)
            {
                availableBalance = overdraftAmount - balanceViewModel.Amount.Value;
            }
            else
            {
                availableBalance = overdraftAmount + balanceViewModel.Amount.Value;
            }

            return availableBalance;
        }

        private static double GetOverdraftAmount(OverdraftViewModel overdraftViewModel)
        {
            return overdraftViewModel != null ? overdraftViewModel.Amount.Value : 0;
        }

        private async Task<AccountViewModel> GetAccountDetailsAsync(string accountNumber)
        {
            var accountApi = new AccountsApi(_configuration.ApiHost);
            var accountViewModel = await accountApi.ApiV1AccountsByAccountNumberGetAsyncWithHttpInfo(accountNumber);

            return accountViewModel.Data;
        }

        private async Task<BalanceViewModel> GetBalanceDetailsAsync(string accountNumber)
        {
            var balanceApi = new BalancesApi(_configuration.ApiHost);
            var balanceViewModel = await balanceApi.ApiV1AccountsByAccountNumberBalanceGetAsyncWithHttpInfo(accountNumber);

            return balanceViewModel.Data;
        }

        public override IEnumerable<BankTransactionDto> GetTransactions(string accountNumber)
        {
            throw new NotImplementedException();
        }
    }
}
