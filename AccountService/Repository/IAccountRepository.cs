using AccountService.Models;
using System.Collections.Generic;

namespace AccountService.Repository
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAccounts();
        Account GetAccountById(int id);
        Account GetAccountByName(string name);
        void AddAccount(Account account);
    }
}
