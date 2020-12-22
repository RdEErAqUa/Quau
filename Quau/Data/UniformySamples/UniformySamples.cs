using Quau.Models;
using Quau.Services.StatisticOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Data.UniformySamples
{
    static class UniformySamples
    {
        //Збіг середніх для 2 вибірок

        static public double uniformyAverage(ICollection<StatisticSample> values, bool isDependent = false)
        {

            if (values.Count != 2)
                return 0;

            if (!isDependent)
            {
                //Independent 
                double deltaX = values.ElementAt(0).QuantitiveCharactacteristics.AritmeitcMean;
                double deltaY = values.ElementAt(1).QuantitiveCharactacteristics.AritmeitcMean;

                double SX2 = values.ElementAt(0).QuantitiveCharactacteristics.RouteMeanSquare;
                double SY2 = values.ElementAt(1).QuantitiveCharactacteristics.RouteMeanSquare;

                return ((deltaX - deltaY) / (Math.Sqrt((Math.Pow(SX2, 2.0) / values.ElementAt(0).Sample.Count) + (Math.Pow(SY2, 2.0) / values.ElementAt(1).Sample.Count))));
            }
            else
            {
                //Dependent
                List<double> zL = new List<double> { };

                for (int i = 0; i < values.ElementAt(0).Sample.Count; i++)
                {
                    zL.Add(values.ElementAt(0).Sample.ElementAt(i) - values.ElementAt(1).Sample.ElementAt(i));
                }

                double deltaZ = 0;

                foreach (var el in zL)
                    deltaZ += el;
                deltaZ *= (1 / zL.Count);

                double SZ2 = 0;

                foreach (var el in zL)
                    SZ2 += Math.Pow(el - deltaZ, 2.0);
                SZ2 *= (1 / (zL.Count - 1));

                if (SZ2 == 0)
                    return 0;
                else
                    return (deltaZ * Math.Sqrt(zL.Count) / Math.Sqrt(SZ2));
            }


        } //Збіг середніх

        static public double uniformyVariances(ICollection<StatisticSample> values)
        {
            if (values.Count == 2)
            {
                double SX = values.ElementAt(0).QuantitiveCharactacteristics.RouteMeanSquare;
                double SY = values.ElementAt(1).QuantitiveCharactacteristics.RouteMeanSquare;
                if (Math.Pow(SX, 2.0) > Math.Pow(SY, 2.0))
                    return (Math.Pow(SX, 2.0) / Math.Pow(SY, 2.0));
                else
                    return (Math.Pow(SY, 2.0) / Math.Pow(SX, 2.0));
            }
            else //Критерій Бартлетта
            {
                double S1 = 0, deltaN = 0;
                foreach (var el in values)
                {
                    S1 += ((el.Sample.Count - 1) * el.QuantitiveCharactacteristics.RouteMeanSquare);
                    deltaN += (el.Sample.Count - 1);
                }
                double S = S1 / deltaN;

                double B = 0;
                double C = 0;

                foreach (var el in values)
                {
                    B += ((el.Sample.Count - 1) * Math.Log(Math.Pow(el.QuantitiveCharactacteristics.RouteMeanSquare, 2.0) / Math.Pow(S, 2.0)));

                    C += (1.0 / (el.Sample.Count - 1));
                }
                B = -B;
                C -= (1.0 / deltaN);
                C *= (1.0 / (3 * values.Count - 3));
                C = 1 + C;

                return B / C;

            }
        } //Збіс дисперсій

        static private List<double> RankCount(List<double> newSample)
        {
            List<double> Rank = new List<double> { };
            List<double> tempValue = new List<double> { };
            for (int i = 0; i < newSample.Count; i++)
            {
                if (!tempValue.Contains(newSample[i]))
                {
                    tempValue.Add(newSample[i]);
                    double countSize = 0;
                    double z = 0;
                    int k = 0;
                    for (k = i; k < newSample.Count; k++)
                    {
                        if (newSample[k] == newSample[i])
                        {
                            countSize += 1.0;
                            z += ((double)k + 1.0);
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int f = i; f < k; f++)
                    {
                        Rank.Add((z / countSize));
                    }
                }
            }
            return Rank;
        }
        static private Dictionary<double, double> RankForEveryValue(List<double> Rank, List<double> FullSample)
        {
            Dictionary<double, double> answerValue = new Dictionary<double, double> { };
            List<double> tempValue = new List<double> { };
            for (int i = 0; i < FullSample.Count; i++)
            {
                if (!tempValue.Contains(FullSample[i]))
                {
                    answerValue.Add(FullSample[i], Rank[i]);
                    tempValue.Add(FullSample[i]);
                }
            }
            return answerValue;
        }
        static public double uniformyWilkson(ICollection<StatisticSample> values)
        {
            if (values.Count != 2)
                return 0;
            List<double> newSample = new List<double> { };

            foreach (var el in values.ElementAt(0).Sample)
                newSample.Add(el);
            foreach (var el in values.ElementAt(1).Sample)
                newSample.Add(el);

            newSample.Sort();
            List<double> Rank = RankCount(newSample);

            Dictionary<double, double> RankPerElement = RankForEveryValue(Rank, newSample);

            double W = 0;

            foreach (var el in values.ElementAt(0).Sample)
                W += RankPerElement[el];

            double EW = (values.ElementAt(0).Sample.Count * (newSample.Count + 1)) / 2;
            double DW = (values.ElementAt(0).Sample.Count * values.ElementAt(1).Sample.Count * (newSample.Count + 1)) / 12;

            double w = ((W - EW) / Math.Sqrt(DW));

            return w;
        } //Критерій суми Вілксона

        static public double uniformyMannaWhitney(ICollection<StatisticSample> values)
        {
            List<double> newSample = new List<double> { };

            foreach (var el in values.ElementAt(0).Sample)
                newSample.Add(el);
            foreach (var el in values.ElementAt(1).Sample)
                newSample.Add(el);

            newSample.Sort();
            List<double> Rank = RankCount(newSample);

            Dictionary<double, double> RankPerElement = RankForEveryValue(Rank, newSample);

            double W = 0;

            foreach (var el in values.ElementAt(0).Sample)
                W += RankPerElement[el];
            //

            double U = (values.ElementAt(0).Sample.Count * values.ElementAt(1).Sample.Count) + (values.ElementAt(0).Sample.Count * ((values.ElementAt(0).Sample.Count - 1)) / 2) - W;

            double EU = (values.ElementAt(0).Sample.Count * values.ElementAt(1).Sample.Count) / 2;

            double DU = (values.ElementAt(0).Sample.Count * values.ElementAt(1).Sample.Count * (newSample.Count)) / 2;

            return ((U - EU) / Math.Sqrt(DU));


        } //Критерій Манна Уїтні

        static public double uniformyMiddleRanking(ICollection<StatisticSample> values)
        {
            List<double> newSample = new List<double> { };

            foreach (var el in values.ElementAt(0).Sample)
                newSample.Add(el);
            foreach (var el in values.ElementAt(1).Sample)
                newSample.Add(el);

            newSample.Sort();
            List<double> Rank = RankCount(newSample);

            Dictionary<double, double> RankPerElement = RankForEveryValue(Rank, newSample);

            double rankX = 0;
            double rankY = 0;

            foreach (var el in values.ElementAt(0).Sample)
                rankX += RankPerElement[el];
            foreach (var el in values.ElementAt(1).Sample)
                rankY += RankPerElement[el];

            rankX *= (1.0 / values.ElementAt(0).Sample.Count);
            rankY *= (1.0 / values.ElementAt(1).Sample.Count);
            return (rankX - rankY) / (newSample.Count * Math.Sqrt((newSample.Count + 1.0) / (12.0 * (values.ElementAt(0).Sample.Count * values.ElementAt(1).Sample.Count))));
        } //Критерій середніх рангів

        static public double uniformyAnalysisVariance(ICollection<StatisticSample> values)
        {
            double deltaX = 0;

            double n = 0;

            foreach (var el in values)
            {
                double f = el.Sample.Count * el.QuantitiveCharactacteristics.AritmeitcMean;
                n += el.Sample.Count;
                deltaX += f;
            }

            deltaX *= (1.0 / n);

            double SM = 0;

            foreach (var el in values)
            {
                SM += el.Sample.Count * Math.Pow((el.QuantitiveCharactacteristics.AritmeitcMean - deltaX), 2.0);
            }

            SM *= (1.0 / (values.Count - 1.0));

            double SB = 0;

            foreach (var el in values)
            {
                double SBTemp = 0;

                SBTemp = SBTemp + (el.Sample.Count - 1) * Math.Pow(el.QuantitiveCharactacteristics.RouteMeanSquare, 2.0);

                SB += SBTemp;
            }

            SB *= (1.0 / (n - values.Count));

            return SM / SB;
        } //Однофакторний дисперсійний аналіз

        static public double uniformyHTest(ICollection<StatisticSample> values)
        {
            List<double> fullSample = new List<double> { };

            foreach (var el in values)
                foreach (var el2 in el.Sample)
                    fullSample.Add(el2);
            fullSample.Sort();
            List<double> fullRank = RankCount(fullSample);

            Dictionary<double, double> Rank = RankForEveryValue(fullRank, fullSample);

            List<double> deltaW = new List<double> { };

            foreach (var el in values)
            {
                double WValue = 0;
                foreach (var el2 in el.Sample)
                    WValue += Rank[el2];
                deltaW.Add(WValue / (double)el.Sample.Count);
            }

            double H = 0;

            for (int i = 0; i < values.Count; i++)
            {
                double HTemp = 0;
                HTemp += ((Math.Pow(deltaW[i] - (fullSample.Count + 1.0) / 2.0, 2.0) /
                    ((fullSample.Count + 1.0) * (fullSample.Count - values.ElementAt(i).Sample.Count) / (12.0 * values.ElementAt(i).Sample.Count))) *
                    (1.0 - values.ElementAt(i).Sample.Count / fullSample.Count));

                H += HTemp;
            }

            return H;
        } //H - тест

        static public double uniformyGTest15Element(ICollection<StatisticSample> values) // Критерій знаків Q - тест
        {
            List<double> fullSample = new List<double> { };

            for (int i = 0; i < values.ElementAt(0).Sample.Count; i++)
            {
                fullSample.Add(values.ElementAt(0).Sample[i] - values.ElementAt(1).Sample[i]);
            }
            fullSample.Sort();

            double Q = 0;

            List<double> rightSample = new List<double> { };

            foreach (var el in fullSample)
            {
                if (el > 0)
                    rightSample.Add(1);
                else if (el < 0)
                    rightSample.Add(0);
            }

            int S = (int)rightSample.Sum();

            int N = rightSample.Count;

            double a0 = 0;

            for (int i = 0; i < N - S; i++)
            {
                a0 += (FindFactorial(i) / (FindFactorial(N) * FindFactorial(N - i)));
            }
            a0 *= Math.Pow(2, -N);

            return a0;
        }

        static public double uniformyGTestMore15Element(ICollection<StatisticSample> values) // Критерій знаків Q - тест
        {
            List<double> fullSample = new List<double> { };

            for (int i = 0; i < values.ElementAt(0).Sample.Count; i++)
            {
                fullSample.Add(values.ElementAt(0).Sample[i] - values.ElementAt(1).Sample[i]);
            }
            fullSample.Sort();

            double Q = 0;

            List<double> rightSample = new List<double> { };

            foreach (var el in fullSample)
            {
                if (el > 0)
                    rightSample.Add(1);
                else if (el < 0)
                    rightSample.Add(0);
            }

            int S = (int)rightSample.Sum();

            int N = rightSample.Count;

            double SAnswer = (2 * S - 1 - N) / Math.Sqrt(N);

            return SAnswer;
        }
        static public double uniformyKolmogorovaSmirnova(ICollection<StatisticSample> values)
        {
            var data1 = CreateEmpiricalData.CreateEmpiricalDataValue(values.ElementAt(0));

            var data2 = CreateEmpiricalData.CreateEmpiricalDataValue(values.ElementAt(1));

            double max = values.ElementAt(0).Sample.Max() > values.ElementAt(1).Sample.Max() ? values.ElementAt(1).Sample.Max() : values.ElementAt(0).Sample.Max();
            double min = values.ElementAt(0).Sample.Min() < values.ElementAt(1).Sample.Min() ? values.ElementAt(1).Sample.Min() : values.ElementAt(0).Sample.Min();

            List<double> answerValue = new List<double> { };

            for(double i = 0; i < max; i += (Math.Abs(max - min)) * 0.01)
            {
                //Вынести в отдельную функцию
                double x1 = 0;
                for(int j = 0; j < data1.Count - 1; j++)
                {
                    if(i < data1.ElementAt(j + 1).x && i > data1.ElementAt(j).x)
                    {
                        x1 = data1.ElementAt(j).p;
                    }
                }

                double x2 = 0;
                for (int j = 0; j < data2.Count - 1; j++)
                {
                    if (i < data2.ElementAt(j + 1).x && i > data2.ElementAt(j).x)
                    {
                        x2 = data2.ElementAt(j).p;
                    }
                }
                answerValue.Add(Math.Abs(x1 - x2));


            }

            return (1.0 - Math.Exp(-2.0 * Math.Pow(answerValue.Max(), 2.0)));
        }

        static public double uniformyQTest(ICollection<StatisticSample> values)
        {
            List<double> XValue = new List<double> { };

            for (int i = 0; i < values.ElementAt(0).Sample.Count; i++)
            {
                double XSum = 0;
                foreach (var el in values)
                    XSum += el.Sample[i];
                XValue.Add(XSum);
            }

            List<double> YValue = new List<double> { };

            foreach (var el in values)
                YValue.Add(el.Sample.Sum());

            int K = values.Count;

            double Q = 0;

            double Q1 = 0;
            double Q2 = 0;

            for(int i = 0; i < K; i++)
            {
                Q += Math.Pow(YValue[i] - YValue.Average(), 2.0);
            }

            for(int i = 0; i < XValue.Count; i++)
            {
                Q1 += XValue[i];
                Q2 += Math.Pow(XValue[i], 2.0);
            }

            Q = K * (K - 1.0) * (Q) / ((Q1 * K) - (Q2));

            return Math.Abs(Q);
        }

        //Факториал
        static public double FindFactorial(int value)
        {
            if (value == 0)
                return 1;
            else
                return value * FindFactorial(value - 1);
        }
    }
}
