using Quau.Data.AbbeTest;
using Quau.Data.ConsentTest;
using Quau.Data.Modeling;
using Quau.Data.UniformySamples;
using Quau.Infrastructure.Commands;
using Quau.Models;
using Quau.Models.DistributionConsent;
using Quau.Models.ModelingSample;
using Quau.Services;
using Quau.Services.FileOpenLoad;
using Quau.Services.StatisticOperation;
using Quau.Services.StatisticOperation.AnomalyData;
using Quau.Services.StatisticOperation.DistributionCalculate;
using Quau.ViewModels.Base;
using Quau.ViewModels.OptionsData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                Set(ref _SelectedSampleData, value);

                DataWindowModel.SelectedSampleData = SelectedSampleData;
                GraphFunctionWindowModel.SelectedSampleData = SelectedSampleData;
            }
        }

        #endregion

        #region ProtocolTwoDimentional : String - протокол двомерной выборки

        private String _ProtocolTwoDimentional;

        public String ProtocolTwoDimentional { get => _ProtocolTwoDimentional; set => Set(ref _ProtocolTwoDimentional, value); }

        #endregion

        #region ProtocolTwoDimentional2X2 : String - протокол двомерной выборки

        private String _ProtocolTwoDimentional2X2;

        public String ProtocolTwoDimentional2X2 { get => _ProtocolTwoDimentional2X2; set => Set(ref _ProtocolTwoDimentional2X2, value); }

        #endregion

        #region ProtocolTwoDimentionalNXM : String - протокол двомерной выборки

        private String _ProtocolTwoDimentionalNXM;

        public String ProtocolTwoDimentionalNXM { get => _ProtocolTwoDimentionalNXM; set => Set(ref _ProtocolTwoDimentionalNXM, value); }

        #endregion

        #region ProtocolRegretion : String - протокол двомерной выборки

        private String _ProtocolRegretion;

        public String ProtocolRegretion { get => _ProtocolRegretion; set => Set(ref _ProtocolRegretion, value); }

        #endregion

        #region SampleFilePath : string - путь к файлу с значением выборки

        public string SampleFilePath
        {
            set
            {
                var deb = DataConvertor.DataConvertorStrToDouble(ReadDataService.ReadData(value)).ToList();

                var temp = new List<double> { };

                foreach(var el in deb)
                {
                    temp.Add(el);
                }


                SampleData = new List<StatisticSample> { new StatisticSample { Sample = new ObservableCollection<double>(deb), fileName = value.Split('\\').Last() } };

                if(temp.Count % 2 == 0)
                {
                    ObservableCollection<(double, double)> sample = new ObservableCollection<(double, double)> { };

                    for(int i = 0; i < temp.Count - 1; i += 2)
                    {
                        sample.Add((temp[i], temp[i + 1]));
                    }

                    TwoDimentional = new TwoDimentionalStatisticSample { TwoDimensionalSample = sample};

                    TwoDimentional.SeparateInTwoSample();

                    TwoDimentional.SetClassSize(13, 13);

                    TwoDimentional.SetTwoDimentionalSample();

                    TwoDimentional.SetHistogramSample();
                    ProtocolRegretion = TwoDimentional.MakeRegression();

                    ProtocolTwoDimentional = TwoDimentional.buildProtocol();
                    ProtocolTwoDimentional2X2 = TwoDimentional.build2X2Table();
                    ProtocolTwoDimentionalNXM = TwoDimentional.buildNXMTable();
                }
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
                    tempCollection.Add(value.Last());

                    Set(ref _SampleData, tempCollection);

                    DataWindowModel.SampleData = SampleData;
                    GraphFunctionWindowModel.SampleData = SampleData;
                }
            }
        }

        #endregion

        #region RecordValue : String - протокол

        private String _RecordValue;

        public String RecordValue { get => _RecordValue; set => Set(ref _RecordValue, value); }

        #endregion

        #region TTestValue : TTest - протокол

        private TTest _TTestValue;

        public TTest TTestValue { get => _TTestValue; set => Set(ref _TTestValue, value); }

        #endregion

        #region TwoDimentionalSample : TwoDimentionalStatisticSample - выборка двойного измерения

        private TwoDimentionalStatisticSample _TwoDimentional;

        public TwoDimentionalStatisticSample TwoDimentional { get => _TwoDimentional; set => Set(ref _TwoDimentional, value); }

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
            SelectedSampleData.DistributionProtocol = "";
            switch (Int32.Parse((string)p))
            {
                case 0:
                    DistributionService.NormalDistribution(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                    DistributionService.NormalDistributionEmpirical(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                    SelectedSampleData.DistributionProtocol += DistributionService.NormalDistributionInterval(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                    break;
                case 1:
                    DistributionService.ExponentialDistribution(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                    DistributionService.ExponentialDistributionEmpirical(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                    SelectedSampleData.DistributionProtocol += DistributionService.ExponentialDistributionInterval(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                    break;
                case 2:
                    DistributionService.EvenDistribution(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                    DistributionService.EvenDistributionEmpirical(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                    SelectedSampleData.DistributionProtocol += DistributionService.EvenDistributionInterval(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                    break;
                case 3:
                    DistributionService.VWeibullDistribution(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                    DistributionService.VWeibullDistributionEmpirical(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                    break;
                case 4:
                    DistributionService.ArcSinDistribution(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                    DistributionService.ArcSinDistributionEmpirical(SelectedSampleData.SampleDataRanking, SelectedSampleData);
                    break;
                default:
                    return;
            }  //Побудова розподілу за вибраним критерієм(експоненціальній, нормальний, та інші)
            double Kolmogorov = KolmogorovTest.KolmogorovTest_Invoke(SelectedSampleData);  //Обрахування оцінки Колмогорова
            double Pearson = PearsonTest.PearsonTest_Invoke(SelectedSampleData); //Обрахування оцінки Пірсона

            double A = SelectedSampleData.Sample.Count < 60 ? 0.5 : 0.2;

            String KolmogorovRecord = A < Kolmogorov ? $" > {A}, отже емпірична функція невідповідає теоретичному": $" < {A}, отже емпірична функція відповідаю теоретичному";

            Quantiles quantiles = new Quantiles();

            quantiles.XI2Quantiles();

            String PearsonValue = A == 0.5 ?
                (Math.Abs(Pearson) < quantiles.XI2_a0_5[(int)SelectedSampleData.ClassSize - 1] ? 
                $" < {quantiles.XI2_a0_5[(int)SelectedSampleData.ClassSize - 1]}, отже емпірична функція відповідає теоретичному" : $" " +
                $"> {quantiles.XI2_a0_5[(int)SelectedSampleData.ClassSize - 1]}, отже емпірична функція невідповідає теоретичному") :
                (Math.Abs(Pearson) < quantiles.XI2_a0_2[(int)SelectedSampleData.ClassSize - 1] ?
                $" < {quantiles.XI2_a0_2[(int)SelectedSampleData.ClassSize - 1]}, отже емпірична функція відповідає теоретичному" : $" " +
                $"> {quantiles.XI2_a0_2[(int)SelectedSampleData.ClassSize - 1]}, отже емпірична функція невідповідає теоретичному");

            SelectedSampleData.DistributionProtocol += $"\nПри a - {A}, так як кількість даних - {SelectedSampleData.Sample.Count}. " +
                $"Перше число в порівнянні - це критерій згоди, друге - a(для Колмогорова), квантиль XI2 - для Пірсона" +
                $"\nКритерій згоди Колмогоров : {Kolmogorov}" + KolmogorovRecord  +  
                $"\nКритерій згоди Пірсон(при степені вільності,v = {SelectedSampleData.ClassSize - 1}) : {Math.Abs(Pearson)}" + PearsonValue;
            SelectedSampleData.DistributionConsentTests = new DistributionConsentTest { KolmogorovTest = Kolmogorov, PirsonTest = Pearson };
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
            switch (Int32.Parse((string)p))
            {
                case 0:
                    RemoveAnomalyData.Move(SelectedSampleData);
                    break;
                case 1:
                    RemoveAnomalyData.Standartization(SelectedSampleData);
                    break;
                case 2:
                    RemoveAnomalyData.Log(SelectedSampleData);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region  CheckUniformy - Однородность выборки независимых, нормального распределения
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

            RecordValue = $"Перевірка однорідностей вибірок: перше число, з яким порівнюється - сама оцінка, друге число - квантиль, з яким іде порівняння";

            RecordValue = Uniformy.UniformyNormalRunIndependent(valuesSample, RecordValue);

        }
        #endregion

        #region  CheckUniformyDependent - Однородность выборки зависимых, нормального распределения
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

            RecordValue = Uniformy.UniformyNormalRunDependent(valuesSample, RecordValue);
        }
        #endregion

        #region  CheckUniformyDependentElse - Однородность выборки зависимых, неизвестного распределния
        public ICommand CheckUniformyDependentElse { get; }

        private bool CanCheckUniformyDependentElseExecute(object p)
        {
            return true;
        }

        private void OnCheckUniformyDependentElseExecuted(object p)
        {
            var items = (System.Collections.ICollection)p;
            int sizeOfValue = items.Count;

            ICollection<StatisticSample> valuesSample = new List<StatisticSample> { };

            foreach (var el in items)
                valuesSample.Add((StatisticSample)el);
            RecordValue = "";

            RecordValue = Uniformy.UniformyElseRunDependent(valuesSample, RecordValue);
        }
        #endregion

        #region  CheckUniformyElse - Однородность выборки независимых, неизвестного распределения
        public ICommand CheckUniformyElse { get; }

        private bool CanCheckUniformyElseyExecute(object p)
        {
            return true;
        }

        private void OnCheckUniformyElseExecuted(object p)
        {
            var items = (System.Collections.ICollection)p;
            int sizeOfValue = items.Count;

            ICollection<StatisticSample> valuesSample = new List<StatisticSample> { };

            foreach (var el in items)
                valuesSample.Add((StatisticSample)el);

            RecordValue = $"Перевірка однорідностей вибірок: перше число, з яким порівнюється - сама оцінка, друге число - квантиль, з яким іде порівняння";

            RecordValue = Uniformy.UniformyElseRunIndependent(valuesSample, RecordValue);

        }
        #endregion

        #region  AbbeTest - Аббе - Тест
        public ICommand AbbeTestRun { get; }

        private bool CanAbbeTestRunExecute(object p)
        {
            return true;
        }

        private void OnAbbeTestRunExecuted(object p)
        {
            if (SelectedSampleData != null)
            {
                double u = AbbeTest.AbbeTestRun(SelectedSampleData);

                RecordValue += $"Abbe тест - {u}";
            }
        }
        #endregion

        #region  OpenOptionsWindow - Открыть протокол по оценкам
        public ICommand OpenOptionsWindow { get; }

        private bool CanOpenOptionsWindowExecute(object p)
        {
            return true;
        }

        private void OnOpenOptionsWindowExecuted(object p)
        {
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;

            var otherWindowViewModel = new OptionsDataViewModel(this);
            displayRootRegistry.ShowPresentation(otherWindowViewModel);
        }
        #endregion

        #region  OpenOptionsWindow - Открыть протокол по интервалам
        public ICommand OpenDistributionWindow { get; }

        private bool CanOpenDistributionWindowExecute(object p)
        {
            return true;
        }

        private void OnOpenDistributionWindowExecuted(object p)
        {
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;

            var otherWindowViewModel = new DistributionDataViewModel(this);
            displayRootRegistry.ShowPresentation(otherWindowViewModel);
        }
        #endregion

        #endregion
        public MainWindowViewModel()
        {
            SampleData = new List<StatisticSample> { };


            GraphFunctionWindowModel = new GraphFunctionWindowViewModel(this);

            DataWindowModel = new DataWindowViewModel(this);

            CheckUniformyDependentElse = new LambdaCommand(OnCheckUniformyDependentElseExecuted, CanCheckUniformyDependentElseExecute);

            CheckUniformyElse = new LambdaCommand(OnCheckUniformyElseExecuted, CanCheckUniformyElseyExecute);

            GetFileName = new LambdaCommand(OnGetFileNameExecuted, CanGetFileNameExecute);

            DistributionStart = new LambdaCommand(OnDistributionStartExecuted, CanDistributionStartExecute);

            AnomalyDataRemove = new LambdaCommand(OnAnomalyDataRemoveExecuted, CanAnomalyDataRemoveExecute);

            CheckUniformy = new LambdaCommand(OnCheckUniformyExecuted, CanCheckUniformyExecute);

            CheckUniformyDependent = new LambdaCommand(OnCheckUniformyDependentExecuted, CanCheckUniformyDependentExecute);

            AbbeTestRun = new LambdaCommand(OnAbbeTestRunExecuted, CanAbbeTestRunExecute);

            OpenOptionsWindow = new LambdaCommand(OnOpenOptionsWindowExecuted, CanOpenOptionsWindowExecute);

            OpenDistributionWindow = new LambdaCommand(OnOpenDistributionWindowExecuted, CanOpenDistributionWindowExecute);

            TTestValue = new TTest();

            RecordValue = "";
            //
        }
    }
}
