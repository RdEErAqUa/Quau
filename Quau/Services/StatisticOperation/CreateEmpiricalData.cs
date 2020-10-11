using Quau.Models;
using Quau.Models.Histograma;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Services.StatisticOperation
{
    static class CreateEmpiricalData
    {
        static public ICollection<DataValueHistogram> CreateEmpiricalDataValue(StatisticSample valueSample)
        {
            List<DataValueHistogram> answer = new List<DataValueHistogram> { };

            double pz = 0;
            foreach (var el in valueSample.SampleDivisionINClass)
            {
                answer.Add(new DataValueHistogram { x = el.SampleDivisionData, p = pz });
                pz += el.SampleDivisionDataRelativeFrequency;
            }

            return answer;
        }
    }
}
