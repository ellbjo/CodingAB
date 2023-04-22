using Microsoft.EntityFrameworkCore;

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
    }
}
