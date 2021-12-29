using AccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountService.DbContexts
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }

        public int Save()
        {
            return base.SaveChanges();
        }
    }
}
