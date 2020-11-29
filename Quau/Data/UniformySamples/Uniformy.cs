using Quau.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Data.UniformySamples
{
    static class Uniformy
    {
        static public String UniformyRunIndependent(ICollection<StatisticSample> valuesSample, String RecordValue)
        {
            int sizeOfValue = valuesSample.Count;
            if(sizeOfValue == 2)
            {
                double uniformyVariances = Math.Abs(UniformySamples.uniformyVariances(valuesSample));
                //
                int v1 = valuesSample.ElementAt(0).Sample.Count - 1 < 10 ? 10 : (valuesSample.ElementAt(0).Sample.Count - 1 < 30 ? 30 : 120), 
                    v2 = valuesSample.ElementAt(0).Sample.Count - 1 < 10 ? 10 : (valuesSample.ElementAt(0).Sample.Count - 1 < 30 ? 30 : 120);

                Quantiles quantiles = new Quantiles();
                quantiles.FQuantiles();
                //
                double f = v1 == 10 ? (quantiles.F_v1_10_a0_05[v2]) : (v1 == 30 ? (quantiles.F_v1_30_a0_05[v2]) : (quantiles.F_v1_120_a0_05[v2]));

                String uniformyVariacnesValue = f > uniformyVariances ? $"\nЗбіг дисперсій - {uniformyVariances} < {f} - вибірки однорідні" 
                    : $"\nЗбіг дисперсій - {uniformyVariances} > {f} - вибірки неоднорідні";
                RecordValue += uniformyVariacnesValue;
                double A = 0;
                if(f > uniformyVariances)
                {
                    double uniformyAverage = Math.Abs(UniformySamples.uniformyAverage(valuesSample));
                    //
                    int v = (valuesSample.ElementAt(0).Sample.Count + valuesSample.ElementAt(0).Sample.Count - 2) < 10 ? 10 : (valuesSample.ElementAt(0).Sample.Count + valuesSample.ElementAt(0).Sample.Count - 2) < 30 ? 30 : 120;

                    A = (valuesSample.ElementAt(0).Sample.Count + valuesSample.ElementAt(1).Sample.Count) < 100 ? 0.5 
                        : (valuesSample.ElementAt(0).Sample.Count + valuesSample.ElementAt(1).Sample.Count) < 500 ? 0.25 : 0.1;
                    //
                    quantiles.TQuantiles();
                    double t = A == 0.5 ? (quantiles.T_a0_5[v]) : (A == 0.25 ? (quantiles.T_a0_25[v]) : (quantiles.T_a0_1[v]));

                    String uniformyAverageValue = t > uniformyAverage ? $"\nЗбіг середніх - {uniformyAverage} < {t} - вибірки однорідні"
                    : $"\nЗбіг середніх - {uniformyAverage} > {t} - вибірки неоднорідні";
                    RecordValue += uniformyAverageValue;
                }
                else
                {
                    RecordValue += "\nКритерій середніх - не пройденно перевірку Збігу Дисперсій";
                }
                //Критерій Вілксона

                A = (valuesSample.ElementAt(0).Sample.Count + valuesSample.ElementAt(1).Sample.Count) < 100 ? 0.5
                        : (valuesSample.ElementAt(0).Sample.Count + valuesSample.ElementAt(1).Sample.Count) < 500 ? 0.25 : 0.1;
                double UA = A == 0.5? 0.7 : 2;
                double uniformyWilksona = Math.Abs(UniformySamples.uniformyWilkson(valuesSample));

                String uniformyWilksonaValue = uniformyWilksona < UA ? $"\nКритерій Вілксона - {uniformyWilksona} < {UA} - вибірки однорідні" : 
                    $"\nКритерій Вілксона - {uniformyWilksona} > {UA} - вибірки неоднорідні";
                RecordValue += uniformyWilksonaValue;
                //Критерій Манна-Уїтні
                double uniformyMannaWhitney = Math.Abs(UniformySamples.uniformyMannaWhitney(valuesSample));

                String uniformyMannaWhitneyValue = uniformyMannaWhitney < UA ? $"\nКритерій Манна-Уїтні - {uniformyMannaWhitney} < {UA} - вибірки однорідні" :
                    $"\nКритерій Манна-Уїтні - {uniformyMannaWhitney} > {UA} - вибірки неоднорідні";
                RecordValue += uniformyMannaWhitneyValue;
                //Критерій Різниці стандартних рангів
                double uniformyMiddleRanking = Math.Abs(UniformySamples.uniformyMiddleRanking(valuesSample));

                String uniformyMiddleRankingValue = uniformyMiddleRanking < UA ? $"\nКритерій Різниці стандартних рангів - {uniformyMiddleRanking} < {UA} - вибірки однорідні" :
                    $"\nКритерій Різниці стандартних рангів - {uniformyMiddleRanking} > {UA} - вибірки неоднорідні";
                RecordValue += uniformyMiddleRankingValue;
            }
            else if(sizeOfValue > 2)
            {
                double uniformyVariances = Math.Abs(UniformySamples.uniformyVariances(valuesSample));
                //
                int v1 = valuesSample.Count < 90 ? valuesSample.Count : 90;

                Quantiles quantiles = new Quantiles();
                quantiles.FQuantiles();
                //
                double A = (valuesSample.ElementAt(0).Sample.Count + valuesSample.ElementAt(1).Sample.Count) < 50 ? 0.5 : 0.2;
                quantiles.XI2Quantiles();
                double XI2 = A == 0.5 ? quantiles.XI2_a0_5[v1] : quantiles.XI2_a0_2[v1];

                String uniformyVariacnesValue = XI2 > uniformyVariances ? $"\nКритерій Бартлетта - {uniformyVariances} < {XI2} - вибірки однорідні"
                    : $"\nКритерій Бартлетта - {uniformyVariances} > {XI2} - вибірки неоднорідні";
                RecordValue += uniformyVariacnesValue;
                //Однофакторний дисперсійний аналіз
                int N = 0;

                foreach (var el in valuesSample)
                    N += el.Sample.Count;

                v1 = valuesSample.Count - 1 < 10 ? 10 : (valuesSample.Count - 1 < 30 ? 30 : 120);
                int v2 = N < 10 ? 10 : (N < 30 ? 30 : 120);

                double f = v1 == 10 ? (quantiles.F_v1_10_a0_05[v2]) : (v1 == 30 ? (quantiles.F_v1_30_a0_05[v2]) : (quantiles.F_v1_120_a0_05[v2]));

                double uniformyAnalysisVariance = UniformySamples.uniformyAnalysisVariance(valuesSample);

                String uniformyAnalysisVarianceValue = f > uniformyAnalysisVariance ? $"\nОднофакторний дисперсійний аналіз - {uniformyAnalysisVariance} < {f} - вибірки однорідні"
                    : $"\nОднофакторний дисперсійний аналіз - {uniformyAnalysisVariance} > {f} - вибірки неоднорідні";
                RecordValue += uniformyAnalysisVarianceValue;
                //Критерій Вілксона(H) для k > 2

                double uniformyHTest = UniformySamples.uniformyHTest(valuesSample);

                String uuniformyHTestValue = uniformyHTest < XI2 ? $"\nКритерій Вілксона(H тест) - {uniformyHTest} < {XI2} - виборки однорідні"
                    : $"\nКритерій Вілксона(H тест) - {uniformyHTest} > {XI2} - виборки неоднорідні";
                RecordValue += uuniformyHTestValue;
            }
            return RecordValue;  
        }

        static public String UniformyRunDependent(ICollection<StatisticSample> valuesSample, String RecordValue)
        {
            int sizeOfValue = valuesSample.Count;
            if (sizeOfValue == 2)
            {
                Quantiles quantiles = new Quantiles();

                double uniformyAverage = Math.Abs(UniformySamples.uniformyAverage(valuesSample));
                //
                int v = (valuesSample.ElementAt(0).Sample.Count + valuesSample.ElementAt(0).Sample.Count - 2) < 10 ? 10 : (valuesSample.ElementAt(0).Sample.Count + valuesSample.ElementAt(0).Sample.Count - 2) < 30 ? 30 : 120;

                double A = (valuesSample.ElementAt(0).Sample.Count + valuesSample.ElementAt(1).Sample.Count) < 100 ? 0.5
                    : (valuesSample.ElementAt(0).Sample.Count + valuesSample.ElementAt(1).Sample.Count) < 500 ? 0.25 : 0.1;
                //
                quantiles.TQuantiles();
                double t = A == 0.5 ? (quantiles.T_a0_5[v]) : (A == 0.25 ? (quantiles.T_a0_25[v]) : (quantiles.T_a0_1[v]));

                String uniformyAverageValue = t > uniformyAverage ? $"\nЗбіг середніх - {uniformyAverage} < {t} - вибірки однорідні"
                : $"\nЗбіг середніх - {uniformyAverage} > {t} - вибірки неоднорідні";
                RecordValue += uniformyAverageValue;
                A = (valuesSample.ElementAt(0).Sample.Count + valuesSample.ElementAt(1).Sample.Count) < 100 ? 0.5
                        : (valuesSample.ElementAt(0).Sample.Count + valuesSample.ElementAt(1).Sample.Count) < 500 ? 0.25 : 0.1;
                double UA = A == 0.5 ? 0.7 : 2;
                //Критерій Вілксона

                double uniformyWilksona = Math.Abs(UniformySamples.uniformyWilkson(valuesSample));

                String uniformyWilksonaValue = uniformyWilksona < UA ? $"\nКритерій Вілксона - {uniformyWilksona} < {UA} - вибірки однорідні" :
                    $"\nКритерій Вілксона - {uniformyWilksona} > {UA} - вибірки неоднорідні";
                RecordValue += uniformyWilksonaValue;
                //Критерій Манна-Уїтні
                double uniformyMannaWhitney = Math.Abs(UniformySamples.uniformyMannaWhitney(valuesSample));

                String uniformyMannaWhitneyValue = uniformyMannaWhitney < UA ? $"\nКритерій Манна-Уїтні - {uniformyMannaWhitney} < {UA} - вибірки однорідні" :
                    $"\nКритерій Манна-Уїтні - {uniformyMannaWhitney} > {UA} - вибірки неоднорідні";
                RecordValue += uniformyMannaWhitneyValue;
                //Критерій Різниці стандартних рангів
                double uniformyMiddleRanking = Math.Abs(UniformySamples.uniformyMiddleRanking(valuesSample));

                String uniformyMiddleRankingValue = uniformyMiddleRanking < UA ? $"\nКритерій Різниці стандартних рангів - {uniformyMiddleRanking} < {UA} - вибірки однорідні" :
                    $"\nКритерій Різниці стандартних рангів - {uniformyMiddleRanking} > {UA} - вибірки неоднорідні";
                RecordValue += uniformyMiddleRankingValue;
            }
            else if (sizeOfValue > 2)
            {
                double uniformyVariances = Math.Abs(UniformySamples.uniformyVariances(valuesSample));
                //
                int v1 = valuesSample.Count < 90 ? valuesSample.Count : 90;

                Quantiles quantiles = new Quantiles();
                quantiles.FQuantiles();
                //
                double A = (valuesSample.ElementAt(0).Sample.Count + valuesSample.ElementAt(1).Sample.Count) < 50 ? 0.5 : 0.2;
                quantiles.XI2Quantiles();
                double XI2 = A == 0.5 ? quantiles.XI2_a0_5[v1] : quantiles.XI2_a0_2[v1];

                String uniformyVariacnesValue = XI2 > uniformyVariances ? $"\nКритерій Бартлетта - {uniformyVariances} < {XI2} - вибірки однорідні"
                    : $"\nКритерій Бартлетта - {uniformyVariances} > {XI2} - вибірки неоднорідні";
                RecordValue += uniformyVariacnesValue;
                //Однофакторний дисперсійний аналіз
                int N = 0;

                foreach (var el in valuesSample)
                    N += el.Sample.Count;

                v1 = valuesSample.Count - 1 < 10 ? 10 : (valuesSample.Count - 1 < 30 ? 30 : 120);
                int v2 = N < 10 ? 10 : (N < 30 ? 30 : 120);

                double f = v1 == 10 ? (quantiles.F_v1_10_a0_05[v2]) : (v1 == 30 ? (quantiles.F_v1_30_a0_05[v2]) : (quantiles.F_v1_120_a0_05[v2]));

                double uniformyAnalysisVariance = UniformySamples.uniformyAnalysisVariance(valuesSample);

                String uniformyAnalysisVarianceValue = f > uniformyAnalysisVariance ? $"\nОднофакторний дисперсійний аналіз - {uniformyAnalysisVariance} < {f} - вибірки однорідні"
                    : $"\nОднофакторний дисперсійний аналіз - {uniformyAnalysisVariance} > {f} - вибірки неоднорідні";
                RecordValue += uniformyAnalysisVarianceValue;
                //Критерій Вілксона(H) для k > 2

                double uniformyHTest = UniformySamples.uniformyHTest(valuesSample);

                String uuniformyHTestValue = uniformyHTest < XI2 ? $"\nКритерій Вілксона(H тест) - {uniformyHTest} < {XI2} - виборки однорідні" 
                    : $"\nКритерій Вілксона(H тест) - {uniformyHTest} > {XI2} - виборки неоднорідні";
                RecordValue += uuniformyHTestValue;
            }
            return RecordValue;
        }
    }
}
