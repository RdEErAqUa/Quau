using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Models.Base
{
    class BaseModel : INotifyPropertyChanged
    {
        #region Шаблон события изменение елемента
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, bool doForce = false,[CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value) && !doForce) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;

        }
        #endregion
    }
}
