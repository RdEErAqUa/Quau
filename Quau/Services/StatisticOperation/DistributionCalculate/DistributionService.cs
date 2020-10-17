﻿using Quau.Models;
using Quau.Models.DistributionSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Services.StatisticOperation.DistributionCalculate
{
    static class DistributionService
    {
        //Функція щільності розподілу ймовірностей
        static public StatisticSample NormalDistribution(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArtiheticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double AritheticMeanDouble = QuantitiveCharacteristicsService.AritheticMeanDouble(valueDoubleTemp);
            double NSize = valueDoubleTemp.Count;

            NSize = NSize / (NSize - 1);

            double ourO = NSize * Math.Sqrt((AritheticMeanDouble - Math.Pow(ArtiheticMean, 2.0)));


            for (int i = 0; i < valueDoubleTemp.Count; i++)
            {
                double ourT = 1 / (ourO * Math.Sqrt(2 * Math.PI));
                double ourT2 = Math.Exp(-((Math.Pow(valueDoubleTemp.ElementAt(i) - ArtiheticMean, 2) / (2 * Math.Pow(ourO, 2)))));

                ourT *= ourT2;

                ourT *= valueSample.StepSize;

                valueDistributionSample.Add(new DistributionSamples { X = valueDoubleTemp.ElementAt(i), Y = ourT});
            }

            valueSample.DistributionSample = valueDistributionSample;

            return valueSample;
        }
        static public StatisticSample ExponentialDistribution(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArithmeticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double lambdaValue = 1 / ArithmeticMean;

            for (int i = 0; i < valueDoubleTemp.Count; i++)
            {
                valueDistributionSample.Add(new DistributionSamples { X = valueDoubleTemp.ElementAt(i), Y = valueDoubleTemp.ElementAt(i) < 0 ? 0 : valueSample.StepSize * lambdaValue * Math.Exp((-lambdaValue) * valueDoubleTemp.ElementAt(i)) }) ;
            }

            valueSample.DistributionSample = valueDistributionSample;

            return valueSample;
        }
        static public StatisticSample EvenDistribution(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArithmeticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double RouteMean = QuantitiveCharacteristicsService.AritheticMeanDouble(valueDoubleTemp);
            double lambdaValue = 1 / ArithmeticMean;

            double aValue = ArithmeticMean - Math.Sqrt(3 * (RouteMean - Math.Pow(ArithmeticMean, 2.0)));
            double bValue = ArithmeticMean + Math.Sqrt(3 * (RouteMean - Math.Pow(ArithmeticMean, 2.0)));

            for (int i = 0; i < valueDoubleTemp.Count; i++)
            {
                valueDistributionSample.Add(new DistributionSamples { X = valueDoubleTemp.ElementAt(i), Y = valueDoubleTemp.ElementAt(i) < aValue || valueDoubleTemp.ElementAt(i) > bValue ? 0 : valueSample.StepSize * (1 / (bValue - aValue)) });
            }

            valueSample.DistributionSample = valueDistributionSample;

            return valueSample;
        }
        static public StatisticSample VWeibullDistribution(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArtiheticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double AritheticMeanDouble = QuantitiveCharacteristicsService.AritheticMeanDouble(valueDoubleTemp);
            double NSize = valueDoubleTemp.Count;

            double aValue = QuantitiveCharacteristicsService.CentralMoment(valueDoubleTemp, 3) / Math.Pow(QuantitiveCharacteristicsService.CentralMoment(valueDoubleTemp, 2), 3.0 / 2.0);

            aValue = Math.Exp(-aValue);

            double bValue = 1.2;

            NSize = NSize / (NSize - 1);

            double ourO = NSize * Math.Sqrt((AritheticMeanDouble - Math.Pow(ArtiheticMean, 2.0)));


            for (int i = 0; i < valueDoubleTemp.Count; i++)
            {
                double ourT = bValue / aValue;

                double ourT2 = Math.Pow(valueDoubleTemp.ElementAt(i), bValue - 1.0);

                ourT *= ourT2;

                ourT2 = Math.Exp(-(Math.Pow(valueDoubleTemp.ElementAt(i), bValue) / aValue));

                ourT *= ourT2;

                ourT *= valueSample.StepSize;

                valueDistributionSample.Add(new DistributionSamples { X = valueDoubleTemp.ElementAt(i), Y = ourT });
            }

            valueSample.DistributionSample = valueDistributionSample;

            return valueSample;
        }
        static public StatisticSample ArcSinDistribution(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArtiheticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double AritheticMeanDouble = QuantitiveCharacteristicsService.AritheticMeanDouble(valueDoubleTemp);

            double AValue = Math.Sqrt(2) * Math.Sqrt((AritheticMeanDouble - Math.Pow(ArtiheticMean, 2.0)));

            for (int i = 0; i < valueDoubleTemp.Count; i++)
            {
                if (valueDoubleTemp.ElementAt(i) > -AValue && valueDoubleTemp.ElementAt(i) < AValue)
                {
                    valueDistributionSample.Add(new DistributionSamples { X = valueDoubleTemp.ElementAt(i), Y = 1 / (3.14 * Math.Sqrt(AValue * AValue - valueDoubleTemp.ElementAt(i) * valueDoubleTemp.ElementAt(i))) * valueSample.StepSize });
                }
            }

            valueSample.DistributionSample = valueDistributionSample;

            return valueSample;
        }
        //Функція розподілу ймовірностей
        static public StatisticSample NormalDistributionEmpirical(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArtiheticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double AritheticMeanDouble = QuantitiveCharacteristicsService.AritheticMeanDouble(valueDoubleTemp);
            double NSize = valueDoubleTemp.Count;

            NSize = NSize / (NSize - 1);

            double ourO = NSize * Math.Sqrt((AritheticMeanDouble - Math.Pow(ArtiheticMean, 2.0)));


            for (int i = 0; i < valueDoubleTemp.Count; i++)
            {
                valueDistributionSample.Add(new DistributionSamples { X = valueDoubleTemp.ElementAt(i), Y = ourFFind((valueDoubleTemp.ElementAt(i) - ArtiheticMean) / ourO) });
            }

            valueSample.DistributionSampleEmpirical = valueDistributionSample;

            return valueSample;
        }
        static public StatisticSample ExponentialDistributionEmpirical(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArtiheticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double LambdaValue = 1 / ArtiheticMean;

            for (int i = 0; i < valueDoubleTemp.Count; i++)
            {
                valueDistributionSample.Add(new DistributionSamples { X = valueDoubleTemp.ElementAt(i), Y = valueDoubleTemp.ElementAt(i) < 0 ? 0 : 1 - Math.Exp((-LambdaValue) * valueDoubleTemp.ElementAt(i)) });
            }

            valueSample.DistributionSampleEmpirical = valueDistributionSample;

            return valueSample;
        }
        static public StatisticSample EvenDistributionEmpirical(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArtiheticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double AritheticMeanDouble = QuantitiveCharacteristicsService.AritheticMeanDouble(valueDoubleTemp);
            double LambdaValue = 1 / ArtiheticMean;

            double AValue = ArtiheticMean - Math.Sqrt(3 * (AritheticMeanDouble - Math.Pow(ArtiheticMean, 2.0)));
            double BValue = ArtiheticMean + Math.Sqrt(3 * (AritheticMeanDouble - Math.Pow(ArtiheticMean, 2.0)));

            for (int i = 0; i < valueDoubleTemp.Count; i++)
            {
                valueDistributionSample.Add(new DistributionSamples { X = valueDoubleTemp.ElementAt(i), Y = valueDoubleTemp.ElementAt(i) < AValue ? 
                    0 : valueDoubleTemp.ElementAt(i) >= AValue && valueDoubleTemp.ElementAt(i) < BValue ?
                    (valueDoubleTemp.ElementAt(i) - AValue) / (BValue - AValue) : 1 });
            }

            valueSample.DistributionSampleEmpirical = valueDistributionSample;

            return valueSample;
        }
        static public StatisticSample VWeibullDistributionEmpirical(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArtiheticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double AritheticMeanDouble = QuantitiveCharacteristicsService.AritheticMeanDouble(valueDoubleTemp);
            double LambdaValue = 1 / ArtiheticMean;

            double AValue = ArtiheticMean - Math.Sqrt(3 * (AritheticMeanDouble - Math.Pow(ArtiheticMean, 2.0)));
            double BValue = ArtiheticMean + Math.Sqrt(3 * (AritheticMeanDouble - Math.Pow(ArtiheticMean, 2.0)));

            for (int i = 0; i < valueDoubleTemp.Count; i++)
            {
                valueDistributionSample.Add(new DistributionSamples
                {
                    X = valueDoubleTemp.ElementAt(i),
                    Y = valueDoubleTemp.ElementAt(i) < AValue ?
                    0 : valueDoubleTemp.ElementAt(i) >= AValue && valueDoubleTemp.ElementAt(i) < BValue ?
                    (valueDoubleTemp.ElementAt(i) - AValue) / (BValue - AValue) : 1
                });
            }

            valueSample.DistributionSampleEmpirical = valueDistributionSample;

            return valueSample;
        }
        static public StatisticSample ArcSinDistributionEmpirical(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArtiheticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double AritheticMeanDouble = QuantitiveCharacteristicsService.AritheticMeanDouble(valueDoubleTemp);

            double AValue = Math.Sqrt(2) * Math.Sqrt((AritheticMeanDouble - Math.Pow(ArtiheticMean, 2.0)));

            for (int i = 0; i < valueDoubleTemp.Count; i++)
            {
                if (valueDoubleTemp.ElementAt(i) < -AValue)
                {
                    valueDistributionSample.Add(new DistributionSamples { X = valueDoubleTemp.ElementAt(i), Y = 0 });
                }
                else if (valueDoubleTemp.ElementAt(i) > -AValue && valueDoubleTemp.ElementAt(i) < AValue)
                {
                    double temp = 0.5;
                    temp += (0.5 * Math.Asin(valueDoubleTemp.ElementAt(i) / AValue));

                    if (temp > 1)
                    {
                        temp = 1;
                    }
                    else if (temp < 0)
                    {
                        temp = 0;
                    }
                    valueDistributionSample.Add(new DistributionSamples { X = valueDoubleTemp.ElementAt(i), Y = temp });
                }
                else
                {
                    valueDistributionSample.Add(new DistributionSamples { X = valueDoubleTemp.ElementAt(i), Y = 1 });
                }
            }

            valueSample.DistributionSampleEmpirical = valueDistributionSample;

            return valueSample;
        }

        //ЧТО ЭТО? 14.10.2020! А ЕСЛИ СЕРЬЕЗНО, Я НАДЕЮСЬ ЧТО ТЫ ЭТО ИСПРАВИЛ!
        static private double ourFFind(double ourNum)
        {
            double ourAnswer = 0;
            if (ourNum < 0)
            {
                ourAnswer = 1;
            }

            ourNum = Math.Abs(ourNum);
            double ourNum1 = 1;

            double ourNum2 = 1 / (Math.Sqrt(6.28));
            double ourNum3 = Math.Exp(-(Math.Pow(ourNum, 2) / 2));
            double ourT = 1 / (1 + 0.2316419 * ourNum);
            double ourNum4 = (0.31938153 * ourT - 0.356563782 * ourT * ourT + 1.781477937 * ourT * ourT * ourT - 1.821255978 * ourT * ourT * ourT * ourT + 1.330274429 * ourT * ourT * ourT * ourT * ourT);

            ourNum1 = ourNum1 - ourNum2 * ourNum3 * ourNum4;

            if (ourAnswer == 1)
                return (ourAnswer - ourNum1);
            else
                return ourNum1;
        }
    }
}
