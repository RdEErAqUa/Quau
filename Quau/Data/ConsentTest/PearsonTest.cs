using Quau.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Data.ConsentTest
{
    static class PearsonTest
    {
        static public double PearsonTest_Invoke(StatisticSample statisticSamples, int k = 500)
        {
            double x = 0;
            for(int i = 0; i < statisticSamples.ClassSize; i++)
            {

                double n0 = statisticSamples.SampleDivisionINClass.ElementAt(i).SampleDivisionDataFrequency;
                double n1 = statisticSamples.SampleDivisionINClass.ElementAt(i).SampleDivisionDataRelativeFrequency * statisticSamples.Sample.Count;
                x += (Math.Pow(n0 - n1, 2) / n1);
            }
            return x;
        }
    }
}
