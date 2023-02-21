using AYUS_RestAPI.Data;
using AYUS_RestAPI.Entity.Metadata;
using AYUS_RestAPI.Entity.Metadata.Mechanic;
using Microsoft.EntityFrameworkCore;

namespace AYUS_RestAPI.ASP.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<PersonalInformation> personalInformation { get; set; }
        public DbSet<Credential> credential { get; set; }
        public DbSet<AccountStatus> accountStatus { get; set; }
        public DbSet<Wallet> wallets { get; set; }
        public DbSet<Vehicle> vehicles { get; set; }
        public DbSet<Billing> billing { get; set; }
        public DbSet<Service> services { get; set; }
        public DbSet<ServiceOffer> serviceOffers { get; set; }
        public DbSet<Shop> shops { get; set; }
        public DbSet<Session> sessions { get; set; }
        public DbSet<ServiceMapLocationAPI> serviceMaps { get; set; }
        public DbSet<Transaction> transactions { get; set; }
    }
}
