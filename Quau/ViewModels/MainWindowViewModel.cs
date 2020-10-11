using Quau.Infrastructure.Commands;
using Quau.Models;
using Quau.Services;
using Quau.Services.FileOpenLoad;
using Quau.Services.StatisticOperation;
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

        public StatisticSample SelectedSampleData { get => _SelectedSampleData; set => Set(ref _SelectedSampleData, value); }

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

                StatisticOperationLauncher.StartStatisticOperation(tempSample);
                var tempCollection = SampleData;
                if (tempCollection == null)
                    tempCollection = new List<StatisticSample> { };
                tempCollection.Add(tempSample);

                SampleData = tempCollection;
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
                DataWindowModel.SampleData = _SampleData;

                //Выбор выборки! Убрать отсюда в будущем и реализовать выбор выборки!
                DataWindowModel.SelectedSampleData = _SampleData.Last();
            }
        }

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

        #endregion
        public MainWindowViewModel()
        {
            GraphFunctionWindowModel = new GraphFunctionWindowViewModel(this);

            DataWindowModel = new DataWindowViewModel(this);

            GetFileName = new LambdaCommand(OnGetFileNameExecuted, CanGetFileNameExecute);
        }
    }
}
