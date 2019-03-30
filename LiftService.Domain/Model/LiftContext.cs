using Microsoft.EntityFrameworkCore;

namespace LiftService.Domain.Model
{
    public class LiftContext : DbContext
    {
        public LiftContext(DbContextOptions<LiftContext> options)
            : base(options)
        {
        }

        public DbSet<Lift> Lifts { get; set; }
    }
}