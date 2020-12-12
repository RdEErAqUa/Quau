using Quau.Data;
using Quau.Models.Base;
using Quau.Models.DistributionConsent;
using Quau.Models.DistributionSet;
using Quau.Models.Histograma;
using Quau.Services.StatisticOperation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Models
{
    internal class StatisticSample : BaseModel
    {
        //Одновимірна вибірка

        #region ClassSize : double - кількість классів
        private double _ClassSize;
        public double ClassSize
        {
            get => _ClassSize; set
            {
                Set(ref _ClassSize, value);
                if (RoundValue == 0)
                {
                    RoundValue = 3;
                    foreach (var el in Sample)
                    {
                        RoundValue = DecimalData.GetDecimalDigitsCount(el) > RoundValue ? DecimalData.GetDecimalDigitsCount(el) : RoundValue;
                    }
                }
                StatisticOperationLauncher.StartStatisticOperation(this);
                QuantitiveCharactacteristics = new unShiftedShiftedQuantitiveCharacteristics(this.Sample, RoundValue);
                HistogramDataValue = new ObservableCollection<DataValueHistogram>(CreateEmpiricalData.CreateEmpiricalDataValue(this));
                QuantitiveCharacteristicsService.QuantitiveCharacteristics(this);
            }
        }
        #endregion
        public double StepSize { get; set; }

        public string fileName { get; set; }

        public int RoundValue { get; set; }
        
        #region Sample : ObservableCollection<double> - колекція вхідних даних
        private ObservableCollection<double> _Sample;
        public ObservableCollection<double> Sample
        {
            get => _Sample; set
            {
                Set(ref _Sample, value);
                ClassSize = Math.Ceiling(StatisticDivisionInClass.SizeClassesFind(this));
            }
        }
        #endregion
        
        #region SampleDataRanking : ObservableCollection<SampleRanking> - варіаційний ряд
        private ObservableCollection<SampleRanking> _SampleDataRanking;
        public ObservableCollection<SampleRanking> SampleDataRanking
        {
            get => _SampleDataRanking; set
            {
                Set(ref _SampleDataRanking, value);
            }
        }
        #endregion
        
        #region SampleDivisionINClass : ObservableCollection<SamplePrimaryDivisionINClass> - розбиття на класи
        private ObservableCollection<SamplePrimaryDivisionINClass> _SampleDaSampleDivisionINClass;
        public ObservableCollection<SamplePrimaryDivisionINClass> SampleDivisionINClass
        {
            get => _SampleDaSampleDivisionINClass; set
            {
                Set(ref _SampleDaSampleDivisionINClass, value);
            }
        }
        #endregion
        
        #region QuantitiveCharactacteristics : unShiftedShiftedQuantitiveCharacteristics - оцінки параметрів
        private unShiftedShiftedQuantitiveCharacteristics _QuantitiveCharactacteristics;
        public unShiftedShiftedQuantitiveCharacteristics QuantitiveCharactacteristics { get => _QuantitiveCharactacteristics; set => Set(ref _QuantitiveCharactacteristics, value); }
        #endregion
        
        #region DistributionSample : ObservableCollection<DistributionSamples> - функція щільності
        private ObservableCollection<DistributionSamples> _DistributionSample;
        public ObservableCollection<DistributionSamples> DistributionSample
        {
            get => _DistributionSample; set
            {
                Set(ref _DistributionSample, value);
            }
        }
        #endregion
        
        #region DistributionSampleEmpirical : ObservableCollection<DistributionSamples> - функція ймовірностей
        private ObservableCollection<DistributionSamples> _DistributionSampleEmpirical;
        public ObservableCollection<DistributionSamples> DistributionSampleEmpirical
        {
            get => _DistributionSampleEmpirical; set
            {
                Set(ref _DistributionSampleEmpirical, value);
            }
        }
        #endregion
        
        #region HistogramDataValue : ICollection<DataValueHistogram> - данные о эмпирической функции распределения

        private ObservableCollection<DataValueHistogram> _HistogramDataValue;

        public ObservableCollection<DataValueHistogram> HistogramDataValue { get => _HistogramDataValue; set => Set(ref _HistogramDataValue, value); }

        #endregion

        #region HistogramLowerLimit : ICollection<DataValueHistogram> - данные о эмпирической функции распределения, верхняя грань

        private ObservableCollection<DataValueHistogram> _HistogramLowerLimit;

        public ObservableCollection<DataValueHistogram> HistogramLowerLimit { get => _HistogramLowerLimit; set => Set(ref _HistogramLowerLimit, value); }

        #endregion

        #region HistogramUpperLimit : ICollection<DataValueHistogram> - данные о эмпирической функции распределения, нижняя грань

        private ObservableCollection<DataValueHistogram> _HistogramUpperLimit;

        public ObservableCollection<DataValueHistogram> HistogramUpperLimit { get => _HistogramUpperLimit; set => Set(ref _HistogramUpperLimit, value); }

        #endregion

        #region DistributionProtocol : String - протокол довірчого інтервального оцінювання, та оцінок параметрів розподілу

        private String _DistributionProtocol;

        public String DistributionProtocol { get => _DistributionProtocol; set => Set(ref _DistributionProtocol, value); }

        #endregion

        #region IntervalProtocol : String - протокол довірчого інтервального оцінювання

        private String _IntervalProtocol;

        public String IntervalProtocol { get => _IntervalProtocol; set => Set(ref _IntervalProtocol, value); }

        #endregion
        public DistributionConsentTest DistributionConsentTests { get; set; }
    }
}
