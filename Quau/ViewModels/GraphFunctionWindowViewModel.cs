using Quau.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.ViewModels
{
    internal class GraphFunctionWindowViewModel : ViewModel
    {
        public MainWindowViewModel MainModel { get; }

        public GraphFunctionWindowViewModel(MainWindowViewModel MainModel)
        {
            this.MainModel = MainModel;
        }
    }
}
