using Quau.Models.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Models.DimentionalModel.Two
{
    class TwoDimentionalSample : BaseModel
    {
        #region X : double - значение X, которому отвечают определенные Y

        private double _X;

        public double X { get => _X; set => Set(ref _X, value); }

        #endregion

        #region Y : double - значение Y, который отвечают определенному X

        private double _Y;

        public double Y { get => _Y; set => Set(ref _Y, value); }

        #endregion

        #region N : int - количество Y которые отвечают конкретному X

        private double _N;

        public double N { get => _N; set => Set(ref _N, value); }

        #endregion
    }
}
