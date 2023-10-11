using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace CurrencyConverter.Entities
{
    public class CurrencyDbContext : DbContext
    {
        public CurrencyDbContext(DbContextOptions<CurrencyDbContext> options) : base(options)
        {
            
        }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<CurrencyExchangeRate> CurrencyExchangeRates { get; set; }
        public virtual DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>().HasKey(c => c.CurrencyId);
            modelBuilder.Entity<CurrencyExchangeRate>().HasKey(c => c.Id);
            modelBuilder.Entity<User>().HasKey(c => c.UserId);

            
        }
    }
}