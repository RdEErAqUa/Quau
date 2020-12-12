using Quau.Models;
using Quau.Models.XYModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Services.StatisticOperation
{
    static class QuantitiveCharacteristicsService
    {
        static public void QuantitiveCharacteristics(StatisticSample valueSample)
        {
            var quantitiveCharacteristics = valueSample.QuantitiveCharactacteristics;

            double A = valueSample.Sample.Count < 40 ? 0.5 : (valueSample.Sample.Count < 100 ? 0.25 : 0.1);

            int v1 = valueSample.Sample.Count == 10 ? 10 : (valueSample.Sample.Count == 30 ? 30 : 120);

            Quantiles quantiles = new Quantiles();

            quantiles.TQuantiles();

            double ourOdeltaX = Math.Round(ourODeltaX_Find(valueSample.Sample), valueSample.RoundValue);
            double ourOSC = Math.Round(ourOSC_Find(valueSample.Sample), valueSample.RoundValue);
            double ourOAC = Math.Round(ourOAC_Find(valueSample.Sample), valueSample.RoundValue);
            double ourOEC = Math.Round(ourOEC_Find(valueSample.Sample), valueSample.RoundValue);

            double t = quantiles.T_a0_05[v1];

            double pMax = 0;
            foreach (var el in valueSample.SampleDivisionINClass)
                if (el.SampleDivisionDataRelativeFrequency > pMax)
                    pMax = el.SampleDivisionDataRelativeFrequency;

            valueSample.IntervalProtocol = "";
            valueSample.IntervalProtocol += $"\no1      |      0.95%      |      o2       |        СКВ" +
                $"\n{quantitiveCharacteristics.AritmeitcMean - Math.Round(t * ourOdeltaX, valueSample.RoundValue)}     |      Середнє арифметичне    |      {quantitiveCharacteristics.AritmeitcMean + Math.Round(t * ourOdeltaX, valueSample.RoundValue)}           |            " +
                $"{Math.Round(quantitiveCharacteristics.AritmeitcMean / Math.Sqrt(valueSample.Sample.Count), valueSample.RoundValue)}" +
                $"\n{quantitiveCharacteristics.RouteMeanSquare - Math.Round(t * ourOSC, valueSample.RoundValue)}       |      Середньоквадратичне    |      {quantitiveCharacteristics.RouteMeanSquare + Math.Round(t * ourOSC, valueSample.RoundValue)}           |            " +
                $"{Math.Round(quantitiveCharacteristics.RouteMeanSquare / Math.Sqrt(valueSample.Sample.Count), valueSample.RoundValue)}" +
                $"\n{quantitiveCharacteristics.Skewness - Math.Round(t * ourOAC, valueSample.RoundValue)}              |      Коефіцієнт асиметрії   |      {quantitiveCharacteristics.Skewness + Math.Round(t * ourOAC, valueSample.RoundValue)}           |            " +
                $"{Math.Round(quantitiveCharacteristics.Skewness / Math.Sqrt(valueSample.Sample.Count), valueSample.RoundValue)}" +
                $"\n{quantitiveCharacteristics.CounterKurtosis - Math.Round(t * ourOEC, valueSample.RoundValue)}       |      Коефіц контрексцесу   |      {quantitiveCharacteristics.CounterKurtosis + Math.Round(t * ourOEC, valueSample.RoundValue)}           |            " +
                $"{Math.Round(quantitiveCharacteristics.CounterKurtosis / Math.Sqrt(valueSample.Sample.Count), valueSample.RoundValue)}";
            //

            valueSample.QuantitiveCharactacteristics = quantitiveCharacteristics;

            valueSample.QuantitiveCharactacteristics.AritmeitcMeanConfidenceInterval = new ObservableCollection<XYData> {
                new XYData {X = quantitiveCharacteristics.AritmeitcMean - quantitiveCharacteristics.RouteMeanSquare , Y = pMax/2.0, A = 0.95},
                new XYData {X = quantitiveCharacteristics.AritmeitcMean + quantitiveCharacteristics.RouteMeanSquare, Y = pMax/2.0, A = 0.95} };
        }

        #region MED - (ICollection<double> value) => double - вибіркова медіана
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
        #endregion

        #region MAD - (ICollection<double> value) => double - вибіркова медіана
        static public double MADFind(ICollection<double> value) => (1.483 * MEDFind(value));
        #endregion

        #region S_Variance_Shifted and S_Variance_unShifte : (ICollection<double> value) => double - Вибіркова дисперсія та середньоквадратичне відхилення
        static public double S_Variance_Shifted(ICollection<double> value) => CentralMoment(value, 2);
        static public double S_Variance_unShifted(ICollection<double> value) 
        {
            double answerValue = 0;
            double valueInitial = InitialStatisticMoment(value, 1);

            for (int i = 0; i < value.Count; i++)
            {
                answerValue += Math.Pow((value.ElementAt(i) - valueInitial), 2);
            }

            answerValue /= value.Count - 1;

            return answerValue;
        }

        static public double RouteMeanSquare(ICollection<double> value) => Math.Sqrt(S_Variance_unShifted(value));
        #endregion

        #region Середньоарифметичне : (ICollection<double> value) => double - обчислення середньоарифметичного
        static public double ArithmeticalMean(ICollection<double> value) => InitialStatisticMoment(value, 1);
        #endregion

        //Серденій Квадратне(для Нормального розподілу) если это читаете, я не знал как назвать эту функцию 12.10.2020
        static public double ArithmeticMeanDouble(ICollection<double> valueData)
        {
            double returnValue = 0;

            for (int i = 0; i < valueData.Count; i++)
            {
                returnValue = returnValue + (valueData.ElementAt(i) * valueData.ElementAt(i));
            }
            returnValue /= valueData.Count;

            return returnValue;
        }

        #region Skewness : (ICollection<double> value) => double - коефіцієнт асиметрії
        static public double Skewness(ICollection<double> value) => CentralMoment(value, 3) / Math.Pow(S_Variance_Shifted(value), 3.0/2.0);
        #endregion

        #region SkewnessUnShifted : (ICollection<double> value) => double - коефіцієнт асиметрії(незсунений)
        static public double SkewnessUnShifted(ICollection<double> value) => (Math.Sqrt(value.Count * (value.Count - 1.0)) / (value.Count - 2.0) * Skewness(value));
        #endregion


        #region Kurtosis : (ICollection<double> value) => double - Коефіцієнт ексцесу
        static public double Kurtosis(ICollection<double> value) => CentralMoment(value, 4) / Math.Pow(CentralMoment(value, 2), 2.0);
        #endregion

        #region KurtosisUnShifted : (ICollection<double> value) => double - Коефіцієнт ексцесу(незсунений)
        static public double KurtosisUnShifted(ICollection<double> value) => ((Math.Pow(value.Count, 2.0) - 1.0) / ((value.Count - 2.0) * (value.Count - 3.0)) * 
            ((Kurtosis(value) - 3.0) + 6.0 / (value.Count + 1.0))) ;
        #endregion

        #region CounterKurtosis : (ICollection<double> value) => double - Коефіцієнт контрексцесу
        static public double CounterKurtosis(ICollection<double> value) => 1.0 / Math.Sqrt(Math.Abs(KurtosisUnShifted(value)));
        #endregion

        #region Pearson_Variation : (ICollection<double> value) -> double - обчислення варіації Пірсона із вибірки value
        static public double Pearson_Variation(ICollection<double> value) => RouteMeanSquare(value) / ArithmeticalMean(value);
        #endregion

        #region InitialStatisticMoment : (ICollection<double> value, int power) -> double - обчислення початкового центрального моменту із вибірки value, зі степенью power
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
        #endregion

        #region CentralMoment : (ICollection<double> value, int power) -> double - обчислення центрального моменту із вибірки value, зі степенью power
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
        #endregion

        //Переделать
        //O(deltaX)
        static public double ourODeltaX_Find(ICollection<double> ourNum1)
        {
            double ourAnswer = S_Variance_unShifted(ourNum1);

            ourAnswer /= Math.Sqrt(ourNum1.Count);

            return ourAnswer;
        }
        //O(S)
        static public double ourOSC_Find(ICollection<double> ourNum1)
        {
            double ourAnswer = S_Variance_unShifted(ourNum1);

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
