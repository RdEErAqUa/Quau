using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Models
{
    class unShiftedShiftedQuantitiveCharacteristics
    {
        //MED
        public double MED { get; set; }
        //MAD
        public double MAD { get; set; }
        //Дисперсія (зсунена, незсунена)
        public double S_Variance_Shifted { get; set; }
        public double S_Variance_unShifted { get; set; }
        //Середнє
        public double AitherticMean { get; set; }
        //Середнєквадратичне
        public double RouteMeanSquare { get; set; }
        //Коефіцієнт асиметрії (зсунений, незсунений)
        public double Skewness_Shifted { get; set; }
        public double Skewness_unShifted { get; set; }
        //Коефіцієнт ексцесу (зсунений, незсунений)
        public double Kurtosis_Shifted { get; set; }
        public double Kurtosis_unShifted { get; set; }
        //Коефіцієнт контрексцесу
        public double CounterKurtosis { get; set; }
        //Варіація пірсона
        public double Pearson_Variation { get; set; }
        //Усічене середнє
    }
}
