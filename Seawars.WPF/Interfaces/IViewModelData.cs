using System.Collections.ObjectModel;
using Seawars.Domain.Models;
using Seawars.WPF.Model;

namespace Seawars.WPF.Interfaces
{
    interface IViewModelData
    {
        public ObservableCollection<Button> Buttons { get; set; }
        public ObservableCollection<Ship> Ships { get; set; }

    }
}
