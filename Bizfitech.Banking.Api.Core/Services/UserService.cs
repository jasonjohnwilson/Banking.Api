using Bizfitech.Banking.Api.Core.Dtos;
using Bizfitech.Banking.Api.Core.Interfaces;
using Bizfitech.Banking.Api.Core.Models;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Bizfitech.Banking.Api.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IBankUow _bankUow;
        private readonly IAccountService _accountService;

        public UserService(
            IBankUow bankUow,
            IAccountService accountService)
        {
            _bankUow = bankUow;
            _accountService = accountService;
        }

        public async Task<Result<User>> AddAsync(UserCreateDto user)
        {
            try
            {
                if(user == null)
                {
                    throw new ArgumentNullException($"{nameof(user)} cannot be null");
                }

                var bank = (await _bankUow.Banks.GetAllAsync(b => b.Name == user.BankAccount.BankName))
                    .SingleOrDefault();

                if (bank == null)
                {
                    return new Result<User>
                    {
                        ErrorCategory = ErrorCategory.NotFound,
                        Message = $"No bank found matching name {user.BankAccount.BankName}"
                    };
                }

                if (await _accountService.IsAccountValidAsync(user.BankAccount.BankName, user.BankAccount.AccountNumber) == false)
                {
                    return new Result<User>
                    {
                        ErrorCategory = ErrorCategory.NotFound,
                        Message = $"No account found at bank {user.BankAccount.BankName} with account number {user.BankAccount.AccountNumber}"
                    };
                }

                if (await IsAccountAssignedToAnotherUser(user.BankAccount.BankName, user.BankAccount.AccountNumber, user.Username))
                {
                    return new Result<User>
                    {
                        ErrorCategory = ErrorCategory.BadData,
                        Message = $"Account {user.BankAccount.AccountNumber} is already assigned to another user"
                    };
                }

                var userModel = new User
                {
                    Email = user.Email,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Id = Guid.NewGuid()
                };

                var accountModel = new Account
                {
                    Id = Guid.NewGuid(),
                    BankId = bank.Id,
                    AccountNumber = user.BankAccount.AccountNumber,
                    UserId = userModel.Id
                };

                userModel.Accounts.Add(accountModel);
                await _bankUow.Users.AddAsync(userModel);
                await _bankUow.Accounts.AddAsync(accountModel);
                await _bankUow.SaveAllChangesAsync();

                return new Result<User>
                {
                    ErrorCategory = ErrorCategory.NoError,
                    Obj = userModel
                };
            }
            catch(Exception e)
            {
                //todo: add logging 

                return new Result<User>
                {
                    ErrorCategory = ErrorCategory.InternalServerError,
                    Message = e.Message
                };
            }
        }

        private async Task<bool> IsAccountAssignedToAnotherUser(string bankName, string accountNumber, string username)
        {
            //todo: move this into an injected query class
            return (await _bankUow.Accounts.GetAllAsync(
                                    a => a.AccountNumber.Equals(accountNumber, StringComparison.InvariantCultureIgnoreCase) &&
                                    a.Bank.Name.Equals(bankName, StringComparison.InvariantCultureIgnoreCase) &&
                                    a.User.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) == false)).Any();
        }
    }
}
