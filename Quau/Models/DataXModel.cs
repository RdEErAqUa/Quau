using Quau.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Models
{
    class DataXModel : BaseModel
    {
        #region X : double - значение X, которому отвечают определенные Y

        private double _X;

        public double X { get => _X; set => Set(ref _X, value); }

        #endregion

        #region Y : double - значение Y, который отвечают определенному X

        private List<double> _Y;

        public List<double> Y { get => _Y; set => Set(ref _Y, value); }

        #endregion
    }
}
