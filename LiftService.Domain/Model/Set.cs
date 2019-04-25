using System;
using System.Collections.Generic;
using System.Text;

namespace LiftService.Domain.Model
{
    public class Set
    {
        public Guid Id { get; private set; }
        public int? NumberOfReps { get; set; }
        public float? Weight { get; set; }
    }
}
