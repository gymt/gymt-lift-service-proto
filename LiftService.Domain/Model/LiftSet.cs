using System;
using System.Collections.Generic;
using System.Text;

namespace LiftService.Domain.Model
{
    public class LiftSet
    {
        public long Id { get; private set; }
        public int? NumberOfReps { get; set; }
        public float? Weight { get; set; }
    }
}
