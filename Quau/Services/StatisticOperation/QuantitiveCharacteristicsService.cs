using Quau.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Services.StatisticOperation
{
    static class QuantitiveCharacteristicsService
    {
        static public void QuantitiveCharacteristics(StatisticSample valueSample)
        {
            unShiftedShiftedQuantitiveCharacteristics quantitiveCharacteristics = new unShiftedShiftedQuantitiveCharacteristics { };
            //
            quantitiveCharacteristics.MED = Math.Round(MEDFind(valueSample.Sample), valueSample.RoundValue);
            quantitiveCharacteristics.MAD = Math.Round(MADFind(valueSample.Sample), valueSample.RoundValue);
            quantitiveCharacteristics.S_Variance_Shifted = Math.Round(S_Variance_Shifted(valueSample.Sample), valueSample.RoundValue);
            quantitiveCharacteristics.S_Variance_unShifted = Math.Round(S_Variance_unShifte(valueSample.Sample), valueSample.RoundValue);
            quantitiveCharacteristics.AritmeitcMean = Math.Round(ArithmeticalMean(valueSample.Sample), valueSample.RoundValue);
            quantitiveCharacteristics.RouteMeanSquare = Math.Round(RouteMeanSquare(valueSample.Sample), valueSample.RoundValue);
            quantitiveCharacteristics.Skewness_Shifted = Math.Round(Skewness_Shifted(valueSample.Sample), valueSample.RoundValue);
            quantitiveCharacteristics.Skewness_unShifted = Math.Round(Skewness_unShifted(valueSample.Sample), valueSample.RoundValue);
            quantitiveCharacteristics.Kurtosis_Shifted = Math.Round(Kurtosis_Shifted(valueSample.Sample), valueSample.RoundValue);
            quantitiveCharacteristics.Kurtosis_unShifted = Math.Round(Kurtosis_unShifted(valueSample.Sample), valueSample.RoundValue);
            quantitiveCharacteristics.CounterKurtosis = Math.Round(CounterKurtosis(valueSample.Sample), valueSample.RoundValue);
            quantitiveCharacteristics.Pearson_Variation = Math.Round(Pearson_Variation(valueSample.Sample), valueSample.RoundValue);

            double A = valueSample.Sample.Count < 40 ? 0.5 : (valueSample.Sample.Count < 100 ? 0.25 : 0.1);

            int v1 = valueSample.Sample.Count == 10 ? 10 : (valueSample.Sample.Count == 30 ? 30 : 120);

            Quantiles quantiles = new Quantiles();

            quantiles.TQuantiles();

            double ourOdeltaX = Math.Round(ourODeltaX_Find(valueSample.Sample), valueSample.RoundValue);
            double ourOSC = Math.Round(ourOSC_Find(valueSample.Sample), valueSample.RoundValue);
            double ourOAC = Math.Round(ourOAC_Find(valueSample.Sample), valueSample.RoundValue);
            double ourOEC = Math.Round(ourOEC_Find(valueSample.Sample), valueSample.RoundValue);

            double t = A == 0.5 ? (quantiles.T_a0_5[v1]) : (A == 0.25 ? (quantiles.T_a0_25[v1]) : (quantiles.T_a0_1[v1]));

            valueSample.IntervalProtocol = "";
            valueSample.IntervalProtocol += $"\no1      |      0.95%      |      o2" +
                $"\n{quantitiveCharacteristics.AritmeitcMean + Math.Round(t * ourOdeltaX, valueSample.RoundValue)}      |      Середнє      |      {quantitiveCharacteristics.AritmeitcMean - Math.Round(t * ourOdeltaX, valueSample.RoundValue)}" +
                $"\n{quantitiveCharacteristics.RouteMeanSquare + Math.Round(t * ourOSC, valueSample.RoundValue)}      |      Середньоквадратичне      |      {quantitiveCharacteristics.RouteMeanSquare - Math.Round(t * ourOSC, valueSample.RoundValue)}" +
                $"\n{quantitiveCharacteristics.Skewness_Shifted + Math.Round(t * ourOAC, valueSample.RoundValue)}      |      Асиметрія      |      {quantitiveCharacteristics.Skewness_Shifted - Math.Round(t * ourOAC, valueSample.RoundValue)}" +
                $"\n{quantitiveCharacteristics.CounterKurtosis + Math.Round(t * ourOEC, valueSample.RoundValue)}      |      Контрексцесу      |      {quantitiveCharacteristics.CounterKurtosis - Math.Round(t * ourOEC, valueSample.RoundValue)}";
            //

            valueSample.QuantitiveCharactacteristics = quantitiveCharacteristics;
        }

        //MED
        static public double MEDFind(ICollection<double> value) {
            if (value.Count % 2 == 0)
            {
                int k = value.Count;
                k /= 2;
                double MED = value.ElementAt(k) + value.ElementAt(k + 1);
                MED /= 2;
                return MED;
            }
            else
            {
                int k = value.Count - 1;
                k /= 2;
                double MED = value.ElementAt(k + 1);
                return MED;
            }
        }
        //MAD
        static public double MADFind(ICollection<double> value) => (1.483 * MEDFind(value));
        //Дисперсія (зсунена, незсунена)
        static public double S_Variance_Shifted(ICollection<double> value) => Math.Sqrt(CentralMoment(value, 2));
        static public double S_Variance_unShifte(ICollection<double> value) 
        {
            double valueSVariance = 0;
            double XAverage = ArithmeticalMean(value);

            for (int i = 0; i < value.Count; i++)
            {
                valueSVariance += Math.Pow((value.ElementAt(i) - XAverage), 2);
            }

            valueSVariance /= (value.Count - 1);

            valueSVariance = Math.Sqrt(valueSVariance);

            return valueSVariance;
        }
        //Середнє арифметичне
        static public double ArithmeticalMean(ICollection<double> value) {
            double deltaX = 0;

            for (int i = 0; i < value.Count; i++)
            {
                deltaX += value.ElementAt(i);
            }
            deltaX /= value.Count;

            return deltaX;
        }
        //Середнєквадратичне
        static public double RouteMeanSquare(ICollection<double> value) 
        {
            double RouteMean = 0;
            double XAverage = ArithmeticalMean(value);

            for (int i = 0; i < value.Count; i++)
            {
                RouteMean += Math.Pow((value.ElementAt(i) - XAverage), 2);
            }
            RouteMean /= value.Count;

            RouteMean = Math.Sqrt(RouteMean);

            return RouteMean;
        }
        //Серденій Квадратне(для Нормального розподілу) если это читаете, я не знал как назвать эту функцию 12.10.2020
        static public double AritheticMeanDouble(ICollection<double> valueData)
        {
            double returnValue = 0;

            for (int i = 0; i < valueData.Count; i++)
            {
                returnValue = returnValue + (valueData.ElementAt(i) * valueData.ElementAt(i));
            }
            returnValue /= valueData.Count;

            return returnValue;
        }
        //Коефіцієнт асиметрії (зсунений, незсунений)
        static public double Skewness_Shifted(ICollection<double> value) 
        {
            double valueCentralMoment = CentralMoment(value, 3);
            double RouteMean = RouteMeanSquare(value);

            double ourA = valueCentralMoment / Math.Pow(RouteMean, 3);

            return ourA;
        }
        static public double Skewness_unShifted(ICollection<double> value) 
        {
            double ourA = Skewness_Shifted(value);

            int N = value.Count;

            double ourAShifted = ((Math.Sqrt(N * (N - 1))) / (N - 2)) * ourA;

            return ourAShifted;
        }
        //Коефіцієнт ексцесу (зсунений, незсунений)
        static public double Kurtosis_Shifted(ICollection<double> value) 
        {
            double valueCentralMoment = CentralMoment(value, 4);
            double RouteMean = RouteMeanSquare(value);

            double ourE = valueCentralMoment / Math.Pow(RouteMean, 4);

            return ourE;
        }
        static public double Kurtosis_unShifted(ICollection<double> value) 
        {
            double valueE = Kurtosis_Shifted(value);

            double N = value.Count;
            double valueEC = (N * N - 1) / ((N - 2) * (N - 3)) * ((valueE - 3) + (6 / (N + 1)));


            return valueEC;
        }
        //Коефіцієнт контрексцесу
        static public double CounterKurtosis(ICollection<double> value) => 1.0 / Math.Sqrt(Math.Abs(Kurtosis_unShifted(value)));

        //Варіація пірсона
        static public double Pearson_Variation(ICollection<double> value) => RouteMeanSquare(value) / ArithmeticalMean(value);
        //Усічене середнє

        //Базові кількістні характеристики
        //Початковий статистичний момент
        static public double InitialStatisticMoment(ICollection<double> value, int power)
        {
            double valueInitial = 0;

            for (int i = 0; i < value.Count; i++)
            {
                valueInitial += Math.Pow(value.ElementAt(i), power);
            }

            valueInitial = valueInitial * (1.0 / (double)value.Count);

            return valueInitial;
        }
        //Центральний момент
        static public double CentralMoment(ICollection<double> value, int power)
        {
            double valueCentral = 0;
            double valueInitial = InitialStatisticMoment(value, 1);

            for (int i = 0; i < value.Count; i++)
            {
                valueCentral += Math.Pow((value.ElementAt(i) - valueInitial), power);
            }

            valueCentral /= value.Count;

            return valueCentral;
        }
        //



        //Переделать
        //O(deltaX)
        static public double ourODeltaX_Find(ICollection<double> ourNum1)
        {
            double ourAnswer = S_Variance_unShifte(ourNum1);

            ourAnswer /= Math.Sqrt(ourNum1.Count);

            return ourAnswer;
        }
        //O(S)
        static public double ourOSC_Find(ICollection<double> ourNum1)
        {
            double ourAnswer = S_Variance_unShifte(ourNum1);

            ourAnswer /= Math.Sqrt(2 * ourNum1.Count);

            return ourAnswer;
        }
        //O(A)
        static public double ourOAC_Find(ICollection<double> ourNum1)
        {
            double ourBCount = 4 * ourB_Find(ourNum1, 4);
            double ourAnswer = 0;

            ourBCount -= (12 * ourB_Find(ourNum1, 3));
            ourBCount -= (24 * ourB_Find(ourNum1, 2));
            ourBCount += (9 * ourB_Find(ourNum1, 2) * ourB_Find(ourNum1, 1));
            ourBCount += (35 * ourB_Find(ourNum1, 1));
            ourAnswer = ourBCount;

            ourAnswer -= 36;

            double ourn = 1.0 / (4.0 * (double)ourNum1.Count);

            ourAnswer = ourAnswer * ourn;

            if (ourAnswer > 0)
            {
                ourAnswer = Math.Sqrt(ourAnswer);
            }
            else
            {
                ourAnswer = 1;
                ourAnswer -= (12 / (2 * ourNum1.Count + 7));
                ourAnswer *= 6;
                ourAnswer /= ourNum1.Count;
                ourAnswer = Math.Sqrt(ourAnswer);
            }
            return ourAnswer;
        }

        static public double ourOEC_Find(ICollection<double> ourNum1)
        {
            double ourAnswer = 0;

            double ourBCount = ourB_Find(ourNum1, 6);

            ourBCount -= (4 * ourB_Find(ourNum1, 4) * ourB_Find(ourNum1, 2));
            ourBCount -= (8 * ourB_Find(ourNum1, 3));
            ourBCount += (4 * ourB_Find(ourNum1, 2) * ourB_Find(ourNum1, 2) * ourB_Find(ourNum1, 2));
            ourBCount -= (ourB_Find(ourNum1, 2) * ourB_Find(ourNum1, 2));
            ourBCount += (16 * ourB_Find(ourNum1, 2) * ourB_Find(ourNum1, 1));
            ourBCount += (16 * ourB_Find(ourNum1, 1));

            ourAnswer = ourBCount;

            double ourn = 1.0 / (double)ourNum1.Count;
            ourAnswer = ourAnswer * ourn;

            ourAnswer = Math.Sqrt(ourAnswer);

            return ourAnswer;
        }

        static public double ourB_Find(ICollection<double> ourNum, int count)
        {
            if (count % 2 == 0)
            {
                int k = count / 2;

                double middleMomentum2k2 = CentralMoment(ourNum, 2 * k + 2);
                double middleMomentum2 = CentralMoment(ourNum, 2);

                double ourAnswer = middleMomentum2k2 / (Math.Pow(middleMomentum2, k + 1.0));

                return ourAnswer;
            }
            else
            {
                int k = (count - 1) / 2;
                double middleMomentum2k3 = CentralMoment(ourNum, 2 * k + 3);
                double middleMomentum2 = CentralMoment(ourNum, 2);
                double middleMomentum3 = CentralMoment(ourNum, 3);

                double ourAnswer = (middleMomentum3 * middleMomentum2k3) / (Math.Pow(middleMomentum2, k + 3.0));

                return ourAnswer;
            }
        }


    }
}
