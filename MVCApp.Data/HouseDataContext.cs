using Microsoft.EntityFrameworkCore;

namespace MVCAppData
{
    public class HouseDataContext : DbContext 
    {
        public DbSet<House> Houses1 { get; set; }

        public HouseDataContext(DbContextOptions<HouseDataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }

    }
}
