using Quau.Models;
using Quau.Services.StatisticOperation.DistributionCalculate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Data.Modeling
{
    static class ModelingElement
    {
        static public ObservableCollection<StatisticSample> modelingElement(int num, double LambdaValue, int itteration, ObservableCollection<StatisticSample> statisticSamples,ObservableCollection<double> tValue = null)
        {
            if (num <= 0) return null;

            Random rand = new Random();

            statisticSamples = new ObservableCollection<StatisticSample> { };

            for (int i = 0; i < itteration; i++)
            {
                List<double> sample = new List<double> { };

                for(int j = 0; j < num; j++)
                {
                    sample.Add(Math.Round(-(Math.Log(rand.NextDouble()) / LambdaValue), 4));
                }

                sample.Sort();

                statisticSamples.Add(new StatisticSample { Sample = new ObservableCollection<double>(sample) });
                double lambdaValue = (1.0 / statisticSamples.Last().QuantitiveCharactacteristics.AritmeitcMean);
                tValue?.Add(Math.Round((lambdaValue - LambdaValue) / (statisticSamples.Last().QuantitiveCharactacteristics.RouteMeanSquare) * Math.Sqrt(num), 4));
            }
            return statisticSamples;
        }
    }
}
