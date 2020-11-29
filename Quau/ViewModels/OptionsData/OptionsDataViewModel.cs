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

namespace Quau.ViewModels.OptionsData
{
    internal class OptionsDataViewModel : ViewModel
    {
        public MainWindowViewModel MainModel { get;}

        #region SelectedSampleData : StatisticSample - данные о выбраной выборке

        private StatisticSample _SelectedSampleData;

        public StatisticSample SelectedSampleData { get => _SelectedSampleData; set => Set(ref _SelectedSampleData, value); }

        #endregion


        public OptionsDataViewModel(MainWindowViewModel MainModel)
        {
            this.MainModel = MainModel;

            this.SelectedSampleData = this.MainModel.SelectedSampleData;
        }
    }
}
