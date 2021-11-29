using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Seawars.WPF.Common;
using Seawars.WPF.View.Pages;
using Seawars.WPF.View.Pages.Game;

namespace Seawars.WPF.Services.GamePagesService
{
    public class GamePageService : ViewModelBase
    {
        private Page _CurrentPage;
        public Page CurrentPage
        {
            get => _CurrentPage;
            set => Set(ref _CurrentPage, value);
        }
        public GamePageService() => CurrentPage = new ConnectionPage();
        public void SetPage<T>(T page) where T : Page => CurrentPage = page as Page;
    }
}
