using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace CodingAB.Models
{
    public class CodingABContext : DbContext
    {
        public CodingABContext(DbContextOptions<CodingABContext> options)
        : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<TimeOffRequest> TimeOffRequests { get; set; }
        public IEnumerable TimeOffTypes { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeOffRequest>()
                .Property(t => t.Type)
                .HasConversion<string>();
        }
    }
}
