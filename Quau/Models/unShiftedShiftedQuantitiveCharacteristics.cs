using Quau.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Models
{
    class unShiftedShiftedQuantitiveCharacteristics : BaseModel
    {
        //MED
        private double _MED;
        public double MED { get => _MED; set => Set(ref _MED, value); }
        //MAD
        private double _MAD;
        public double MAD { get => _MAD; set => Set(ref _MAD, value); }
        //Дисперсія (зсунена, незсунена)
        private double _S_Variance_Shifted;
        public double S_Variance_Shifted { get => _S_Variance_Shifted; set => Set(ref _S_Variance_Shifted, value); }
        private double _S_Variance_unShifted;
        public double S_Variance_unShifted { get => _S_Variance_unShifted; set => Set(ref _S_Variance_unShifted, value); }
        //Середнє
        private double _AritmeitcMean;
        public double AritmeitcMean { get => _AritmeitcMean; set => Set(ref _AritmeitcMean, value); }
        //Середнєквадратичне
        private double _RouteMeanSquare;
        public double RouteMeanSquare { get => _RouteMeanSquare; set => Set(ref _RouteMeanSquare, value); }
        //Коефіцієнт асиметрії (зсунений, незсунений)
        private double _Skewness_Shifted;
        public double Skewness_Shifted { get => _Skewness_Shifted; set => Set(ref _Skewness_Shifted, value); }
        private double _Skewness_unShifted;
        public double Skewness_unShifted { get => _Skewness_unShifted; set => Set(ref _Skewness_unShifted, value); }
        //Коефіцієнт ексцесу (зсунений, незсунений)
        private double _Kurtosis_Shifted;
        public double Kurtosis_Shifted { get => _Kurtosis_Shifted; set => Set(ref _Kurtosis_Shifted, value); }
        private double _Kurtosis_unShifted;
        public double Kurtosis_unShifted { get => _Kurtosis_unShifted; set => Set(ref _Kurtosis_unShifted, value); }
        //Коефіцієнт контрексцесу
        private double _CounterKurtosis;
        public double CounterKurtosis { get => _CounterKurtosis; set => Set(ref _CounterKurtosis, value); }
        //Варіація пірсона
        private double _Pearson_Variation;
        public double Pearson_Variation { get => _Pearson_Variation; set => Set(ref _Pearson_Variation, value); }
        //Усічене середнє
    }
}
