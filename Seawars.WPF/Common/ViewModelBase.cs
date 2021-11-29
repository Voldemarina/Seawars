using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Seawars.WPF.Common
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertName);
            return true;
        }
        protected virtual bool AddLastSymbol(ref string field, string value, [CallerMemberName] string propertName = null)
        {
            if (Equals(field, value)) return false;
            if (value is "*")
            {
                field = string.Empty;
                goto PropertyChanged;
            }
            if (field is not null && value.Length < field.Length)
            {
                field = field.Remove(field.Length - 1);
                goto PropertyChanged;
            }

            field += value[value.Length - 1];

            PropertyChanged:

            OnPropertyChanged(propertName);
            return true;
        }

    }
}

