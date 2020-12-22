using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Quau.Models.Base;
using Quau.Models.DimentionalModel.Two;
using Quau.Models.XYModel;
using Quau.Services.StatisticOperation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Quau.Models
{
    class TwoDimentionalStatisticSample : BaseModel
    {
        #region Двомерная выборка разбитая на x-y.XY - значение
        private StatisticSample _xSample;
        public StatisticSample xSample { get => _xSample; set => Set(ref _xSample, value); }
        #endregion

        #region Двомерная выборка разбитая на x-y. Y - значение
        private StatisticSample _ySample;
        public StatisticSample ySample { get => _ySample; set => Set(ref _ySample, value); }
        #endregion TwoDimensionalSample - сама выборка

        #region 
        private ObservableCollection<XYData> _TwoDimensionalSample2;

        public ObservableCollection<XYData> TwoDimensionalSample2 { get => _TwoDimensionalSample2; set => Set(ref _TwoDimensionalSample2, value); }
        #endregion

        #region 
        private ObservableCollection<(double, double)> _TwoDimensionalSample;

        public ObservableCollection<(double, double)> TwoDimensionalSample { get => _TwoDimensionalSample; set => Set(ref _TwoDimensionalSample, value); }
        #endregion

        #region Темп значение только для графика(heatmap)

        private SeriesCollection _Values;

        public SeriesCollection Values { get => _Values; set => Set(ref _Values, value); }

        #endregion

        #region Темп значение только для графика(heatmap)

        private string[] _LabelPointsX;

        public string[] LabelPointsX { get => _LabelPointsX; set => Set(ref _LabelPointsX, value); }

        #endregion

        #region Темп значение только для графика(heatmap)

        private string[] _LabelPointsY;

        public string[] LabelPointsY { get => _LabelPointsY; set => Set(ref _LabelPointsY, value); }

        #endregion

        #region Темп значение только для графика(heatmap)

        private SeriesCollection _HistogramValues;

        public SeriesCollection HistogramValues { get => _HistogramValues; set => Set(ref _HistogramValues, value); }

        #endregion

        #region 
        private ObservableCollection<TwoDimentionalSample> _TwoDimensionalStatisticSample;

        public ObservableCollection<TwoDimentionalSample> TwoDimensionalStatisticSample { get => _TwoDimensionalStatisticSample; set => Set(ref _TwoDimensionalStatisticSample, value); }
        #endregion

        #region 
        private ObservableCollection<DataXModel> _ValuePerX;

        public ObservableCollection<DataXModel> ValuePerX { get => _ValuePerX; set => Set(ref _ValuePerX, value); }
        #endregion

        #region 
        private ObservableCollection<TwoDimentionalSample> _TwoDimensionalDensityFunction;

        public ObservableCollection<TwoDimentionalSample> TwoDimensionalDensityFunction { get => _TwoDimensionalDensityFunction; set => Set(ref _TwoDimensionalDensityFunction, value); }
        #endregion

        #region 
        private ObservableCollection<XYData> _LinearRegresionMNK;

        public ObservableCollection<XYData> LinearRegresionMNK { get => _LinearRegresionMNK; set => Set(ref _LinearRegresionMNK, value); }
        #endregion

        #region 
        private ObservableCollection<XYData> _LinearRegresionTaylor;

        public ObservableCollection<XYData> LinearRegresionTaylor { get => _LinearRegresionTaylor; set => Set(ref _LinearRegresionTaylor, value); }
        #endregion


        #region (xStep, yStep) : (double, double) - шаг, с каким определяется количество классов для x и y

        private (double, double) _Step;

        public (double, double) Step { get => _Step; set => Set(ref _Step, value); }

        #endregion

        #region (xClassSiZe, xClassSiZe) : (double, double) - размер класса соответственного x и y

        private (double, double) _ClassSize;

        public (double, double) ClassSize { get => _ClassSize; set => Set(ref _ClassSize, value); }

        #endregion
        public bool SeparateInTwoSample()
        {

            TwoDimensionalSample = new ObservableCollection<(double, double)>(TwoDimensionalSample.OrderBy(X => X.Item1).ToList());

            if (TwoDimensionalSample != null && TwoDimensionalSample.Count > 0)
            {
                var xSample = new ObservableCollection<double> { };
                var ySample = new ObservableCollection<double> { };
                var SampleData = new ObservableCollection<DataXModel> { };

                foreach (var el in TwoDimensionalSample)
                {
                    xSample.Add(el.Item1);
                    ySample.Add(el.Item2);
                    if (SampleData.Select(X => X.X).ToList().Contains(el.Item1))
                        SampleData[SampleData.Select(X => X.X).ToList().IndexOf(el.Item1)].Y.Add(el.Item2);
                    else
                        SampleData.Add(new DataXModel { X = el.Item1, Y = new List<double> { el.Item2 } });
                }
                xSample.OrderBy(X => X);
                ySample.OrderBy(X => X);

                this.xSample = new StatisticSample { Sample = xSample };

                this.ySample = new StatisticSample { Sample = ySample };

                double deltaX = (xSample.Last() - xSample.First()) / (this.xSample.ClassSize);

                var SampleDataValue = new ObservableCollection<DataXModel> { };

                for (int i = 0; i < this.xSample.ClassSize; i++)
                {
                    SampleDataValue.Add(new DataXModel { X = xSample.First() + (i + 1 - 0.5) * deltaX, Y = new List<double> { } });
                }

                for(int i = 0; i < SampleData.Count; i++)
                {
                    for(int j = 0; j < SampleDataValue.Count; j++)
                    {
                        if(SampleData[i].X >= SampleDataValue[j].X - 0.5 * deltaX && SampleData[i].X <= SampleDataValue[j].X + 0.5 * deltaX)
                        {
                            foreach (var el in SampleData[i].Y)
                                SampleDataValue[j].Y.Add(el);
                        }
                    }
                }

                this.ValuePerX = SampleDataValue;

                return true;
            }
            return false;
        }

        public void SetClassSize(double xClass, double yClass)
        {
            this.ClassSize = (xClass, yClass);
            (this.xSample.ClassSize, this.ySample.ClassSize) = this.ClassSize;
        }

        public bool SetTwoDimentionalSample()
        {
            if (this.xSample == null || ySample == null) return false;
            if (ClassSize.Item1 <= 0 || ClassSize.Item2 <= 0)
                this.ClassSize = (this.xSample.ClassSize, this.ySample.ClassSize);

            Step = (xSample.StepSize, ySample.StepSize);

            var sample = new ObservableCollection<TwoDimentionalSample> { };

            for (int i = 0; i < xSample.ClassSize; i++)
            {
                for (int i1 = 0; i1 < ySample.ClassSize; i1++)
                {
                    sample.Add(new TwoDimentionalSample { X = this.xSample.SampleDivisionINClass.First().SampleDivisionData + Step.Item1 * i, Y = this.ySample.SampleDivisionINClass.First().SampleDivisionData + Step.Item2 * i1 });

                    var elXLeft = sample[sample.Count - 1].X;
                    var elYLeft = sample[sample.Count - 1].Y;
                    for (int i2 = 0; i2 < TwoDimensionalSample.Count; i2++)
                    {
                        var elSample = TwoDimensionalSample[i2];
                        if (i == xSample.SampleDivisionINClass.Count - 1 && (elSample.Item1 >= elXLeft && elSample.Item1 <= elXLeft + Step.Item1)
                            && (elSample.Item2 >= elYLeft && elSample.Item2 <= elYLeft + Step.Item2))
                        {
                            sample[sample.Count - 1].N++;
                        }
                        else if ((elSample.Item1 >= elXLeft && elSample.Item1 < elXLeft + Step.Item1)
                            && (elSample.Item2 >= elYLeft && elSample.Item2 < elYLeft + Step.Item2))
                        {
                            sample[sample.Count - 1].N++;
                        }
                    }
                }
            }
            this.TwoDimensionalStatisticSample = sample;
            var temp2 = new ObservableCollection<XYData> { };
            foreach (var el in TwoDimensionalSample)
                temp2.Add(new XYData { X = el.Item1, Y = el.Item2 });

            TwoDimensionalSample2 = temp2;

            var values = new SeriesCollection { };

            var SeriesValues = new ChartValues<HeatPoint> { };

            double stepX = Math.Round(Step.Item1, 4);
            double stepY = Math.Round(Step.Item2, 4);

            var z = new List<string> { };
            var zY = new List<string> { };

            for (int i = 0; i < xSample.ClassSize; i++)
            {
                double firstX = Math.Round(sample.First().X, 4) + stepX * i;
                double firstY = Math.Round(sample.First().Y, 4);

                for (int i1 = 0; i1 < ySample.ClassSize; i1++)
                {
                    firstY += stepY;
                    SeriesValues.Add(new HeatPoint(i, i1, sample[i * (int)ySample.ClassSize + i1].N / TwoDimensionalSample.Count));
                    if (!zY.Contains(firstY.ToString())) zY.Add(firstY.ToString());
                }
                z.Add(firstX.ToString());
            }

            LabelPointsX = z.ToArray();
            LabelPointsY = zY.ToArray();

            var GradientStopCollection = new GradientStopCollection
                {
                    new GradientStop(Color.FromRgb(192,192,192), 0),
                    new GradientStop(Color.FromRgb(220,20,60), 1)
                };


            values = new SeriesCollection { new HeatSeries { GradientStopCollection = GradientStopCollection, DrawsHeatRange = false, Values = SeriesValues, DataLabels = true } };
            this.Values = values;

            return true;
        }

        public bool SetHistogramSample()
        {
            var temp = this.TwoDimensionalStatisticSample;
            var temp1 = new ObservableCollection<TwoDimentionalSample> { };
            var HistogramValue = new List<(int, int, double)> { };

            for (int i1 = 0; i1 < xSample.ClassSize; i1++)
            {
                for (int i2 = 0; i2 < ySample.ClassSize; i2++)
                {
                    var iCondition = i1 + 1;

                    double p = DensityFunction(temp[i1 * (int)ySample.ClassSize + i2].X, temp[i1 * (int)ySample.ClassSize + i2].Y) * Step.Item1 * Step.Item2;

                    temp1.Add(new TwoDimentionalSample { X = temp[i1 * (int)ySample.ClassSize + i2].X, Y = temp[i1 * (int)ySample.ClassSize + i2].Y, N = p });

                    HistogramValue.Add((i1, i2, p));
                }
            }
            var values = new SeriesCollection { };

            TwoDimensionalDensityFunction = temp1;

            var Xi2 = Xi2Find();

            var SeriesValues = new ChartValues<HeatPoint> { };
            foreach (var el in HistogramValue)
            {
                SeriesValues.Add(new HeatPoint(el.Item1, el.Item2, el.Item3));
            }

            var GradientStopCollection = new GradientStopCollection
                {
                    new GradientStop(Color.FromRgb(192,192,192), 0),
                    new GradientStop(Color.FromRgb(220,20,60), 1)
                };

            values = new SeriesCollection { new HeatSeries { GradientStopCollection = GradientStopCollection, DrawsHeatRange = false, Values = SeriesValues, DataLabels = true } };

            this.HistogramValues = values;

            return true;
        }

        public double DensityFunction(double x, double y)
        {
            double ox = DispersionX(), oy = DispersionY();

            double xAverage = AritheticMeanX(), yAverage = AritheticMeanY();

            double Correlation = CorrelationCountRating();

            double f0 = (2 * Math.PI * ox * oy * Math.Sqrt(1.0 - Math.Pow(Correlation, 2.0)));

            double f1 = 1.0 / f0;

            double f2 = Math.Pow((x - xAverage) / ox, 2.0) - 2.0 * Correlation * (x - xAverage) * (y - yAverage) / (ox * oy) + Math.Pow((y - yAverage) / oy, 2.0);

            double f3 = -1.0 / (2.0 * (1.0 - Math.Pow(Correlation, 2.0)));

            double f4 = f1 * Math.Exp(f3 * f2);

            return f4;
        }

        public double CorrelationCountRating()
        {
            double ox = DispersionX(), oy = DispersionY();

            double xAverage = AritheticMeanX(), yAverage = AritheticMeanY();

            double XFullAverage = FullAverage();

            double N = TwoDimensionalSample.Count;

            double rxy = (N / (N - 1.0)) * (XFullAverage - xAverage * yAverage) / (ox * oy);

            return rxy;
        }

        public double CorrelationCount()
        {
            double ox = DispersionX(), oy = DispersionY();

            double covaration = Covaration();

            double rxy = covaration / (Math.Sqrt(Math.Pow(ox, 2.0) * Math.Pow(oy, 2.0)));

            return rxy;
        }

        public double AritheticMeanX()
        {
            double answer = 0;
            foreach (var el in TwoDimensionalSample)
                answer += el.Item1;

            return answer * (1.0 / TwoDimensionalSample.Count);
        }

        public double AritheticMeanY()
        {
            double answer = 0;
            foreach (var el in TwoDimensionalSample)
                answer += el.Item2;

            return answer * (1.0 / TwoDimensionalSample.Count);
        }

        public double DispersionX()
        {
            double answer = 0;

            double mean = AritheticMeanX();

            foreach (var el in TwoDimensionalSample)
                answer += Math.Pow(el.Item1 - mean, 2.0);

            return Math.Sqrt(answer * (1.0 / (TwoDimensionalSample.Count - 1.0)));
        }

        public double DispersionY()
        {
            double answer = 0;

            double mean = AritheticMeanY();
            foreach (var el in TwoDimensionalSample)
                answer += Math.Pow(el.Item2 - mean, 2.0);

            return Math.Sqrt(answer * (1.0 / (TwoDimensionalSample.Count - 1.0)));
        }

        public double Covaration()
        {
            double covaration = 0;

            double meanX = AritheticMeanX(), meanY = AritheticMeanY();

            foreach (var el in TwoDimensionalSample)
            {
                covaration += ((el.Item1 - meanX) * (el.Item2 - meanY));
            }

            covaration *= (1.0 / TwoDimensionalSample.Count);

            return covaration;
        }
        public double FullAverage()
        {
            double average = 0;
            foreach (var el in TwoDimensionalSample)
            {
                average += (el.Item1 * el.Item2);
            }
            average *= (1.0 / TwoDimensionalSample.Count);

            return average;
        }

        public double Xi2Find()
        {
            var denstiyFunc = TwoDimensionalDensityFunction;

            var value = TwoDimensionalStatisticSample;

            double Xi2 = 0;

            for (int i = 0; i < xSample.ClassSize; i++)
            {
                for (int j = 0; j < ySample.ClassSize; j++)
                {
                    double p = denstiyFunc[i * (int)ySample.ClassSize + j].N;

                    double p1 = value[i * (int)ySample.ClassSize + j].N / TwoDimensionalSample.Count;

                    Xi2 = p != 0 ? Xi2 + Math.Pow((p1 - p), 2.0) / p : Xi2;
                }
            }
            return Xi2;
        }

        public double TTest()
        {
            double r = CorrelationCountRating();

            double T = r * Math.Sqrt(TwoDimensionalSample.Count - 2) / (Math.Sqrt(1.0 - Math.Pow(r, 2.0)));

            return T;
        }

        public double CorelationRelation()
        {
            double yAverage = ySample.QuantitiveCharactacteristics.AritmeitcMean;

            double f1 = 0;

            foreach (var el in ValuePerX)
            {
                f1 += el.Y.Count * Math.Pow(el.Y.Average() - yAverage, 2.0);
            }
            double f2 = 0;
            foreach (var el in ValuePerX)
            {
                foreach (var el2 in el.Y)
                {
                    f2 += Math.Pow(el2 - yAverage, 2.0);
                }
            }

            return Math.Sqrt(f1 / f2);
        }
        public String buildProtocol(bool buildRankingCorrelation = true)
        {
            var Xi2 = Xi2Find();

            Quantiles quantiles = new Quantiles();

            quantiles.XI2Quantiles();

            int v1 = (int)xSample.ClassSize * (int)ySample.ClassSize - 2;

            v1 = v1 > 80 ? 80 : v1;

            var Xi2Quantilies = quantiles.XI2_a0_2[v1];

            // T - статистика
            double T = TTest();
            //Інтервальне оцінювання r
            double r = CorrelationCount();
            double u0_05 = 1.97;
            double rMin = r + (r * (1 - Math.Pow(r, 2.0))) / (2 * TwoDimensionalSample.Count) - u0_05 * (1 - Math.Pow(r, 2.0)) / Math.Sqrt(TwoDimensionalSample.Count - 1);
            double rMax = r + (r * (1 - Math.Pow(r, 2.0))) / (2 * TwoDimensionalSample.Count) + u0_05 * (1 - Math.Pow(r, 2.0)) / Math.Sqrt(TwoDimensionalSample.Count - 1);

            //
            double tCorrelationSignificant = Math.Abs(r * Math.Sqrt(TwoDimensionalSample.Count - 2) / (Math.Sqrt(1.0 - Math.Pow(r, 2.0))));

            quantiles.TQuantiles();

            int v2 = TwoDimensionalSample.Count - 1 <= 10 ? 10 : (TwoDimensionalSample.Count - 1 <= 30 ? 30 : 120);

            double TQuantile = quantiles.T_a0_05[v2];

            String data = Xi2 < Xi2Quantilies ? "відповідає нормальному розподілу, так як Xi2 < Xi2Quantilies" : "не відповідає нормальному розподілу, так як Xi2 >= Xi2Quantilies";

            String Protocol = $"При a = 0.2. Та Xi2 = {Xi2}, квантиль Xi2 = {Xi2Quantilies}. \n" +
                "Отже вибірка " + data + $"\nВектор середніх значень {{{Math.Round(AritheticMeanX(), 4)}; {Math.Round(AritheticMeanY(), 4)}}}\n" +
                $"кореляційний аналіз: r, та оцінка параметра r\n" +
                $"{CorrelationCount()} - r, {CorrelationCountRating()} - оцінка параметра r\n" +
                $"Довірчий інтервал для r = {{{rMin}; {rMax}}}\n" +
                $"Значущість r, при a = 0.05 є t = {tCorrelationSignificant}, так як квантиль t, при v = {v2} є {TQuantile}: " + (TQuantile > tCorrelationSignificant ? $"{tCorrelationSignificant} < {TQuantile}, отже r - незначуща"
                : $"{tCorrelationSignificant} > {TQuantile}, отже r - значуща");
            //Кореляційне відношення
            if (TQuantile > tCorrelationSignificant)
            {
                double correlationRelation = CorelationRelation();
                double tCorrelationRelationSignificant = correlationRelation == 1 ? 0 : correlationRelation * Math.Sqrt(TwoDimensionalSample.Count - 2) / (Math.Sqrt(1.0 - Math.Pow(correlationRelation, 2.0)));
                Protocol += $"\nКореляційне відношення : {correlationRelation}, так як квантиль t, при v = {v2} є {TQuantile}: " + (TQuantile > tCorrelationRelationSignificant ? $"{tCorrelationRelationSignificant} < {TQuantile}, отже між   n, E - не існує стохастичного зв'язку"
                : $"{tCorrelationRelationSignificant} > {TQuantile}, отже між   n, E - існує стохастичного зв'язку ");
            }

            if (!buildRankingCorrelation) return Protocol;

            var tempSampleX = xSample.Sample.ToList();
            var tempSampleY = ySample.Sample.ToList();

            tempSampleX.Sort();
            tempSampleY.Sort();

            var RankX = RankCount(tempSampleX);
            var RankY = RankCount(tempSampleY);

            var RankingValueX = RankForEveryValue(RankX, tempSampleX);
            var RankingValueYData = RankForEveryValue(RankY, tempSampleY);
            var RankingValueY = new List<(double, double)> { };

            for (int i = 0; i < TwoDimensionalSample.Count; i++)
            {
                double rankY = RankingValueYData.Where(X => X.Item1 == TwoDimensionalSample[i].Item2).Select(X => X.Item2).ToList().First();

                RankingValueY.Add((TwoDimensionalSample[i].Item2, rankY));
            }

            List<double> DL = new List<double> { };

            double tC = 0;

            for (int i = 0; i < TwoDimensionalSample.Count; i++)
            {
                DL.Add(RankingValueX[i].Item2 - RankingValueY[i].Item2);
                tC += Math.Pow(DL.Last(), 2.0);
            }

            tC = 1.0 - 6.0 / (TwoDimensionalSample.Count * (Math.Pow(TwoDimensionalSample.Count, 2.0) - 1.0)) * tC;

            double ttCSignificant = Math.Abs(tC) == 1 ? 0 : tC * Math.Sqrt(TwoDimensionalSample.Count - 2) / (Math.Sqrt(1.0 - Math.Pow(tC, 2.0)));

            Protocol += $"\nРанговий коефіцієнт кореляції Спірмена : {tC}, так як квантиль t, при v = {v2} є {TQuantile}: " + (TQuantile > ttCSignificant ? $"{ttCSignificant} < {TQuantile}, отже коефіцієнт незначущій"
                : $"{ttCSignificant} > {TQuantile}, отже коефіцієнт значуща ");

            double tCInterval = Math.Sqrt((1.0 - Math.Pow(tC, 2.0)) / (TwoDimensionalSample.Count - 2));

            Protocol += $"\nІнтервальне оцінювання рангового коефіцієнта кореляції Спірмена - {{{tC - TQuantile * tCInterval}, {tC + TQuantile * tCInterval}}} ";

            double S = 0;

            for (int i = 0; i < TwoDimensionalSample.Count; i++)
            {
                for (int j = i + 1; j < TwoDimensionalSample.Count; j++)
                    if (RankingValueY[i].Item2 != RankingValueY[j].Item2)
                        S = RankingValueY[i].Item2 < RankingValueY[j].Item2 ? S + 1 : S - 1;
            }

            double tK = 2.0 * S / ((TwoDimensionalSample.Count * (TwoDimensionalSample.Count - 1.0)));

            double N = TwoDimensionalSample.Count;

            double ttKSignificant = 3.0 * tK * Math.Sqrt(N * (N - 1.0)) / (Math.Sqrt(2 * (2.0 * N + 5.0)));

            double tKInterval = Math.Sqrt((4.0 * N + 10.0) / (9.0 * (N * N - N)));

            Protocol += $"\nРанговий коефіцієнт кореляції Кенделла : {tK}, так як квантиль t, при v = {v2} є {TQuantile}: " + (TQuantile > ttKSignificant ? $"{ttKSignificant} < {TQuantile}, отже коефіцієнт незначущій"
                : $"{ttKSignificant} > {TQuantile}, отже коефіцієнт значуща ") +
                $"\nОтже інтервал оцінки кореляції Кенделла {{{tK - 1.9 * tKInterval} , {tK + 1.9 * tKInterval}}}";

            return Protocol;
        }
        public String build2X2Table()
        {
            String Protocol = "";

            double xAverage = Math.Round(xSample.Sample.Average(), 5);
            double yAverage = Math.Round(ySample.Sample.Average(), 5);

            double N00 = 0;
            double N01 = 0;

            double N10 = 0;
            double N11 = 0;

            foreach (var el in TwoDimensionalSample)
            {
                if (el.Item1 < xAverage && el.Item2 < yAverage) N00++;
                if (el.Item1 > xAverage && el.Item2 < yAverage) N01++;
                if (el.Item1 < xAverage && el.Item2 > yAverage) N10++;
                if (el.Item1 > xAverage && el.Item2 > yAverage) N11++;
            }

            double N0 = N00 + N01;
            double N1 = N11 + N10;
            double N = N0 + N1;

            double M0 = N00 + N10;
            double M1 = N01 + N11;

            Protocol = "Таблция сполучень 2*2\n" + $"Y\\X             <{xAverage}           |             >{xAverage}              |" +
                $"\n <{yAverage}              {N00}           |             {N01}              |            {N0}" +
                $"\n  >{yAverage}             {N10}           |             {N11}              |            {N1}" +
                $"\n                          {M0}            |             {M1}               |            {N}";

            double I = (N00 + N11 - N10 - N01) / (N00 + N11 + N10 + N01);

            Protocol += $"\nІндекс Фехнера має значення {I}, якщо I > 0 - додатня кореляція, при < 0 - від'ємна, при = 0 - відсутність зв'язку";

            var quantiles = new Quantiles();

            quantiles.XI2Quantiles();
            quantiles.TQuantiles();
            //Квантилі


            var Xi2Quantilies = quantiles.XI2_a0_2[1];

            int v2 = TwoDimensionalSample.Count - 1 <= 10 ? 10 : (TwoDimensionalSample.Count - 1 <= 30 ? 30 : 120);

            double TQuantile = quantiles.T_a0_05[v2];

            //

            //Коефіцієнт сполучень Ф

            double F = (N00 * N11 - N01 * N10) / Math.Sqrt(N0 * N1 * M0 * M1);

            double Xi2 = 0;
            if (TwoDimensionalSample.Count >= 40) Xi2 = TwoDimensionalSample.Count * Math.Pow(F, 2.0);
            else Xi2 = TwoDimensionalSample.Count * Math.Pow(N00 * N11 - N01 * N10 - 0.5, 2.0) / (N0 * N1 * M0 * M1);

            Protocol += $"\nКоефіцієнт сполучень Ф = {F}, при v = 1, та a = 0.2. " + (Xi2 >= Xi2Quantilies ? $"{Xi2} >= {Xi2Quantilies}, отже оцінка є значущую" :
                $"{Xi2} < {Xi2Quantilies}, отже оцінка не є значущою");
            //Коефіцієнт сполучень Юла

            double Q = (N00 * N11 - N01 * N10) / (N00 * N11 + N01 * N10);
            double Y = (Math.Sqrt(N00 * N11) - Math.Sqrt(N01 * N10)) / (Math.Sqrt(N00 * N11) + Math.Sqrt(N01 * N10));

            double SQ = 0.5 * (1.0 - Q * Q) * Math.Sqrt(1.0 / N00 + 1.0 / N01 + 1.0 / N10 + 1.0 / N11);
            double SY = 0.25 * (1.0 - Y * Y) * Math.Sqrt(1.0 / N00 + 1.0 / N01 + 1.0 / N10 + 1.0 / N11);

            double uQ = Math.Abs(Q / SQ);
            double uY = Math.Abs(Y / SY);

            Protocol += $"\nКоефіцієнт Юла Q = {Q}, Y = {Y} їх значущість" + (uQ < 1.96 ? $"\n{uQ} < 1.96, отже  H0:Q = 0" : $"\n{uQ} > 1.96, отже  H0:Q != 0") +
                (uY < 1.96 ? $"\n{uY} < 1.96, отже  H0:Q = 0" : $"\n{uY} >= 1.96, отже  H0:Q != 0");
            //

            return Protocol;
        }

        public String buildNXMTable()
        {
            String Protocol = "";

            List<double> ni = new List<double> { };
            List<double> mj = new List<double> { };

            double N = 0;

            for (int i = 0; i < ySample.ClassSize; i++)
            {
                if (i == 0)
                {
                    Protocol += $"|    y\\x    |";
                    for (int j = 0; j < xSample.ClassSize; j++)
                    {
                        Protocol += $"|   x{j}    |";
                    }
                    Protocol += "\n";
                }
                Protocol += $"|   y{i}    |";

                double mjValue = 0;
                for (int j = 0; j < xSample.ClassSize; j++)
                {
                    Protocol += $"|   {TwoDimensionalStatisticSample[i * (int)xSample.ClassSize + j].N}    |";

                    if (i == 0) ni.Add(TwoDimensionalStatisticSample[i * (int)xSample.ClassSize + j].N);
                    else ni[j] += TwoDimensionalStatisticSample[i * (int)xSample.ClassSize + j].N;

                    mjValue += TwoDimensionalStatisticSample[i * (int)xSample.ClassSize + j].N;
                }
                mj.Add(mjValue);
                Protocol += "\n";
            }
            //Критерій Xi2

            double NJ23 = TwoDimensionalStatisticSample.Sum(X => X.N);

            N = mj.Sum();

            double FR = ni.Sum();

            double Xi2 = 0;

            for(int i = 0; i < xSample.ClassSize; i++)
            {
                for(int j = 0; j < ySample.ClassSize; j++)
                {
                    double NIJ = (ni[i] * mj[j]) / N;
                    if(NIJ != 0)
                        Xi2 += (Math.Pow(TwoDimensionalStatisticSample[i * (int)ySample.ClassSize + j].N - NIJ, 2.0) / NIJ);
                }
            }

            //

            Quantiles quantiles = new Quantiles();

            quantiles.XI2Quantiles();

            int v1 = ((int)xSample.ClassSize - 1) * ((int)ySample.ClassSize - 1);

            v1 = v1 > 80 ? 80 : v1;

            var Xi2Quantilies = quantiles.XI2_a0_2[v1];

            Protocol += $"\nXi2 = {Xi2}" + (Xi2 > Xi2Quantilies ? $" так як {Xi2} > {Xi2Quantilies} - отже стохастичний звязок відсутній"
                : $" так як {Xi2} < {Xi2Quantilies} - отже наявний стохастичний звязок");

            //

            double C = Math.Sqrt(Xi2 / (N + Xi2));

            Protocol += $"\nКоефіцієнт сполучень Пірсона - {C}";

            //
            double P = 0;

            double Q = 0;

            double T1 = 0.5 * ni.Sum(X => X * (X - 1));
            double T2 = 0.5 * mj.Sum(X => X * (X - 1));

            for(int i = 0; i < xSample.ClassSize; i++)
            {
                for(int j = 0; j < ySample.ClassSize; j++)
                {
                    double f1 = 0, f2 = 0;

                    for(int k = i + 1; k < xSample.ClassSize; k++)
                    {
                        for(int l = j + 1; l < ySample.ClassSize; l++)
                        {
                            f1 += TwoDimensionalStatisticSample[k * (int)ySample.ClassSize + l].N;
                        }

                        for (int l = 0; l < j - 1; l++)
                        {
                            f2 += TwoDimensionalStatisticSample[k * (int)ySample.ClassSize + l].N;
                        }
                    }

                    f1 *= TwoDimensionalStatisticSample[i * (int)ySample.ClassSize + j].N;
                    f2 *= TwoDimensionalStatisticSample[i * (int)ySample.ClassSize + j].N;

                    P += f1;
                    Q += f2;
                }
            }

            if(xSample.ClassSize == ySample.ClassSize)
            {
                Protocol += $"\nn == m, отже Міра зв'язку Кендалла обчислюється. Не обчислюється статистика Стюарда";
                double tb = (P - Q) / (Math.Sqrt((0.5 * N * (N - 1.0) - T1) * (0.5 * N * (N - 1.0) - T2)));

                Protocol += $"\nМіра зв'язку Кендалла = {tb}";

            }
            else
            {
                Protocol += $"\nn != m, отже Міра зв'язку Кендалла не обчислюється. Обчислити статистику Стюарда";

                double ts = 2 * (P - Q) * Math.Min(xSample.ClassSize, ySample.ClassSize) / (N * N * (Math.Min(xSample.ClassSize, ySample.ClassSize) - 1));

                Protocol += $"\nСтатистика Стюарда = {ts}";
            }


            return Protocol;
        }
        //LinearRegresion
        public String MakeRegression()
        {
            String Protocol = "";

            double N = ValuePerX.Sum(X => X.Y.Count);

            List<double> SYXi = new List<double> { };

            double C = 1.0 + (1.0) / (3.0 * (ValuePerX.Count - 1.0)) * ValuePerX.Sum(X => 1.0 / X.Y.Count) - (1.0) / (N);

            for(int i = 0; i < ValuePerX.Count; i++)
            {
                double SValue = 0;

                for(int j = 0; j < ValuePerX[i].Y.Count; j++)
                {
                    SValue += Math.Pow(ValuePerX[i].Y[j] - ValuePerX[i].Y.Average(), 2.0);
                }
                SValue *= (1.0 / (ValuePerX[i].Y.Count - 1.0));
                if (ValuePerX[i].Y.Count - 1.0 > 0)
                    SYXi.Add(SValue);
                else
                    SYXi.Add(0);
            }
            double S = 0;

            for(int i = 0; i < ValuePerX.Count; i++)
            {
                S += (ValuePerX[i].Y.Count - 1.0) * SYXi[i];
            }
            S *= 1.0 / (N - ValuePerX.Count);

            double LambdaValue = 0;
            for (int i = 0; i < ValuePerX.Count; i++)
                LambdaValue += (ValuePerX[i].Y.Count * Math.Log(SYXi[i] / S));
            LambdaValue *= (-1.0 / C);

            Quantiles quantiles = new Quantiles();

            quantiles.XI2Quantiles();

            int v1 = ValuePerX.Count - 1;

            v1 = v1 > 80 ? 80 : v1;

            var Xi2Quantilies = quantiles.XI2_a0_2[v1];

            //
            double LambdaValue2 = 0;

            if(Xi2Quantilies > LambdaValue)
            {
                List<double> SYXi2 = new List<double> { };
                //Неизвестно как найти h.

                S = 0;

                for (int i = 0; i < ValuePerX.Count; i++)
                {
                    S += (ValuePerX[i].Y.Count - 1.0) * SYXi[i];
                }
                S *= 1.0 / (N - ValuePerX.Count);
            }
            Protocol += "\nПеревірка умов регресійного аналізу" +
                "\nВисувається гіпотезаH0:D{y/x1} = ... = D{y/xk}\n" +  $"/\\ - коефіцієнт є {LambdaValue}, а v = {v1}" + (Xi2Quantilies < LambdaValue ? ($"{Xi2Quantilies} < {LambdaValue} - головна гіпотеза відхилена, отже висувається гіпотеза відносно\n " 
                + "\nВисувається гіпотезаH0:D{y/x1}/h^2(x1) = ... = D{y/xk}/h^2(xk)\n" + ((true) ? "" : "")) 
                : $"\n{Xi2Quantilies} > {LambdaValue} - головну гіпотезу підтверджено. Отже реалізується параметричний регрісійний аналіз");

            if (Xi2Quantilies > LambdaValue)
            {
                double R = CorrelationCountRating();

                double R2 = Math.Pow(R, 2.0) * 100;

                Protocol += $"\nКоефіцієнт детермінації = {R2}";

                Protocol = buildLinearRegresionMNK(Protocol);

                Protocol = buildLinearRegresionTaylor(Protocol);
            }

            return Protocol;

        }
        public String buildLinearRegresionMNK(String Protocol)
        {
            double R = CorrelationCountRating();

            double b = R * ySample.QuantitiveCharactacteristics.S_Variance_unShifted / xSample.QuantitiveCharactacteristics.S_Variance_unShifted;
            double a = ySample.QuantitiveCharactacteristics.AritmeitcMean - b * ySample.QuantitiveCharactacteristics.AritmeitcMean;
            LinearRegresionMNK = new ObservableCollection<XYData> { };
            for(double i = xSample.Sample.Min(); i <= xSample.Sample.Max(); i += xSample.StepSize)
            {
                LinearRegresionMNK.Add(new XYData { X = i, Y = a + b * i});
            }

            double SCommon2 = 0;

            for(int i = 0; i < TwoDimensionalSample.Count; i++)
            {
                SCommon2 += Math.Pow(TwoDimensionalSample[i].Item2 - (a + b * TwoDimensionalSample[i].Item1), 2.0);
            }

            Protocol += $"\nОцінка МНК - на графіку чорна лінія. S^2 загальне - {SCommon2}";

            return Protocol;
        }

        public String buildLinearRegresionTaylor(String Protocol)
        {
            double R = CorrelationCountRating();

            List<double> MEDB = new List<double> { };
            List<double> MEDA = new List<double> { };

            for(int i = 0; i < TwoDimensionalSample.Count; i++)
            {
                for(int j = i; j < TwoDimensionalSample.Count; j++)
                {
                    MEDB.Add((TwoDimensionalSample[j].Item2 - TwoDimensionalSample[i].Item2) / (TwoDimensionalSample[j].Item1 - TwoDimensionalSample[i].Item1));
                }
            }

            double b = QuantitiveCharacteristicsService.MEDFind(MEDB);
            for(int i = 0; i < TwoDimensionalSample.Count; i++)
            {
                MEDA.Add(TwoDimensionalSample[i].Item2 - b * TwoDimensionalSample[i].Item1);
            }
            double a = QuantitiveCharacteristicsService.MEDFind(MEDA);
            LinearRegresionTaylor = new ObservableCollection<XYData> { };
            for (double i = xSample.Sample.Min(); i <= xSample.Sample.Max(); i += xSample.StepSize)
            {
                LinearRegresionTaylor.Add(new XYData { X = i, Y = a + b * i });
            }

            double SCommon2 = 0;

            for (int i = 0; i < TwoDimensionalSample.Count; i++)
            {
                SCommon2 += Math.Pow(TwoDimensionalSample[i].Item2 - (a + b * TwoDimensionalSample[i].Item1), 2.0);
            }

            Protocol += $"\nОцінка Тейлора - на графіку сіра лінія. S^2 загальне - {SCommon2}";

            return Protocol;
        }
        private List<double> RankCount(List<double> newSample)
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
        private List<(double, double)> RankForEveryValue(List<double> Rank, List<double> FullSample)
        {
            List<(double, double)> answerValue = new List<(double, double)> { };
            for (int i = 0; i < FullSample.Count; i++)
            {
                answerValue.Add((FullSample[i], Rank[i]));
            }
            return answerValue;
        }
    }
}
