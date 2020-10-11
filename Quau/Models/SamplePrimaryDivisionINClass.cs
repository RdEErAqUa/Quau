using System.Collections.Generic;

namespace Quau.Models
{
    internal class SamplePrimaryDivisionINClass
    {
        //M - class count
        public double ClassSize { get; set; }
        //xi
        public ICollection<double> SampleDivisionData { get; set; }

        //ni - ni - количество елементов от [xi; x(i+1)]
        public ICollection<double> SampleDivisionDataFrequency { get; set; }

        //pi
        public ICollection<double> SampleDivisionDataRelativeFrequency { get; set; }
    }
}
