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
            quantitiveCharacteristics.MED = MEDFind(valueSample.Sample);
            quantitiveCharacteristics.MAD = MADFind(valueSample.Sample);
            quantitiveCharacteristics.S_Variance_Shifted = S_Variance_Shifted(valueSample.Sample);
            quantitiveCharacteristics.S_Variance_unShifted = S_Variance_unShifte(valueSample.Sample);
            quantitiveCharacteristics.AitherticMean = ArithmeticalMean(valueSample.Sample);
            quantitiveCharacteristics.RouteMeanSquare = RouteMeanSquare(valueSample.Sample);
            quantitiveCharacteristics.Skewness_Shifted = Skewness_Shifted(valueSample.Sample);
            quantitiveCharacteristics.Skewness_unShifted = Skewness_unShifted(valueSample.Sample);
            quantitiveCharacteristics.Kurtosis_Shifted = Kurtosis_Shifted(valueSample.Sample);
            quantitiveCharacteristics.Kurtosis_unShifted = Kurtosis_unShifted(valueSample.Sample);
            quantitiveCharacteristics.CounterKurtosis = CounterKurtosis(valueSample.Sample);
            quantitiveCharacteristics.Pearson_Variation = Pearson_Variation(valueSample.Sample);
            //

            valueSample.QuantitiveCharactacteristics = new List<unShiftedShiftedQuantitiveCharacteristics> { quantitiveCharacteristics };
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

    }
}
