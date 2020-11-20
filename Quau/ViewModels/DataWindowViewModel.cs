using Quau.Infrastructure.Commands;
using Quau.Models;
using Quau.ViewModels.Base;
using System;
using System.Collections.Generic;
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

        public StatisticSample SelectedSampleData { get => _SelectedSampleData; set => Set(ref _SelectedSampleData, value, true); }

        #endregion

        #region SampleData : ICollection<StatisticSample> - данные о выборке

        private ICollection<StatisticSample> _SampleData;

        public ICollection<StatisticSample> SampleData { get => _SampleData; set { Set(ref _SampleData, value, true);} }

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


        public DataWindowViewModel(MainWindowViewModel MainModel)
        {
            TTestFind = new LambdaCommand(OnTTestFindExecuted, CanTTestFindExecute);

            this.MainModel = MainModel;
        }
    }
}
