using Quau.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Services.StatisticOperation
{
    static class StatisticDivisionInClass
    {
        static public void DivisionInClass(StatisticSample valueSample)
        {
            ICollection<SamplePrimaryDivisionINClass> SampleDivisionData = new List<SamplePrimaryDivisionINClass> { };
            //
            double SizeClasses = SizeClassesFind(valueSample);
            valueSample.ClassSize = SizeClasses;
            //
            List<double> xDivision = FindXDivision(valueSample);

            //

            List<int> xDivisionFrequency = FindFrequencyDivision(valueSample, xDivision);
            //

            List<double> RelativeFrequencyDivision = FindRelativeFrequencyDivision(valueSample, xDivisionFrequency);

            //
            for (int i = 0; i < xDivision.Count; i++) SampleDivisionData.Add(
                 new SamplePrimaryDivisionINClass
                 {
                     SampleDivisionData = xDivision[i],
                     SampleDivisionDataFrequency = xDivisionFrequency[i],
                     SampleDivisionDataRelativeFrequency = RelativeFrequencyDivision[i]
                 });

            valueSample.SampleDivisionINClass = SampleDivisionData;
        }

        static private double SizeClassesFind(StatisticSample valueSample)
        {

            if (valueSample.Sample.Count < 100)
            {
                switch (valueSample.Sample.Count % 2)
                {
                    case 0:
                        return Math.Sqrt(valueSample.Sample.Count) - 1;
                    case 1:
                        return Math.Sqrt(valueSample.Sample.Count);
                    default:
                        break;
                }
            }
            else
            {
                switch (valueSample.Sample.Count % 2)
                {
                    case 0:
                        return Math.Pow(valueSample.Sample.Count, 1.0 / 3.0) - 1;
                    case 1:
                        return Math.Pow(valueSample.Sample.Count, 1.0 / 3.0);
                    default:
                        break;
                }
            }
            return 0;
        }

        static private List<double> FindXDivision(StatisticSample valueSample)
        {
            List<double> xDivision = new List<double> { };

            double stepSize = (valueSample.SampleDataRanking.Last().SampleData - valueSample.SampleDataRanking.First().SampleData) / (valueSample.ClassSize);

            valueSample.ClassSize = Math.Ceiling(valueSample.ClassSize);

            valueSample.StepSize = stepSize;

            for (int i = 0; i <= valueSample.ClassSize; i++)
            {
                xDivision.Add((i) * stepSize + valueSample.SampleDataRanking.First().SampleData);
            }

            return xDivision;
        }

        static private List<int> FindFrequencyDivision(StatisticSample valueSample, List<double> xDivision)
        {
            List<int> zFrequencyDivision = new List<int> { };


            for (int i = 0; i < xDivision.Count; i++)
            {
                int count = 0;
                foreach (var el in valueSample.SampleDataRanking)
                {
                    if (i + 1 >= xDivision.Count)
                    {
                        count += valueSample.SampleDataRanking.Last().SampleDataFrequency;
                        break;
                    }
                    else if (xDivision[i] <= el.SampleData && xDivision[i + 1] > el.SampleData)
                    {
                        count += el.SampleDataFrequency;
                    }

                }
                zFrequencyDivision.Add(count);
            }
            return zFrequencyDivision;
        }

        static private List<double> FindRelativeFrequencyDivision(StatisticSample valueSample, List<int> xDivisionFrequency)
        {
            List<double> RelativeFrequencyDivision = new List<double> { };

            foreach(var el in xDivisionFrequency)
            {
                RelativeFrequencyDivision.Add((double)el / (double)valueSample.Sample.Count);
            }

            return RelativeFrequencyDivision;
        }

    }
}
