using Bizfitech.Banking.Api.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Bizfitech.Banking.Api.Core.Services
{
    public class BankApiClientProvider : IBankApiClientProvider
    {
        private readonly IDictionary<string, BankApiClientBase> _bankApiClients;

        public BankApiClientProvider(IDictionary<string, BankApiClientBase> bankApiClients)
        {
            _bankApiClients = bankApiClients;
        }

        public BankApiClientBase Get(string bankName)
        {
            if (string.IsNullOrEmpty(bankName))
            {
                throw new ArgumentNullException($"{nameof(bankName)} cannot be null or empty");
            }

            if (_bankApiClients.TryGetValue(bankName, out BankApiClientBase client) == false)
            {
                throw new ArgumentException($"No bank API client configured for bank {bankName}");
            }

            return client;
        }
    }
}
