using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Quau.Models.Base;
using Quau.Models.DimentionalModel.Two;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private ObservableCollection<(double, double)> _TwoDimensionalSample;

        public ObservableCollection<(double, double)> TwoDimensionalSample { get => _TwoDimensionalSample; set => Set(ref _TwoDimensionalSample, value); }
        #endregion

        #region Темп значение только для графика(heatmap)

        private SeriesCollection _Values;

        public SeriesCollection Values { get => _Values; set => Set(ref _Values, value); }

        #endregion

        #region 
        private ObservableCollection<TwoDimentionalSample> _TwoDimensionalStatisticSample;

        public ObservableCollection<TwoDimentionalSample> TwoDimensionalStatisticSample { get => _TwoDimensionalStatisticSample; set => Set(ref _TwoDimensionalStatisticSample, value); }
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
            if (TwoDimensionalSample != null && TwoDimensionalSample.Count > 0)
            {
                var xSample = new ObservableCollection<double> { };
                var ySample = new ObservableCollection<double> { };

                foreach (var el in TwoDimensionalSample)
                {
                    xSample.Add(el.Item1);
                    ySample.Add(el.Item2);
                }

                this.xSample = new StatisticSample { Sample = xSample };

                this.ySample = new StatisticSample { Sample = ySample };
                return true;
            }
            return false;
        }

        public void SetClassSize(double xClass, double yClass) {
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

            for (int i = 0; i < xSample.SampleDivisionINClass.Count; i++)
            {
                var elXLeft = this.xSample.SampleDivisionINClass[i];
                for (int i1 = 0; i1 < ySample.SampleDivisionINClass.Count; i1++)
                {
                    var elYLeft = this.ySample.SampleDivisionINClass[i1];

                    sample.Add(new TwoDimentionalSample { X = this.xSample.SampleDivisionINClass.First().SampleDivisionData + Step.Item1 * i, Y = this.ySample.SampleDivisionINClass.First().SampleDivisionData + Step.Item2 * i1 });
                    for (int i2 = 0; i2 < TwoDimensionalSample.Count; i2++)
                    {
                        var elSample = TwoDimensionalSample[i2];
                        if ((elSample.Item1 >= elXLeft.SampleDivisionData && elSample.Item1 < elXLeft.SampleDivisionData + Step.Item1) && (elSample.Item2 >= elYLeft.SampleDivisionData && elSample.Item2 < elYLeft.SampleDivisionData + Step.Item1))
                        {
                            sample[sample.Count - 1].N++;
                        }
                    }
                }
            }
            this.TwoDimensionalStatisticSample = sample;

            var values = new SeriesCollection  { };

            var SeriesValues = new ChartValues<ScatterPoint> { };

            foreach (var el in sample)
            {
                SeriesValues.Add(new ScatterPoint(el.Y, el.X, el.N / TwoDimensionalSample.Count));
            }
            values = new SeriesCollection { new ScatterSeries { Values = SeriesValues, MinPointShapeDiameter = 15,
                    MaxPointShapeDiameter = 45 } };
            this.Values = values;

            return true;
        } 

    }
}
