using Quau.Models;
using Quau.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ICollection<StatisticSample> SampleData { get => _SampleData; set => Set(ref _SampleData, value); }

        #endregion

        #region Test : ICollection<StatisticSample> - данные о выборке

        private ICollection<SampleRanking> _Test;

        public ICollection<SampleRanking> Test { get => _Test; set => Set(ref _Test, value); }

        #endregion

        /* ------------------------------------------------------------------------- */

        public DataWindowViewModel(MainWindowViewModel MainModel)
        {
            this.MainModel = MainModel;

            Test = new List<SampleRanking> { new SampleRanking { SampleData = 0, SampleDataFrequency = 0, SampleDataRelativeFrequency = 0 } };
            Test.Add(new SampleRanking { SampleData = 0, SampleDataFrequency = 0, SampleDataRelativeFrequency = 0 });
            Test.Add(new SampleRanking { SampleData = 0, SampleDataFrequency = 0, SampleDataRelativeFrequency = 0 });
            Test.Add(new SampleRanking { SampleData = 0, SampleDataFrequency = 0, SampleDataRelativeFrequency = 0 });
            Test.Add(new SampleRanking { SampleData = 0, SampleDataFrequency = 0, SampleDataRelativeFrequency = 0 });
            Test.Add(new SampleRanking { SampleData = 0, SampleDataFrequency = 0, SampleDataRelativeFrequency = 0 });
            Test.Add(new SampleRanking { SampleData = 0, SampleDataFrequency = 0, SampleDataRelativeFrequency = 0 });
        }
    }
}
