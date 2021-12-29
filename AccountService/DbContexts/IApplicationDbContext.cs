using AccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountService.DbContexts
{
    public interface IApplicationDbContext
    {
        DbSet<Account> Accounts { get; set; }
        int Save();
    }
}
