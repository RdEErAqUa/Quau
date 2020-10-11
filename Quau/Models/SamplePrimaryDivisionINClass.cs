using System.Collections.Generic;

namespace Quau.Models
{
    internal class SamplePrimaryDivisionINClass
    {
        //xi
        public double SampleDivisionData { get; set; }

        //ni - ni - количество елементов от [xi; x(i+1)]
        public int SampleDivisionDataFrequency { get; set; }

        //pi
        public double SampleDivisionDataRelativeFrequency { get; set; }
    }
}
