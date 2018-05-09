using Bizfitech.Banking.Api.Core.Models;

namespace Bizfitech.Banking.Api.Core.Interfaces
{
    public interface IBankService
    {
        bool IsAccountValid(string bankName, string accountNumber);
    }
}
