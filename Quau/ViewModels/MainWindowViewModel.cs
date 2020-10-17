using Quau.Infrastructure.Commands;
using Quau.Models;
using Quau.Services;
using Quau.Services.FileOpenLoad;
using Quau.Services.StatisticOperation;
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
                Set(ref _SelectedSampleData, value);

                DataWindowModel.SelectedSampleData = SelectedSampleData;
                GraphFunctionWindowModel.SelectedSampleData = SelectedSampleData;
            }
        }

        #endregion

        #region SampleFilePath : string - путь к файлу с значением выборки

        private string _SampleFilePath;

        public string SampleFilePath
        {
            get => _SampleFilePath;
            set
            {
                Set(ref _SampleFilePath, value);

                StatisticSample tempSample = new StatisticSample();

                tempSample.Sample = DataConvertor.DataConvertorStrToDouble(ReadDataService.ReadData(_SampleFilePath));

                if (tempSample.Sample != null)
                {

                    StatisticOperationLauncher.StartStatisticOperation(tempSample);
                    QuantitiveCharacteristicsService.QuantitiveCharacteristics(tempSample);
                    var tempCollection = SampleData;
                    if (tempCollection == null)
                        tempCollection = new List<StatisticSample> { };
                    tempCollection.Add(tempSample);

                    SampleData = tempCollection;

                    // Временное хранилище для Скопированой выбраной выборки
                    SampleDataCopy = tempSample;
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
                Set(ref _SampleData, value);
                SelectedSampleData = _SampleData?.Last();
                DataWindowModel.SampleData = _SampleData;
                GraphFunctionWindowModel.SampleData = _SampleData;
                //Выбор выборки! Убрать отсюда в будущем и реализовать выбор выборки!

                //DataWindowModel.SelectedSampleData = _SampleData?.Last();
                //GraphFunctionWindowModel.SelectedSampleData = _SampleData?.Last();
            }
        }

        #endregion

        #region SampleDataCopy : ICollection<StatisticSample> - сохраненные данные о выборке(для возвращение SampleData начальной позиции

        private StatisticSample _SampleDataCopy;

        public StatisticSample SampleDataCopy { get => _SampleDataCopy; set => Set(ref _SampleDataCopy, value); }

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

        #region  DistributionStart - Выбор файла, путь к которому записывается в SampleFilePath
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
        }
        #endregion

        #endregion
        public MainWindowViewModel()
        {
            GraphFunctionWindowModel = new GraphFunctionWindowViewModel(this);

            DataWindowModel = new DataWindowViewModel(this);

            GetFileName = new LambdaCommand(OnGetFileNameExecuted, CanGetFileNameExecute);

            DistributionStart = new LambdaCommand(OnDistributionStartExecuted, CanDistributionStartExecute);
        }
    }
}
