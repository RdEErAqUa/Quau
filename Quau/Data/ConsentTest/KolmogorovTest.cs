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
            double dmaxplus = 0, dmaxminus = 0;
            dmaxplus = DMaxFindPlus(statisticSamples.SampleDataRanking, statisticSamples.DistributionSampleEmpirical);
            dmaxminus = DMaxFindMinus(statisticSamples.SampleDataRanking, statisticSamples.DistributionSampleEmpirical);
            double dmax = dmaxplus > dmaxminus ? dmaxplus : dmaxminus;

            dmax = dmaxplus;

            double KolmogorovTestValue = 1;

            double KolmogorovValueLoop = 0;

            double z = Math.Sqrt(statisticSamples.SampleDataRanking.Count) * dmax;

            for (double i = -30000; i < 30000; i++)
            {
                double f1 = FunctionOneFind(i);
                double f2 = FunctionTwoFind(i);
                double z2 = Math.Exp(-2.0 * Math.Pow(i, 2.0) * Math.Pow(z, 2.0));
                KolmogorovValueLoop += (Math.Pow(-1.0, i) * Math.Exp(-2.0 * Math.Pow(i, 2.0) * Math.Pow(z, 2.0)));
                //*
                //(1 - (2 * Math.Pow(i, 2) * z) / 3 * Math.Sqrt(statisticSamples.Sample.Count) - 1 / (18 - statisticSamples.Sample.Count)*
                //((f1 - 4 * (f1 + 3) * Math.Pow(i, 2) * Math.Pow(z, 2) + 8 * Math.Pow(i, 4) * Math.Pow(z, 4)) +
                //((Math.Pow(i, 2) * z) / (27 * Math.Sqrt(Math.Pow(statisticSamples.Sample.Count, 3))) *
                //(Math.Pow(f2, 2) / 5 - (4 * (f2 + 45) * Math.Pow(i, 2) * Math.Pow(z, 2)) / 15) + 8 * Math.Pow(i, 4) * Math.Pow(z, 4)))));
            }
            return KolmogorovValueLoop;
        }

        static private double FunctionOneFind(double k)
        {
            return (Math.Pow(k, 2) - 0.5 * (1 - Math.Pow(-1.0, k)));
        }

        static private double FunctionTwoFind(double k)
        {
            return (5 * Math.Pow(k, 2) + 22 - 7.5 * (1 - Math.Pow(-1.0, k)));
        }


        static private double DMaxFindPlus(ICollection<SampleRanking> statisticSample, ICollection<DistributionSamples> statisticFunction)
        {
            double DMAX = 0;

            for(int i = 0; i < statisticFunction.Count; i++)
            {
                double valueHistogram = 0;

                for(int j = 0; j < statisticSample.Count - 1; j++)
                {
                    if(statisticSample.ElementAt(j).SampleData <= statisticFunction.ElementAt(i).X)
                    {
                        valueHistogram += statisticSample.ElementAt(j).SampleDataRelativeFrequency;
                    }
                }

                double value = Math.Abs(valueHistogram - statisticFunction.ElementAt(i).Y);
                if (value > DMAX)
                    DMAX = value;
            }
            return DMAX;
        }
        static private double DMaxFindMinus(ICollection<SampleRanking> statisticSample, ICollection<DistributionSamples> statisticFunction)
        {
            double DMAX = 0;

            for (int i = 1; i < statisticSample.Count; i++)
            {
                double valueHistogram = 0;

                for (int j = 0; j < statisticSample.Count - 1; j++)
                {
                    if (statisticSample.ElementAt(j).SampleData <= statisticFunction.ElementAt(i).X)
                    {
                        valueHistogram += statisticSample.ElementAt(j).SampleDataRelativeFrequency;
                    }
                }

                double value = Math.Abs(valueHistogram - statisticFunction.ElementAt(i - 1).Y);
                if (value > DMAX)
                    DMAX = value;
            }
            return DMAX;
        }
    }
}
