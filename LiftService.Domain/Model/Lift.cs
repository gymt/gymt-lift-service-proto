using System;

namespace LiftService.Domain.Model
{
    public class Lift
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
