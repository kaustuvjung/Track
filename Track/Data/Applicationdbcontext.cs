using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Track.Models;

namespace Track.Data
{
    public class Applicationdbcontext : IdentityDbContext
    {
        private readonly IConfiguration _configuration;
        public Applicationdbcontext(DbContextOptions<Applicationdbcontext>options, IConfiguration configuration): base(options)
        {
            _configuration=configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("connect_db"));
        }
        public DbSet<ProductClass> ProductTable { get; set; }
        
        public DbSet<VendorClass> VendorTable { get; set; }
        public DbSet<ClinetClass> ClientTable { get; set;}
        public DbSet<OrderClass> OrderTable { get; set; }

        public DbSet<StockClass> StockTable { get; set; }

        public DbSet<CustomerClass> CustomerTable { get; set; }

        public DbSet<ProvinceClass> Province { get; set; }
        public DbSet<DistrictClass> District { get; set; }
        public DbSet<LocalBodyClass> LocalBody { get; set; }

        public DbSet<OrderhasProducts> OrderhasProducts { get; set; }

        public DbSet<BucketClass> Bucket { get; set; }

        public DbSet<BillClass> Bill { get; set; }
        public DbSet<BillhasProductClass> billhasProduct { get; set; }

        public DbSet<PaymentClass> Payment { get; set; }

        public DbSet<ChalaniClass> Chalani { get; set; }

        public DbSet<ChalanihasProductClass> chalanihasProducts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StockClass>().HasIndex(a => a.serial_number).IsUnique();
            modelBuilder.Entity<OrderClass>().HasIndex(a=>a.Invoice_no).IsUnique();
            modelBuilder.Entity<BillClass>().HasIndex(a => a.Billno).IsUnique();
            modelBuilder.Entity<CustomerClass>().HasCheckConstraint("PhoneNumberCheck", "[PhoneNumber] BETWEEN 9800000000 AND 9899999999");
            modelBuilder.Entity<ProvinceClass>().ToTable("Province");
        }
    }
}
