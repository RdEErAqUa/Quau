using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Models
{
    internal class StatisticSample
    {
        //Одновимірна вибірка
        public ICollection<double> Sample { get; set; }

        public SamplePrimaryStatisticAnalyse SamplePrimaryAnalyse { get; set; }
    }

    internal class SampleRanking
    {
        // xl
        public ICollection<double> SampleData { get; set; }
        //nl
        public ICollection<int> SampleDataFrequency { get; set; }
        //pl
        public ICollection<double> SampleDataRelativeFrequency { get; set; }
    }

    internal class SamplePrimaryStatisticAnalyse
    {
        public SampleRanking SampleDataRanking { get; set; }
    }

    internal class SamplePrimaryDivisionINClass
    {
        //xi
        public ICollection<double> SampleDivisionData { get; set; }

        //ni - ni - количество елементов от [xi; x(i+1)]
        public ICollection<double> SampleDivisionDataFrequency { get; set; }

        //pi
        public ICollection<double> SampleDivisionDataRelativeFrequency { get; set; }
    }
}
