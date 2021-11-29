using System.Windows;
using Seawars.WPF.Common.Commands.Base;

namespace Seawars.WPF.Common.Commands
{
    class CloseCommand : BaseCommand
    {
        public override bool CanExecute(object parameter) => true;
        public override void Execute(object parameter) => System.Windows.Application.Current.Shutdown();
    }
}
