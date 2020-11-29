using Quau.Models;
using Quau.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.ViewModels.OptionsData
{
    internal class DistributionDataViewModel : ViewModel
    {
        public MainWindowViewModel MainModel { get; }

        #region SelectedSampleData : StatisticSample - данные о выбраной выборке

        private StatisticSample _SelectedSampleData;

        public StatisticSample SelectedSampleData { get => _SelectedSampleData; set => Set(ref _SelectedSampleData, value); }

        #endregion

        public DistributionDataViewModel(MainWindowViewModel MainModel)
        {

            this.MainModel = MainModel;

            this.SelectedSampleData = this.MainModel.SelectedSampleData;
        }
    }
}
