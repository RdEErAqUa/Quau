using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Models.ModelingSample
{
    internal class TSample
    {
        public int Size { get; set; }
        public double tDelta { get; set; }
        public double tDeltaSquare { get; set; }
        public double aValue { get; set; }

        public double criticalValue { get; set; }
        public bool isPassed { get; set; }

        public TSample(int Size, double tDelta, double tDeltaSquare, double aValue, double criticalValue, bool isPassed)
        {
            this.Size = Size;
            this.tDelta = tDelta;
            this.tDeltaSquare = tDeltaSquare;
            this.aValue = aValue;
            this.criticalValue = criticalValue;
            this.isPassed = isPassed;
        }
    }
}
