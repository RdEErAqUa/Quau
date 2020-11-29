using Quau.Models.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Models.ModelingSample
{
    class ModelingSample : BaseModel
    {
        //
        private double _y;
        public double y { get => _y; set => Set(ref _y, value); }
        //
        private ObservableCollection<StatisticSample> _statisticSamples_10;
        public ObservableCollection<StatisticSample> statisticSamples_10{get => _statisticSamples_10; set => Set(ref _statisticSamples_10, value); }

        private ObservableCollection<double> _tValue_10 = new ObservableCollection<double> { };
        public ObservableCollection<double> tValue_10 { get => _tValue_10; set => Set(ref _tValue_10, value); }
        //
        private ObservableCollection<StatisticSample> _statisticSamples_40;
        public ObservableCollection<StatisticSample> statisticSamples_40 { get => _statisticSamples_40; set => Set(ref _statisticSamples_40, value); }
        private ObservableCollection<double> _tValue_40 = new ObservableCollection<double> { };
        public ObservableCollection<double> tValue_40 { get => _tValue_40; set => Set(ref _tValue_40, value); }
        //
        private ObservableCollection<StatisticSample> _statisticSamples_100;
        public ObservableCollection<StatisticSample> statisticSamples_100 { get => _statisticSamples_100; set => Set(ref _statisticSamples_100, value); }
        private ObservableCollection<double> _tValue_100 = new ObservableCollection<double> { };
        public ObservableCollection<double> tValue_100 { get => _tValue_100; set => Set(ref _tValue_100, value); }
        //
        private ObservableCollection<StatisticSample> _statisticSamples_400;
        public ObservableCollection<StatisticSample> statisticSamples_400 { get => _statisticSamples_400; set => Set(ref _statisticSamples_400, value); }
        private ObservableCollection<double> _tValue_400 = new ObservableCollection<double> { };
        public ObservableCollection<double> tValue_400 { get => _tValue_400; set => Set(ref _tValue_400, value); }
        //
        private ObservableCollection<StatisticSample> _statisticSamples_1000;
        public ObservableCollection<StatisticSample> statisticSamples_1000 { get => _statisticSamples_1000; set => Set(ref _statisticSamples_1000, value); }
        private ObservableCollection<double> _tValue_1000 = new ObservableCollection<double> { };
        public ObservableCollection<double> tValue_1000 { get => _tValue_1000; set => Set(ref _tValue_1000, value); }
        //


    }
}
