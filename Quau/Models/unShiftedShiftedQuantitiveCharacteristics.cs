using Quau.Models.Base;
using Quau.Models.XYModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quau.Services.StatisticOperation;

namespace Quau.Models
{
    class unShiftedShiftedQuantitiveCharacteristics : BaseModel
    {
        #region MED : double - MED
        private double _MED;
        public double MED { get => _MED; set => Set(ref _MED, value); }
        #endregion

        #region MAD : double - MAD
        private double _MAD;
        public double MAD { get => _MAD; set => Set(ref _MAD, value); }
        #endregion

        #region S_Variance_unShifted : double - дисперсія
        private double _S_Variance_Shifted;
        public double S_Variance_Shifted { get => _S_Variance_Shifted; set => Set(ref _S_Variance_Shifted, value); }
        #endregion

        #region S_Variance_unShifted : double - дисперсія(незсунена)
        private double _S_Variance_unShifted;
        public double S_Variance_unShifted { get => _S_Variance_unShifted; set => Set(ref _S_Variance_unShifted, value); }
        #endregion

        #region ArithemticMean : double - середнє арифметичне
        private double _AritmeitcMean;
        public double AritmeitcMean { get => _AritmeitcMean; set => Set(ref _AritmeitcMean, value); }
        #endregion

        #region AritmeitcMeanConfidenceInterval : ObservableCollection<XYData> - довірчий інтервал
        private ObservableCollection<XYData> _AritmeitcMeanConfidenceInterval;

        public ObservableCollection<XYData> AritmeitcMeanConfidenceInterval { get => _AritmeitcMeanConfidenceInterval; set => Set(ref _AritmeitcMeanConfidenceInterval, value); }
        #endregion

        #region RouteMeanSquare : double - середньоквадратичне
        private double _RouteMeanSquare;
        public double RouteMeanSquare { get => _RouteMeanSquare; set => Set(ref _RouteMeanSquare, value); }
        #endregion

        #region Skewness : double - коефіцієнт асиметрії
        private double _Skewness;
        public double Skewness { get => _Skewness; set => Set(ref _Skewness, value); }
        #endregion

        #region SkewnessUnShifted : double - коефіцієнт асиметрії(незсунений)
        private double _SkewnessUnShifted;
        public double SkewnessUnShifted { get => _SkewnessUnShifted; set => Set(ref _SkewnessUnShifted, value); }
        #endregion

        #region Kurtosis : double - коефіцієнт ексцесу
        private double _Kurtosis;
        public double Kurtosis { get => _Kurtosis; set => Set(ref _Kurtosis, value); }
        #endregion

        #region KurtosisUnShifted : double - коефіцієнт ексцесу(незсунений)
        private double _KurtosisUnShifted;
        public double KurtosisUnShifted { get => _KurtosisUnShifted; set => Set(ref _KurtosisUnShifted, value); }
        #endregion

        #region ConterKurtosis : double - коефіцієнт ексцесу(незсунений)
        private double _ConterKurtosis;
        public double CounterKurtosis { get => _ConterKurtosis; set => Set(ref _ConterKurtosis, value); }
        #endregion

        #region Pearson_Variation : double - Варіація Пірсона
        private double _Pearson_Variation;
        public double Pearson_Variation { get => _Pearson_Variation; set => Set(ref _Pearson_Variation, value); }
        #endregion

        public unShiftedShiftedQuantitiveCharacteristics(ICollection<double> Sample, int RoundValue)
        {
            MED = Math.Round(QuantitiveCharacteristicsService.MEDFind(Sample), RoundValue);
            MAD = Math.Round(QuantitiveCharacteristicsService.MADFind(Sample), RoundValue);
            S_Variance_Shifted = Math.Round(QuantitiveCharacteristicsService.S_Variance_Shifted(Sample), RoundValue);
            S_Variance_unShifted = Math.Round(QuantitiveCharacteristicsService.S_Variance_unShifted(Sample), RoundValue);
            AritmeitcMean = Math.Round(QuantitiveCharacteristicsService.ArithmeticalMean(Sample), RoundValue);
            RouteMeanSquare = Math.Round(QuantitiveCharacteristicsService.RouteMeanSquare(Sample), RoundValue);
            Skewness = Math.Round(QuantitiveCharacteristicsService.Skewness(Sample), RoundValue);
            SkewnessUnShifted = Math.Round(QuantitiveCharacteristicsService.SkewnessUnShifted(Sample), RoundValue);
            Kurtosis = Math.Round(QuantitiveCharacteristicsService.Kurtosis(Sample), RoundValue);
            KurtosisUnShifted = Math.Round(QuantitiveCharacteristicsService.KurtosisUnShifted(Sample), RoundValue);
            CounterKurtosis = Math.Round(QuantitiveCharacteristicsService.CounterKurtosis(Sample), RoundValue);
            Pearson_Variation = Math.Round(QuantitiveCharacteristicsService.Pearson_Variation(Sample), RoundValue);
        }
    }
}
