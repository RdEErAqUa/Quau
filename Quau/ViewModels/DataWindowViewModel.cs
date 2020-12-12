using Quau.Data.Modeling;
using Quau.Infrastructure.Commands;
using Quau.Models;
using Quau.Models.ModelingSample;
using Quau.Services.StatisticOperation;
using Quau.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quau.ViewModels
{
    internal class DataWindowViewModel : ViewModel
    {
        public MainWindowViewModel MainModel { get; }

        /* ------------------------------------------------------------------------- */

        #region SelectedSampleData : StatisticSample - данные о выбраной выборке

        private StatisticSample _SelectedSampleData;

        public StatisticSample SelectedSampleData { get => _SelectedSampleData; set => Set(ref _SelectedSampleData, value); }

        #endregion

        #region SampleData : ICollection<StatisticSample> - данные о выборке

        private ICollection<StatisticSample> _SampleData;

        public ICollection<StatisticSample> SampleData { get => _SampleData; set { Set(ref _SampleData, value);} }

        #endregion

        #region lambdaValue : double - значение ламбды для моделирования экспоненциальной выборки

        private double _lambdaValue;
        public double lambdaValue { get => _lambdaValue; set => Set(ref _lambdaValue, Math.Round(value, 4)); }

        #endregion

        #region ModelingSamples : ModelingSample - смоделировать выборку
        private ModelingSample _ModelingSamples;
        public ModelingSample ModelingSamples { get => _ModelingSamples; set => Set(ref _ModelingSamples, value); }

        #endregion

        #region RecordValue : String - протокол

        private String _RecordValue;

        public String RecordValue { get => _RecordValue; set => Set(ref _RecordValue, value); }

        #endregion

        #region samples : ObservableCollection<TSample> - представлення т-статистики в таблицы

        private ObservableCollection<TSample> _TSamples;

        public ObservableCollection<TSample> TSamples { get => _TSamples; set => Set(ref _TSamples, value); }

        #endregion

        /* ------------------------------------------------------------------------- */
        #region  TTestFind - Однородность выборки зависимых
        public ICommand TTestFind { get; }

        private bool CanTTestFindExecute(object p)
        {
            return true;
        }

        private void OnTTestFindExecuted(object p)
        {
            TTest test = MainModel.TTestValue;

            test.TValue = (test.value - test.newValue) / (test.delta2Value) * Math.Sqrt(test.NValue);

            MainModel.TTestValue = test;
        }
        #endregion

        #region ModelingExponentialSample - Смоделировать выборку из значения lambda

        public ICommand ModelingExponentialSample { get; }

        private bool CanModelingExponentialSampleExecute(object p)
        {
            return true;
        }

        private void OnModelingExponentialSampleExecuted(object p)
        {
            ModelingSamples = new ModelingSample();
            TSamples = new ObservableCollection<TSample>();
            ModelingSamples.statisticSamples_10 = ModelingElement.modelingElement(10, lambdaValue, 400, ModelingSamples.statisticSamples_10, ModelingSamples.tValue_10);
            ModelingSamples.statisticSamples_40 = ModelingElement.modelingElement(40, lambdaValue, 400, ModelingSamples.statisticSamples_40, ModelingSamples.tValue_40);
            ModelingSamples.statisticSamples_100 = ModelingElement.modelingElement(100, lambdaValue, 400, ModelingSamples.statisticSamples_100, ModelingSamples.tValue_100);
            ModelingSamples.statisticSamples_400 = ModelingElement.modelingElement(400, lambdaValue, 400, ModelingSamples.statisticSamples_400, ModelingSamples.tValue_400);
            ModelingSamples.statisticSamples_1000 = ModelingElement.modelingElement(1000, lambdaValue, 400, ModelingSamples.statisticSamples_1000, ModelingSamples.tValue_1000);

            this.MainModel.SampleData = new List<StatisticSample> { new StatisticSample { Sample = ModelingSamples.statisticSamples_10.First().Sample, fileName = "Samples10 First" } };
            this.MainModel.SampleData = new List<StatisticSample> { new StatisticSample { Sample = ModelingSamples.statisticSamples_40.First().Sample, fileName = "Samples40 First" } };
            this.MainModel.SampleData = new List<StatisticSample> { new StatisticSample { Sample = ModelingSamples.statisticSamples_100.First().Sample, fileName = "Samples100 First" } };
            this.MainModel.SampleData = new List<StatisticSample> { new StatisticSample { Sample = ModelingSamples.statisticSamples_400.First().Sample, fileName = "Samples400 First" } };
            this.MainModel.SampleData = new List<StatisticSample> { new StatisticSample { Sample = ModelingSamples.statisticSamples_1000.First().Sample, fileName = "Samples1000 First" } };

            double deltaT10 = Math.Round(Math.Abs(ModelingSamples.tValue_10.Sum() / 400.0), 4);
            double deltaT40 = Math.Round(Math.Abs(ModelingSamples.tValue_40.Sum() / 400.0), 4);
            double deltaT100 = Math.Round(Math.Abs(ModelingSamples.tValue_100.Sum() / 400.0), 4);
            double deltaT400 = Math.Round(Math.Abs(ModelingSamples.tValue_400.Sum() / 400.0), 4);
            double deltaT1000 = Math.Round(Math.Abs(ModelingSamples.tValue_1000.Sum() / 400.0), 4);

            double deltaRootT10 = Math.Round(deltaT10 / Math.Sqrt(10), 4);
            double deltaRootT40 = Math.Round(deltaT40 / Math.Sqrt(40), 4);
            double deltaRootT100 = Math.Round(deltaT100 / Math.Sqrt(100), 4);
            double deltaRootT400 = Math.Round(deltaT400 / Math.Sqrt(400), 4);
            double deltaRootT1000 = Math.Round(deltaT1000 / Math.Sqrt(1000), 4);

            TSamples.Add(new TSample(10, deltaT10, deltaRootT10, 0.5, 0.7, deltaT10 < 0.7));
            TSamples.Add(new TSample(40, deltaT40, deltaRootT40, 0.5, 0.681, deltaT40 < 0.681));
            TSamples.Add(new TSample(100, deltaT100, deltaRootT100, 0.25, 1.16, deltaT100 < 1.16));
            TSamples.Add(new TSample(400, deltaT400, deltaRootT400, 0.1, 1.64, deltaT400 < 1.64));
            TSamples.Add(new TSample(1000, deltaT1000, deltaRootT1000, 0.05, 1.96, deltaT1000 < 1.96));

            string T10 = deltaT10 > 0.7 ? " > 0.7. Гіпотезу спростовано, змодельованні виборки в середньому - статистично відмінні" 
                : " < 0.7. Гіпотеза підтвердженна, змодельованні виборки в середньому - статистично невідмінні";
            string T40 = deltaT40 > 0.681 ? " > 0.681. Гіпотезу спростовано, змодельованні виборки в середньому - статистично відмінні" 
                : " < 0.681. Гіпотеза підтвердженна, змодельованні виборки в середньому - статистично невідмінні";
            string T100 = deltaT100 > 1.16 ? " > 1.16. Гіпотезу спростовано, змодельованні виборки в середньому - статистично відмінні" 
                : " < 1.16. Гіпотеза підтвердженна, змодельованні виборки в середньому - статистично невідмінні";
            string T400 = deltaT400 > 1.64 ? " > 1.64. Гіпотезу спростовано, змодельованні виборки в середньому - статистично відмінні" 
                : " < 1.64. Гіпотеза підтвердженна, змодельованні виборки в середньому - статистично невідмінні";
            string T1000 = deltaT1000 > 1.96 ? " > 1.96. Гіпотезу спростовано, змодельованні виборки в середньому - статистично відмінні" 
                : " < 1.96. Гіпотеза підтвердженна, змодельованні виборки в середньому - статистично невідмінні";

            RecordValue = 
                $"Головна гіпотеза - H0 : ^O = ^-O. Для кожного тестування визначається власне значення a(в залежності від кількості даних)" +
                $"\nПорівняння t для 10, при a = 0.5 : {deltaT10}" + T10 +
                $"\nПорівняння t для 40, при a = 0.5 : {deltaT40}" + T40 +
                $"\nПорівняння t для 100, при a = 0.25 : {deltaT100}" + T100 +
                $"\nПорівняння t для 400, при a = 0.1 : {deltaT400}" + T400 +
                $"\nПорівняння t для 1000, при a = 0.05 : {deltaT1000}" + T1000;
        }

        #endregion


        public DataWindowViewModel(MainWindowViewModel MainModel)
        {
            TTestFind = new LambdaCommand(OnTTestFindExecuted, CanTTestFindExecute);

            ModelingExponentialSample = new LambdaCommand(OnModelingExponentialSampleExecuted, CanModelingExponentialSampleExecute);

            this.MainModel = MainModel;
        }
    }
}
