using Seawars.WPF.Common;
using Seawars.WPF.View.Pages;
using System.Windows.Controls;

namespace Seawars.WPF.Services.AuthorizationPageServices
{
    public class PageService : ViewModelBase
    {
        private Page _CurrentPage;
        public Page CurrentPage
        {
            get => _CurrentPage;
            set => Set(ref _CurrentPage, value);
        }
        public PageService() => CurrentPage = new AuthorizationPage();
        public void SetPage<T>(T page) where T : Page => CurrentPage = page as Page;
    }
}
