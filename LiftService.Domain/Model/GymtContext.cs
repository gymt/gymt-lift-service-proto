using Microsoft.EntityFrameworkCore;

namespace LiftService.Domain.Model
{
    public class GymtContext : DbContext
    {
        public GymtContext(DbContextOptions<GymtContext> options)
            : base(options)
        {
        }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Lift> Lifts { get; set; }
        public DbSet<LiftSet> LiftSets { get; set; }
        public DbSet<Muscle> Muscles { get; set; }
        public DbSet<MuscleGroup> MuscleGroups { get; set; }

    }
}