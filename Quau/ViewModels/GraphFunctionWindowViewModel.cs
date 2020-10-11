using Quau.Models;
using Quau.Models.Histograma;
using Quau.Services.StatisticOperation;
using Quau.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.ViewModels
{
    internal class GraphFunctionWindowViewModel : ViewModel
    {
        public MainWindowViewModel MainModel { get; }

        /* ------------------------------------------------------------------------- */

        #region Properties

        #region SelectedSampleData : StatisticSample - данные о выбраной выборке

        private StatisticSample _SelectedSampleData;

        public StatisticSample SelectedSampleData
        {
            get => _SelectedSampleData; set
            {
                Set(ref _SelectedSampleData, value);
                HistogramDataValue = CreateEmpiricalData.CreateEmpiricalDataValue(_SelectedSampleData);
            }
        }

        #endregion
        #region SampleData : ICollection<StatisticSample> - данные о выборке

        private ICollection<StatisticSample> _SampleData;

        public ICollection<StatisticSample> SampleData {get => _SampleData; set => Set(ref _SampleData, value);}

        #endregion

        #region HistogramDataValue : ICollection<DataValueHistogram> - данные о эмпирической функции распределения

        private ICollection<DataValueHistogram> _HistogramDataValue;

        public ICollection<DataValueHistogram> HistogramDataValue { get => _HistogramDataValue; set => Set(ref _HistogramDataValue, value); }

        #endregion
        #endregion

        /* ------------------------------------------------------------------------- */

        public GraphFunctionWindowViewModel(MainWindowViewModel MainModel)
        {
            this.MainModel = MainModel;
        }
    }
}
