using Quau.Models;
using Quau.Models.DistributionSet;
using Quau.Models.Histograma;
using Quau.Services.StatisticOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Data.ConsentTest
{
    static class KolmogorovTest
    {
        static public double KolmogorovTest_Invoke(StatisticSample statisticSamples, int k = 500)
        {
            ICollection<SampleRanking> SampleCollection = new List<SampleRanking>(statisticSamples.SampleDataRanking);
            ICollection<double> EmpiricalCollection = new List<double>();
            foreach(var el in SampleCollection)
            {
                double fr = 0;

                foreach (var el2 in SampleCollection)
                    if (el2.SampleData <= el.SampleData)
                        fr++;
                EmpiricalCollection.Add(fr / statisticSamples.SampleDataRanking.Count);

            }
            double dmax = DMaxFindPlus(EmpiricalCollection, statisticSamples.DistributionSampleEmpirical);

            double KolmogorovValueLoop = 0;

            double z = Math.Sqrt(statisticSamples.SampleDataRanking.Count) * dmax;

            for (double i = 1; i < 30000; i++)
            {
                KolmogorovValueLoop += (Math.Pow(-1.0, i) * Math.Exp(-2.0 * Math.Pow(i, 2.0) * Math.Pow(z, 2.0)));
            }
            KolmogorovValueLoop = 1 + 2 * KolmogorovValueLoop;
            return Math.Round(KolmogorovValueLoop, 4);
        }

        static private double FunctionOneFind(double k)
        {
            return (Math.Pow(k, 2) - 0.5 * (1 - Math.Pow(-1.0, k)));
        }

        static private double FunctionTwoFind(double k)
        {
            return (5 * Math.Pow(k, 2) + 22 - 7.5 * (1 - Math.Pow(-1.0, k)));
        }


        static private double DMaxFindPlus(ICollection<double> statisticSample, ICollection<DistributionSamples> statisticFunction)
        {
            double value = 0;
            for(int i = 0; i < statisticSample.Count; i++)
            {
                value = statisticSample.ElementAt(i) - statisticFunction.ElementAt(i).Y > value ? statisticSample.ElementAt(i) - statisticFunction.ElementAt(i).Y : value;
            }
            return value;
        }
        static private double DMaxFindMinus(ICollection<double> statisticSample, ICollection<DistributionSamples> statisticFunction)
        {
            double value = 0;
            for (int i = 1; i < statisticSample.Count; i++)
            {
                value = statisticSample.ElementAt(i) - statisticFunction.ElementAt(i - 1).Y > value ? statisticSample.ElementAt(i) - statisticFunction.ElementAt(i - 1).Y : value;
            }
            return value;
        }
    }
}
