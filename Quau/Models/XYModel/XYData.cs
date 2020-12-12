using Quau.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Models.XYModel
{
    class XYData : BaseModel
    {
        #region X : double - значение  X
        private double _X;
        public double X { get => _X; set => Set(ref _X, value); }
        #endregion

        #region Y : double - значение  Y
        private double _Y;
        public double Y { get => _Y; set => Set(ref _Y, value); }
        #endregion

        #region A : double - значение  A
        private double _A;
        public double A { get => _A; set => Set(ref _A, value); }
        #endregion
    }
}
