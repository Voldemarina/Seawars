using System;
using System.Windows;
using System.Windows.Controls;
using Seawars.WPF.Common;

namespace Seawars.WPF.Model
{
    public class Button : ViewModelBase
    {
        public Thickness Border { get; set; } = new Thickness(0.3);
        public Image Content { get; set; }

        private Boolean _CanUse = true;
        public Boolean CanUse
        {
            get => _CanUse;
            set => Set(ref _CanUse, value);
        }
    }
}
