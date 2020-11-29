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

            double pz = valueSample.SampleDivisionINClass.First().SampleDivisionDataRelativeFrequency;

            answer.Add(new DataValueHistogram { x = valueSample.SampleDivisionINClass.First().SampleDivisionData, p = pz });
            foreach (var el in valueSample.SampleDivisionINClass)
            {
                if (el == valueSample.SampleDivisionINClass.First()) continue;
                else
                {
                    pz += el.SampleDivisionDataRelativeFrequency;
                    answer.Add(new DataValueHistogram { x =  Math.Round(el.SampleDivisionData, valueSample.RoundValue), p = Math.Round(pz, valueSample.RoundValue) });
                }
            }

            return answer;
        }
    }
}
