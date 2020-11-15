using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Quau.ViewModels.Base
{
    internal abstract class ViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Шаблон события изменение елемента
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, bool doForce = false, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value) && !doForce) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;

        }
        #endregion

        #region Деструктор

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
