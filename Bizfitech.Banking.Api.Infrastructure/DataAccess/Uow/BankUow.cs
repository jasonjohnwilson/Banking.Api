using Bizfitech.Banking.Api.Core.Interfaces;
using Bizfitech.Banking.Api.Core.Models;
using Bizfitech.Banking.Api.Infrastructure.DataAccess.Contexts;
using Bizfitech.Banking.Api.Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Bizfitech.Banking.Api.Infrastructure.DataAccess.Uow
{
    public class BankUow : IBankUow
    {
        private readonly DbContext _context;
        private readonly IFactory _factory;

        public BankUow(IFactory factory, BankContext context)
        {
            _factory = factory;
            _context = context;
        }

        public IGenericRepository<User> Users
        {
            get { return _factory.Get<GenericRepository<User>>(_context); }
        }

        public IGenericRepository<Account> Accounts
        {
            get { return _factory.Get<GenericRepository<Account>>(_context); }
        }

        public IGenericRepository<Bank> Banks
        {
            get { return _factory.Get<GenericRepository<Bank>>(_context); }
        }
        
        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveAllChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
