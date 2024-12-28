using BarisKuafor.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BarisKuafor.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Mesailer> Mesailer { get; set; }
        public DbSet <Berberler> Berberler { get; set; }
        public DbSet<Randevu> Randevu { get; set; }
        public DbSet<BerberBecerileri> BerberBecerileri {  get; set; }
        public DbSet<Beceriler> Beceriler { get; set; }
    }
}
