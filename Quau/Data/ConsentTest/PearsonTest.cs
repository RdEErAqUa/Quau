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
            List<double> Sample = new List<double>(statisticSamples.Sample);
            List<double> n = new List<double>();
            List<double> p = new List<double>();
            List<double> E = new List<double>();
            for (int i = 0; i < statisticSamples.ClassSize - 1; i++)
            {
                double nValue = 0;

                foreach(var el in statisticSamples.Sample)
                {
                    if (el > statisticSamples.HistogramDataValue.ElementAt(i).x && el <= statisticSamples.HistogramDataValue.ElementAt(i + 1).x)
                        nValue++;
                }

                double xl = statisticSamples.HistogramDataValue.ElementAt(i).x, xr = statisticSamples.HistogramDataValue.ElementAt(i + 1).x;

                double pl = -1, pr = -1;

                for(int j = 0; j < statisticSamples.DistributionSampleEmpirical.Count; j++)
                {
                    if (xl < statisticSamples.DistributionSampleEmpirical.ElementAt(j).X && pl == -1)
                        pl = statisticSamples.DistributionSampleEmpirical.ElementAt(j).Y;
                    if (xr < statisticSamples.DistributionSampleEmpirical.ElementAt(j).X && pr == -1)
                        pr = statisticSamples.DistributionSampleEmpirical.ElementAt(j).Y;
                }
                pl = pl == -1 ? 0 : pl;
                pr = pr == -1 ? 0 : pr;

                p.Add(pr - pl);
                E.Add(statisticSamples.Sample.Count * p[i]);

                n.Add(nValue);
                if (E[i] != 0)
                    x += (Math.Pow(n[i] - E[i], 2.0) / E[i]);
                else
                    x += 0;
            }

            return Math.Round(x, 4);
        }
    }
}
