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
        //M - class count
        public double ClassSize { get; set; }

        public double StepSize { get; set; }
        public ICollection<double> Sample { get; set; }

        public SamplePrimaryStatisticAnalyse SamplePrimaryAnalyse { get; set; }

        public ICollection<SampleRanking> SampleDataRanking { get; set; }

        public ICollection<SamplePrimaryDivisionINClass> SampleDivisionINClass { get; set; }
    }
}
