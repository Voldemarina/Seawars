using Seawars.WPF.View.Windows;
using Seawars.WPF.Common.Commands.Base;

namespace Seawars.WPF.Common.Commands
{
    class SowRulesCommand : BaseCommand
    {
        public override bool CanExecute(object parameter) => true;
        public override void Execute(object parameter) => new RulesWindow().ShowDialog();
    }
}
