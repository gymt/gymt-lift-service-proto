using System;

namespace LiftService.Domain.Model
{
    public class Lift
    {
        public Guid Id { get; private set; }
        public Guid WorkoutId { get; set; }
        public Guid Sets { get; set; }
    }
}
