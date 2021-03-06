﻿using Quau.Models;
using Quau.Models.DistributionSet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            double AritheticMeanDouble = QuantitiveCharacteristicsService.ArithmeticMeanDouble(valueDoubleTemp);
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

            valueSample.DistributionSample = new ObservableCollection<DistributionSamples>(valueDistributionSample);

            return valueSample;
        }

        static public double lambdaValueExponential(ICollection<SampleRanking> valueSampleRanking)
        {
            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArithmeticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double lambdaValue = 1 / ArithmeticMean;
            return lambdaValue;
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

            valueSample.DistributionSample = new ObservableCollection<DistributionSamples>(valueDistributionSample);

            return valueSample;
        }
        static public StatisticSample EvenDistribution(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArithmeticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double RouteMean = QuantitiveCharacteristicsService.ArithmeticMeanDouble(valueDoubleTemp);
            double lambdaValue = 1 / ArithmeticMean;

            double aValue = ArithmeticMean - Math.Sqrt(3 * (RouteMean - Math.Pow(ArithmeticMean, 2.0)));
            double bValue = ArithmeticMean + Math.Sqrt(3 * (RouteMean - Math.Pow(ArithmeticMean, 2.0)));

            for (int i = 0; i < valueDoubleTemp.Count; i++)
            {
                valueDistributionSample.Add(new DistributionSamples { X = valueDoubleTemp.ElementAt(i), Y = valueDoubleTemp.ElementAt(i) < aValue || valueDoubleTemp.ElementAt(i) > bValue ? 0 : valueSample.StepSize * (1 / (bValue - aValue)) });
            }

            valueSample.DistributionSample = new ObservableCollection<DistributionSamples>(valueDistributionSample);

            return valueSample;
        }
        static public StatisticSample VWeibullDistribution(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArtiheticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double AritheticMeanDouble = QuantitiveCharacteristicsService.ArithmeticMeanDouble(valueDoubleTemp);
            double NSize = valueDoubleTemp.Count;

            double aValue = 1;

            double bValue = 1.5;

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

            valueSample.DistributionSample = new ObservableCollection<DistributionSamples>(valueDistributionSample);

            return valueSample;
        }
        static public StatisticSample ArcSinDistribution(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArtiheticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double AritheticMeanDouble = QuantitiveCharacteristicsService.ArithmeticMeanDouble(valueDoubleTemp);

            double AValue = Math.Sqrt(2) * Math.Sqrt((AritheticMeanDouble - Math.Pow(ArtiheticMean, 2.0)));

            for (int i = 0; i < valueDoubleTemp.Count; i++)
            {
                if (valueDoubleTemp.ElementAt(i) > -AValue && valueDoubleTemp.ElementAt(i) < AValue)
                {
                    valueDistributionSample.Add(new DistributionSamples { X = valueDoubleTemp.ElementAt(i), Y = 1 / (3.14 * Math.Sqrt(AValue * AValue - valueDoubleTemp.ElementAt(i) * valueDoubleTemp.ElementAt(i))) * valueSample.StepSize });
                }
            }

            valueSample.DistributionSample = new ObservableCollection<DistributionSamples>(valueDistributionSample);

            return valueSample;
        }
        //Функція розподілу ймовірностей
        static public StatisticSample NormalDistributionEmpirical(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArtiheticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double AritheticMeanDouble = QuantitiveCharacteristicsService.ArithmeticMeanDouble(valueDoubleTemp);
            double NSize = valueDoubleTemp.Count;

            NSize = NSize / (NSize - 1);

            double ourO = NSize * Math.Sqrt((AritheticMeanDouble - Math.Pow(ArtiheticMean, 2.0)));


            for (int i = 0; i < valueDoubleTemp.Count; i++)
            {
                valueDistributionSample.Add(new DistributionSamples { X = valueDoubleTemp.ElementAt(i), Y = ourFFind((valueDoubleTemp.ElementAt(i) - ArtiheticMean) / ourO) });
            }

            valueSample.DistributionSampleEmpirical = new ObservableCollection<DistributionSamples>(valueDistributionSample);

            double DM = Math.Pow(ourO, 2.0) / valueDoubleTemp.Count;
            double DO = Math.Pow(ourO, 2.0) / (valueDoubleTemp.Count * 2.0);

            double UA = 1.9 + 0.04;

            valueSample.HistogramLowerLimit = new ObservableCollection<Models.Histograma.DataValueHistogram> { };
            valueSample.HistogramUpperLimit = new ObservableCollection<Models.Histograma.DataValueHistogram> { };

            foreach (var el in valueSample.DistributionSampleEmpirical)
            {
                double dFdM = (1.0 / (ourO * Math.Sqrt(2.0 * Math.PI))) * Math.Exp(-(Math.Pow(el.X - ArtiheticMean, 2.0) / (2.0 * Math.Pow(ourO, 2.0))));
                double dFdO = ((el.X - ArtiheticMean) / (Math.Pow(ourO, 2.0) * Math.Sqrt(2.0 * Math.PI))) * Math.Exp(-(Math.Pow(el.X - ArtiheticMean, 2.0) / (2.0 * Math.Pow(ourO, 2.0))));

                double S_Variance = Math.Pow(dFdM, 2.0) * DM + Math.Pow(dFdO, 2.0) * DO;

                double f1 = el.Y + UA * Math.Sqrt(S_Variance), f2 = el.Y - UA * Math.Sqrt(S_Variance);

                f1 = f1 < 0 ? 0 : f1 > 1 ? 1 : f1;
                f2 = f2 < 0 ? 0 : f2 > 1 ? 1 : f2;

                valueSample.HistogramUpperLimit.Add(new Models.Histograma.DataValueHistogram { x = el.X, p = f1 });
                valueSample.HistogramLowerLimit.Add(new Models.Histograma.DataValueHistogram { x = el.X, p = f2 });
            }


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

            valueSample.DistributionSampleEmpirical = new ObservableCollection<DistributionSamples>(valueDistributionSample);

            valueSample.HistogramLowerLimit = new ObservableCollection<Models.Histograma.DataValueHistogram> { };
            valueSample.HistogramUpperLimit = new ObservableCollection<Models.Histograma.DataValueHistogram> { };

            double UA = 1.9 + 0.04;

            foreach (var el in valueSample.DistributionSampleEmpirical)
            {
                double S_Variance = Math.Pow(el.X, 2.0) * Math.Exp(-2 * LambdaValue * el.X) * Math.Pow(LambdaValue, 2.0) / valueDoubleTemp.Count;

                double f1 = el.Y + UA * Math.Sqrt(S_Variance), f2 = el.Y - UA * Math.Sqrt(S_Variance);

                f1 = f1 < 0 ? 0 : f1 > 1 ? 1 : f1;
                f2 = f2 < 0 ? 0 : f2 > 1 ? 1 : f2;

                valueSample.HistogramUpperLimit.Add(new Models.Histograma.DataValueHistogram { x = el.X, p = f1 });
                valueSample.HistogramLowerLimit.Add(new Models.Histograma.DataValueHistogram { x = el.X, p = f2 });
            }


            return valueSample;
        }
        static public StatisticSample EvenDistributionEmpirical(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArtiheticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double AritheticMeanDouble = QuantitiveCharacteristicsService.ArithmeticMeanDouble(valueDoubleTemp);
            double LambdaValue = 1 / ArtiheticMean;

            double AValue = ArtiheticMean - Math.Sqrt(3 * (AritheticMeanDouble - Math.Pow(ArtiheticMean, 2.0)));
            double BValue = ArtiheticMean + Math.Sqrt(3 * (AritheticMeanDouble - Math.Pow(ArtiheticMean, 2.0)));

            for (int i = 0; i < valueDoubleTemp.Count; i++)
            {
                valueDistributionSample.Add(new DistributionSamples { X = valueDoubleTemp.ElementAt(i), Y = valueDoubleTemp.ElementAt(i) < AValue ? 
                    0 : valueDoubleTemp.ElementAt(i) >= AValue && valueDoubleTemp.ElementAt(i) < BValue ?
                    (valueDoubleTemp.ElementAt(i) - AValue) / (BValue - AValue) : 1 });
            }

            valueSample.DistributionSampleEmpirical = new ObservableCollection<DistributionSamples>(valueDistributionSample);

            valueSample.HistogramLowerLimit = new ObservableCollection<Models.Histograma.DataValueHistogram> { };
            valueSample.HistogramUpperLimit = new ObservableCollection<Models.Histograma.DataValueHistogram> { };

            double UA = 1.9 + 0.04;

            double ourDAValue = Math.Pow((BValue - AValue), 2) / (12.0 * valueDoubleTemp.Count);

            double ourDBValue = (Math.Pow((BValue - AValue), 4) + 15.0 * Math.Pow(AValue + BValue, 2) * Math.Pow(BValue - AValue, 2)) / (180.0 * valueDoubleTemp.Count);

            double cov = (AValue + BValue) * Math.Pow(BValue - AValue, 2) / (12 * valueDoubleTemp.Count);

            foreach (var el in valueSample.DistributionSampleEmpirical)
            {
                double S_Variance = (Math.Pow(el.X - BValue, 2.0) / Math.Pow(BValue - AValue, 4.0)) * ourDAValue +
                    (Math.Pow(el.X - AValue, 2.0) / Math.Pow(BValue - AValue, 4.0)) * ourDBValue +
                    ((el.X - AValue) * (el.X - BValue) / Math.Pow(BValue - AValue, 4.0)) * cov;

                double f1 = el.Y + UA * Math.Sqrt(S_Variance), f2 = el.Y - UA * Math.Sqrt(S_Variance);

                f1 = f1 < 0 ? 0 : f1 > 1 ? 1 : f1;
                f2 = f2 < 0 ? 0 : f2 > 1 ? 1 : f2;

                valueSample.HistogramUpperLimit.Add(new Models.Histograma.DataValueHistogram { x = el.X, p = f1 });
                valueSample.HistogramLowerLimit.Add(new Models.Histograma.DataValueHistogram { x = el.X, p = f2 });
            }

            return valueSample;
        }
        static public StatisticSample VWeibullDistributionEmpirical(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArtiheticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double AritheticMeanDouble = QuantitiveCharacteristicsService.ArithmeticMeanDouble(valueDoubleTemp);
            double LambdaValue = 1 / ArtiheticMean;

            double AValue = 1;

            double BValue = 1.5;

            for (int i = 0; i < valueDoubleTemp.Count; i++)
            {
                valueDistributionSample.Add(new DistributionSamples
                {
                    X = valueDoubleTemp.ElementAt(i),
                    Y = (1.0 - Math.Exp(-Math.Pow(valueDoubleTemp.ElementAt(i), BValue) / AValue))
                });
            }

            valueSample.DistributionSampleEmpirical = new ObservableCollection<DistributionSamples>(valueDistributionSample);

            return valueSample;
        }
        static public StatisticSample ArcSinDistributionEmpirical(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArtiheticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double AritheticMeanDouble = QuantitiveCharacteristicsService.ArithmeticMeanDouble(valueDoubleTemp);

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

            valueSample.DistributionSampleEmpirical = new ObservableCollection<DistributionSamples>(valueDistributionSample);

            return valueSample;
        }
        //Довірчі інтервали
        static public String NormalDistributionInterval(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArtiheticMean = Math.Round(QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp), valueSample.RoundValue); //Мат сподівання
            double AritheticMeanDouble = Math.Round(QuantitiveCharacteristicsService.ArithmeticMeanDouble(valueDoubleTemp), valueSample.RoundValue);
            double NSize = valueDoubleTemp.Count;

            NSize = NSize / (NSize - 1);

            double ourO = Math.Round(NSize * Math.Sqrt((AritheticMeanDouble - Math.Pow(ArtiheticMean, 2.0))), valueSample.RoundValue);

            double ourDArithmeticMean = Math.Round(Math.Pow(ourO, 2) / valueDoubleTemp.Count, valueSample.RoundValue);

            double ourDO = Math.Round(Math.Pow(ourO, 2) / (2 * valueDoubleTemp.Count), valueSample.RoundValue);

            ourO = Math.Round(ourO, valueSample.RoundValue);
            ourDArithmeticMean = Math.Round(ourDArithmeticMean, valueSample.RoundValue);
            ourDO = Math.Round(ourDO, valueSample.RoundValue);

            String returnValue = $"\n##############################################" + 
                $"\nДовірче оцінювання: m    |   Sigma" + 
                $"\nОцінка:             {ArtiheticMean}    |    {ourO}" +
                $"\nДисперсія:          {ourDArithmeticMean} | {ourDO}" +
                $"\nВерхня межа:        {ArtiheticMean + Math.Round(Math.Sqrt(ourDArithmeticMean), valueSample.RoundValue)} | {ourO + Math.Round(Math.Sqrt(ourDO), valueSample.RoundValue)}" +
                $"\nНижня межа:         {ArtiheticMean - Math.Round(Math.Sqrt(ourDArithmeticMean), valueSample.RoundValue)} | {ourO - Math.Round(Math.Sqrt(ourDO), valueSample.RoundValue)}" +
                $"\n##############################################";

            return returnValue;



        }
        static public String ExponentialDistributionInterval(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArithmeticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double lambdaValue = 1 / ArithmeticMean;

            double ourDLambda = Math.Pow(lambdaValue, 2) / valueDoubleTemp.Count;

            lambdaValue = Math.Round(lambdaValue, valueSample.RoundValue);
            ourDLambda = Math.Round(ourDLambda, valueSample.RoundValue);

            String returnValue = $"\n##############################################" +
                $"\nДовірче оцінювання: LambdaValue" +
                $"\nОцінка:             {Math.Round(lambdaValue, valueSample.RoundValue)}" +
                $"\nДисперсія:          {Math.Round(ourDLambda, valueSample.RoundValue)}" +
                $"\nВерхня межа:        {lambdaValue + Math.Round(Math.Sqrt(ourDLambda), valueSample.RoundValue)}" +
                $"\nНижня межа:         {lambdaValue - Math.Round(Math.Sqrt(ourDLambda), valueSample.RoundValue)}" +
                $"\n##############################################";

            return returnValue;



        }
        static public String EvenDistributionInterval(ICollection<SampleRanking> valueSampleRanking, StatisticSample valueSample)
        {
            List<DistributionSamples> valueDistributionSample = new List<DistributionSamples> { };

            ICollection<double> valueDoubleTemp = new List<double> { };

            foreach (var el in valueSampleRanking) valueDoubleTemp.Add(el.SampleData);

            double ArithmeticMean = QuantitiveCharacteristicsService.ArithmeticalMean(valueDoubleTemp);
            double RouteMean = QuantitiveCharacteristicsService.ArithmeticMeanDouble(valueDoubleTemp);
            double lambdaValue = 1 / ArithmeticMean;

            double aValue = ArithmeticMean - Math.Sqrt(3 * (RouteMean - Math.Pow(ArithmeticMean, 2.0)));
            double bValue = ArithmeticMean + Math.Sqrt(3 * (RouteMean - Math.Pow(ArithmeticMean, 2.0)));

            double ourDAValue = Math.Pow((bValue - aValue), 2) / (12.0 * valueDoubleTemp.Count);

            double ourDBValue = (Math.Pow((bValue - aValue), 4) + 15.0 * Math.Pow(aValue + bValue, 2) * Math.Pow(bValue - aValue, 2)) / (180.0 * valueDoubleTemp.Count);

            double cov = (aValue + bValue) * Math.Pow(bValue - aValue, 2) / (12 * valueDoubleTemp.Count);
            aValue = Math.Round(aValue, valueSample.RoundValue);
            bValue = Math.Round(bValue, valueSample.RoundValue);
            String returnValue = $"\n##############################################" +
                $"\nДовірче оцінювання: a    |   b" +
                $"\nОцінка:             {aValue}    |    {bValue}" +
                $"\nДисперсія:          {Math.Round(ourDAValue, valueSample.RoundValue)} | {Math.Round(ourDBValue,valueSample.RoundValue)}" +
                $"\nВерхня межа:        {aValue + Math.Round(Math.Sqrt(ourDAValue), valueSample.RoundValue)} | {bValue + Math.Round(Math.Sqrt(ourDBValue), valueSample.RoundValue)}" +
                $"\nНижня межа:         {aValue - Math.Round(Math.Sqrt(ourDAValue), valueSample.RoundValue)} | {bValue - Math.Round(Math.Sqrt(ourDBValue), valueSample.RoundValue)}" +
                $"\n##############################################";

            return returnValue;



        }
        //Приближенная апроксимация функции Ф((x-m)/o)
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
