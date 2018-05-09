using System;
using System.Threading.Tasks;

namespace Bizfitech.Banking.Api.Core.Interfaces
{
    public interface IUowBase : IDisposable
    {
        Task<int> SaveAllChangesAsync();
    }
}
