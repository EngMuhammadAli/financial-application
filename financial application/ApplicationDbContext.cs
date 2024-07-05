using financial_application.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace financial_application
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }

}
