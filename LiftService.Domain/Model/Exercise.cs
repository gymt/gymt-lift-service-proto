using System;

namespace LiftService.Domain.Model
{
    public class Exercise
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public Guid MuscleGroupId { get; set; }
    }
}
