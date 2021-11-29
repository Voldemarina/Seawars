using System.Diagnostics;
using Seawars.WPF.Common.Commands.Base;
using Seawars.WPF.Services;
using Seawars.WPF.View.UserControls;

namespace Seawars.WPF.Common.Commands
{
    public class NewGameCommand : BaseCommand
    {
        public override bool CanExecute(object parameter) => true;
        public override void Execute(object parameter)//TODO: restart game: *Observer* all VM subscribe on observer class, all VM implement some interface with Reload method, that reload all vm data. Or make scope for one game, hold it in app services. 
        {
            Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            System.Windows.Application.Current.Shutdown();

            //ServicesLocator.UserPageViewModel.CurrentViewControl = new ProfileControl();
            //App.WindowCurrent.Close();
            //App.UserProfileWindow.Show();
        }
    }
} 
