using Bizfitech.Banking.Api.Core.Models;

namespace Bizfitech.Banking.Api.Core.Interfaces
{
    public interface IBankUow : IUowBase
    {
        IGenericRepository<Account> Accounts { get; }
        IGenericRepository<User> Users { get; }
        IGenericRepository<Bank> Banks { get; }
    }
}
