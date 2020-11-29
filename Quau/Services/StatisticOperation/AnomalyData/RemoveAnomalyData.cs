using Quau.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Services.StatisticOperation.AnomalyData
{
    static class RemoveAnomalyData
    {
        static public StatisticSample Log(StatisticSample valueSample)
        {
            ObservableCollection<double> ourXi = new ObservableCollection<double> { };
            foreach (var el in valueSample.Sample)
            {
                double ourNum = Math.Log(el);

                ourXi.Add(ourNum);
            }
            valueSample.Sample = ourXi;

            return valueSample;
        }

        static public StatisticSample Standartization(StatisticSample valueSample)
        {
            double MED = QuantitiveCharacteristicsService.MEDFind(valueSample.Sample);
            double MAD = QuantitiveCharacteristicsService.MADFind(valueSample.Sample);

            ObservableCollection<double> ourXi = new ObservableCollection<double> { };
            foreach (var el in valueSample.Sample)
            {
                double ourNum = (el - MED);
                ourNum /= MAD;

                ourXi.Add(ourNum);
            }
            valueSample.Sample = ourXi;
            return valueSample;
        }
        static public StatisticSample Move(StatisticSample valueSample)
        {

            ObservableCollection<double> ourXi = new ObservableCollection<double> { };
            foreach (var el in valueSample.Sample)
            {
                double ourNum = valueSample.Sample.First() + el + 0.1;

                ourXi.Add(ourNum);
            }
            valueSample.Sample = ourXi;
            return valueSample;
        }
    }
}
