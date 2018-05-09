using Bizfitech.Banking.Api.Core.Services;

namespace Bizfitech.Banking.Api.Core.Interfaces
{
    public interface IBankApiClientProvider
    {
        BankApiClientBase Get(string bankName);
    }
}