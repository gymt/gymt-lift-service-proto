using System;
using System.Collections.Generic;
using System.Text;

namespace LiftService.Domain.Model
{
    public class Workout
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public List<Set> Sets { get; set; }
    }
}
