using AccountService.DbContexts;
using AccountService.Models;
using System.Collections.Generic;
using System.Linq;

namespace AccountService.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IApplicationDbContext _dbContext;

        public AccountRepository(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddAccount(Account account)
        {
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            _dbContext.Accounts.Add(account);
            _dbContext.Save();
        }

        public Account GetAccountById(int id)
        {
            return _dbContext.Accounts.Find(id);
        }

        public Account GetAccountByName(string name)
        {
            return _dbContext.Accounts.SingleOrDefault(account => account.Name == name);
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _dbContext.Accounts.ToList();
        }
    }
}
