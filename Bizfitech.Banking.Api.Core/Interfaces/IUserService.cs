using Bizfitech.Banking.Api.Core.Dtos;
using Bizfitech.Banking.Api.Core.Models;
using System.Threading.Tasks;

namespace Bizfitech.Banking.Api.Core.Interfaces
{
    public interface IUserService
    {
        Task<Result<User>> AddAsync(UserCreateDto user);
    }
}
