using Quau.Data.AbbeTest;
using Quau.Data.ConsentTest;
using Quau.Data.UniformySamples;
using Quau.Infrastructure.Commands;
using Quau.Models;
using Quau.Models.DistributionConsent;
using Quau.Services;
using Quau.Services.FileOpenLoad;
using Quau.Services.StatisticOperation;
using Quau.Services.StatisticOperation.AnomalyData;
using Quau.Services.StatisticOperation.DistributionCalculate;
using Quau.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Quau.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        /* ----------------------------------ViewModel--------------------------------------- */

        public GraphFunctionWindowViewModel GraphFunctionWindowModel { get; }

        public DataWindowViewModel DataWindowModel { get; }

        /* ------------------------------------------------------------------------- */

        #region Properties

        #region SelectedSampleData : StatisticSample - данные о выбраной выборке

        private StatisticSample _SelectedSampleData;

        public StatisticSample SelectedSampleData
        {
            get => _SelectedSampleData; set
            {
                Set(ref _SelectedSampleData, value, true);

                DataWindowModel.SelectedSampleData = SelectedSampleData;
                GraphFunctionWindowModel.SelectedSampleData = SelectedSampleData;
            }
        }

        #endregion

        #region SampleFilePath : string - путь к файлу с значением выборки

        public string SampleFilePath
        {
            set
            {
                SampleData = new List<StatisticSample> { new StatisticSample { Sample = DataConvertor.DataConvertorStrToDouble(ReadDataService.ReadData(value)) } };
            }
        }

        #endregion

        #region SampleData : ICollection<StatisticSample> - данные о выборке

        private ICollection<StatisticSample> _SampleData;

        public ICollection<StatisticSample> SampleData
        {
            get => _SampleData; set
            {
                var tempCollection = new List<StatisticSample> { };
                if (_SampleData != null)
                {
                    foreach (var el in _SampleData)
                    {
                        var tempSample = el;
                        tempCollection.Add(tempSample);
                    }
                }

                if (value.Count > 0)
                {
                    StatisticOperationLauncher.StartStatisticOperation(value.Last());
                    QuantitiveCharacteristicsService.QuantitiveCharacteristics(value.Last());
                    tempCollection.Add(value.Last());

                    Set(ref _SampleData, tempCollection);

                    // Временное хранилище для Скопированой выбраной выборки

                    DataWindowModel.SampleData = _SampleData;
                    GraphFunctionWindowModel.SampleData = _SampleData;
                }
                //Выбор выборки! Убрать отсюда в будущем и реализовать выбор выборки!

                //DataWindowModel.SelectedSampleData = _SampleData?.Last();
                //GraphFunctionWindowModel.SelectedSampleData = _SampleData?.Last();
            }
        }

        #endregion

        #region SampleDataCopy : StatisticSample - сохраненные данные о выборке(для возвращение SampleData начальной позиции

        private StatisticSample _SampleDataCopy;

        public StatisticSample SampleDataCopy { get => _SampleDataCopy; set => Set(ref _SampleDataCopy, value); }

        #endregion

        #region RecordValue : String - протокол

        private String _RecordValue;

        public String RecordValue { get => _RecordValue; set => Set(ref _RecordValue, value); }

        #endregion
        #region TTestValue : TTest - протокол

        private TTest _TTestValue;

        public TTest TTestValue { get => _TTestValue; set => Set(ref _TTestValue, value, true); }

        #endregion

        #endregion

        /* ------------------------------------------------------------------------- */
        #region Command

        #region GetFileName - Выбор файла, путь к которому записывается в SampleFilePath
        public ICommand GetFileName { get; }

        private bool CanGetFileNameExecute(object p)
        {
            return true;
        }

        private void OnGetFileNameExecuted(object p)
        {
            var zOpenFileDialog = new SaveDialogSerivce();

            if (zOpenFileDialog.OpenFileDialog()) SampleFilePath = zOpenFileDialog.FilePath;
        }
        #endregion

        #region  DistributionStart - Распределение начать
        public ICommand DistributionStart { get; }

        private bool CanDistributionStartExecute(object p)
        {
            return true;
        }

        private void OnDistributionStartExecuted(object p)
        {
            if(SampleData == null) { 
            }
            else if ((string)p == "0") {
                var test = DistributionService.NormalDistribution(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                var test2 = DistributionService.NormalDistributionEmpirical(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                SelectedSampleData = null;
                SelectedSampleData = test;
            }
            else if ((string)p == "1")
            {
                var test = DistributionService.ExponentialDistribution(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                var test2 = DistributionService.ExponentialDistributionEmpirical(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                SelectedSampleData = null;
                SelectedSampleData = test;
            }
            else if ((string)p == "2")
            {
                var test = DistributionService.EvenDistribution(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                var test2 = DistributionService.EvenDistributionEmpirical(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                SelectedSampleData = null;
                SelectedSampleData = test;
            }
            else if ((string)p == "3")
            {
                var test = DistributionService.VWeibullDistribution(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                var test2 = DistributionService.VWeibullDistributionEmpirical(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                SelectedSampleData = null;
                SelectedSampleData = test;
            }
            else if ((string)p == "4")
            {
                var test = DistributionService.ArcSinDistribution(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                var test2 = DistributionService.ArcSinDistributionEmpirical(SelectedSampleData.SampleDataRanking, SelectedSampleData);

                SelectedSampleData = null;
                SelectedSampleData = test;
            }
            else if ((string)p == "5")
            {
                SelectedSampleData = SampleDataCopy;
            }
            double Kolmogorov = KolmogorovTest.KolmogorovTest_Invoke(SelectedSampleData);

            RecordValue = "";

            RecordValue += $"Крітерій Колмогорова                - {Kolmogorov}\n";
            double Pearson = PearsonTest.PearsonTest_Invoke(SelectedSampleData);
            RecordValue += $"Крітерій Пірсона                - {Pearson}\n";
            SelectedSampleData.DistributionConsentTests = new List<DistributionConsentTest> { };

            SelectedSampleData.DistributionConsentTests.Add(new DistributionConsentTest { KolmogorovTest = Kolmogorov, PirsonTest = Pearson });
        }
        #endregion

        #region  AnomalyDataRemove - Извлечение аномальных данных
        public ICommand AnomalyDataRemove { get; }

        private bool CanAnomalyDataRemoveExecute(object p)
        {
            return true;
        }

        private void OnAnomalyDataRemoveExecuted(object p) 
        {
            if (SampleData == null)
            {
            }
            else if ((string)p == "0")
            {
                var test = RemoveAnomalyData.Move(SelectedSampleData);
                SampleData = new List<StatisticSample> { SelectedSampleData };
            }
            else if ((string)p == "1")
            {
                var test = RemoveAnomalyData.Standartization(SelectedSampleData);
                SampleData = new List<StatisticSample> { SelectedSampleData };
            }
            else if ((string)p == "2")
            {
                var test = RemoveAnomalyData.Log(SelectedSampleData);
                SampleData = new List<StatisticSample> { SelectedSampleData };
            }
        }
        #endregion

        #region  CheckUniformy - Однородность выборки независимых
        public ICommand CheckUniformy { get; }

        private bool CanCheckUniformyExecute(object p)
        {
            return true;
        }

        private void OnCheckUniformyExecuted(object p)
        {
            var items = (System.Collections.ICollection)p;
            int sizeOfValue = items.Count;

            ICollection<StatisticSample> valuesSample = new List<StatisticSample> { };

            foreach (var el in items)
                valuesSample.Add((StatisticSample)el);

            RecordValue = "";

            if (sizeOfValue == 2)
            {
                double a = 0.3;

                RecordValue += $"Кількість N1 - {valuesSample.ElementAt(0).Sample.Count}, N2 - {valuesSample.ElementAt(1).Sample.Count}, де a - {a}\n";
                //Do that
                double uniformyAverage = UniformySamples.uniformyAverage(valuesSample, false);

                RecordValue += $"Збіг середніх для вибірок  -         {uniformyAverage}, де v - {valuesSample.ElementAt(0).Sample.Count + - valuesSample.ElementAt(1).Sample.Count - 2}\n";

                double uniformyVariances = UniformySamples.uniformyVariances(valuesSample);

                RecordValue += $"Збіг дисперсій для вибірок -         {uniformyVariances}, де v1 - {valuesSample.ElementAt(0).Sample.Count - 1}, v2 - {valuesSample.ElementAt(1).Sample.Count - 1}\n";
                double uniformyWilkson = UniformySamples.uniformyWilkson(valuesSample);

                RecordValue += $"Критерій Вілксона          -         {uniformyWilkson}, при a - {a}\n";
                double uniformyMannaWhitney = UniformySamples.uniformyMannaWhitney(valuesSample);

                RecordValue += $"Критерій Манна-Уїзні       -         {uniformyMannaWhitney}, при a - {a}\n";
                double uniformyMiddleRanking = UniformySamples.uniformyMiddleRanking(valuesSample);

                RecordValue += $"Різниця рангів             -         {uniformyMiddleRanking}, при a - {a}\n";
                double uniformyKolmogorovaSmirnova = UniformySamples.uniformyKolmogorovaSmirnova(valuesSample);

                RecordValue += $"Колмогорова-Смірнова       -         {uniformyKolmogorovaSmirnova}\n";
                double k = 0;
            }
            else if (sizeOfValue > 2)
            {
                //Do if
                double a = 0.3;
                int i = 0;
                RecordValue += "Кількість ";
                foreach (var el in valuesSample)
                    RecordValue += $"N{i++} - {el.Sample.Count}, ";
                RecordValue += $"де a - { a}\n";

                double uniformyVariances = UniformySamples.uniformyVariances(valuesSample);

                RecordValue += $"Збіг дисперсій для вибірок -         {uniformyVariances}, де v1 - {valuesSample.Count}\n";
                double uniformyAnalysisVariance = UniformySamples.uniformyAnalysisVariance(valuesSample);

                RecordValue += $"Одноф-дисперсійний аналіз  -         {uniformyAnalysisVariance}\n";
                double uniformyHTest = UniformySamples.uniformyHTest(valuesSample);

                RecordValue += $"H - test                   -         {uniformyHTest}\n";
                double k = 0;
            }
            else
            {

            }

        }
        #endregion

        #region  CheckUniformyDependent - Однородность выборки зависимых
        public ICommand CheckUniformyDependent { get; }

        private bool CanCheckUniformyDependentExecute(object p)
        {
            return true;
        }

        private void OnCheckUniformyDependentExecuted(object p)
        {
            var items = (System.Collections.ICollection)p;
            int sizeOfValue = items.Count;

            ICollection<StatisticSample> valuesSample = new List<StatisticSample> { };

            foreach (var el in items)
                valuesSample.Add((StatisticSample)el);
            RecordValue = "";
            if (sizeOfValue == 2)
            {
                try
                {
                    double a = 0.3;

                    RecordValue += $"Кількість N1 - {valuesSample.ElementAt(0).Sample.Count}, N2 - {valuesSample.ElementAt(1).Sample.Count}, де a - {a}\n";
                    //Do that
                    double uniformyAverage = UniformySamples.uniformyAverage(valuesSample, false);

                    RecordValue += $"Збіг середніх для вибірок  -         {uniformyAverage}, де v - {valuesSample.ElementAt(0).Sample.Count + -valuesSample.ElementAt(1).Sample.Count - 2}\n";
                    double uniformyWilkson = UniformySamples.uniformyWilkson(valuesSample);

                    RecordValue += $"Критерій Вілксона          -         {uniformyWilkson}, при a - {a}\n";
                    double uniformyMannaWhitney = UniformySamples.uniformyMannaWhitney(valuesSample);

                    RecordValue += $"Критерій Манна-Уїзні       -         {uniformyMannaWhitney}, при a - {a}\n";
                    double uniformyMiddleRanking = UniformySamples.uniformyMiddleRanking(valuesSample);

                    RecordValue += $"Різниця рангів             -         {uniformyMiddleRanking}, при a - {a}\n";
                    double uniformyKolmogorovaSmirnova = UniformySamples.uniformyKolmogorovaSmirnova(valuesSample);

                    RecordValue += $"Колмогорова-Смірнова       -         {uniformyKolmogorovaSmirnova}\n";
                }
                catch (Exception e)
                {
                    RecordValue = $"Хибний вибір знаходження оцінки однорідності вибірки {e.Message}";
                }
            }
            else if (sizeOfValue > 2)
            {
                //Do if
                try
                {
                    double a = 0.3;
                    int i = 0;
                    RecordValue += "Кількість ";
                    foreach (var el in valuesSample)
                        RecordValue += $"N{i++} - {el.Sample.Count}, ";
                    RecordValue += $"де a - { a}\n";

                    double uniformyVariances = UniformySamples.uniformyVariances(valuesSample);

                    RecordValue += $"Збіг дисперсій для вибірок -         {uniformyVariances}, де v1 - {valuesSample.Count}\n";
                }
                catch (Exception e)
                {
                    RecordValue = $"Хибний вибір знаходження оцінки однорідності вибірки {e.Message}";
                }
            }
            else
            {

            }

        }
        #endregion

        #region  AbbeTest - Однородность выборки зависимых
        public ICommand AbbeTestRun { get; }

        private bool CanAbbeTestRunExecute(object p)
        {
            return true;
        }

        private void OnAbbeTestRunExecuted(object p)
        {
            if(SelectedSampleData != null)
            {
                double u = AbbeTest.AbbeTestRun(SelectedSampleData);

                RecordValue += $"Abbe тест - {u}";
            }
        }
        #endregion

        #endregion
        public MainWindowViewModel()
        {
            GraphFunctionWindowModel = new GraphFunctionWindowViewModel(this);

            DataWindowModel = new DataWindowViewModel(this);

            GetFileName = new LambdaCommand(OnGetFileNameExecuted, CanGetFileNameExecute);

            DistributionStart = new LambdaCommand(OnDistributionStartExecuted, CanDistributionStartExecute);

            AnomalyDataRemove = new LambdaCommand(OnAnomalyDataRemoveExecuted, CanAnomalyDataRemoveExecute);

            CheckUniformy = new LambdaCommand(OnCheckUniformyExecuted, CanCheckUniformyExecute);

            CheckUniformyDependent = new LambdaCommand(OnCheckUniformyDependentExecuted, CanCheckUniformyDependentExecute);

            AbbeTestRun = new LambdaCommand(OnAbbeTestRunExecuted, CanAbbeTestRunExecute);

            TTestValue = new TTest();

            RecordValue = "";
            //
            SampleData = new List<StatisticSample> { };
        }
    }
}
