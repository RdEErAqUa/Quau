using Quau.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Data.AbbeTest
{
    static class AbbeTest
    {
        static public double AbbeTestRun(StatisticSample statisticSample)
        {
            double d = 0;
            for (int i = 0; i < statisticSample.Sample.Count - 1; i++)
                d += Math.Pow(statisticSample.Sample.ElementAt(i + 1) - statisticSample.Sample.ElementAt(i), 2);

            d *= (1.0 / (statisticSample.Sample.Count - 1));

            double q = d / (2.0 * Math.Pow(statisticSample.QuantitiveCharactacteristics.ElementAt(0).RouteMeanSquare, 2.0));

            return (q - 1.0) * Math.Sqrt((Math.Pow(statisticSample.Sample.Count - 1.0, 2.0) - 1.0)  / (statisticSample.Sample.Count - 2.0));
        }
    }
}
