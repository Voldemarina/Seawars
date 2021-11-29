using System;
using Seawars.WPF.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Seawars.WPF.Authorization.Model;
using System.Windows.Input;
using Seawars.WPF.Authorization.View.UserControls;
using Seawars.WPF.Common;
using Seawars.WPF.Common.Commands.Base;
using Seawars.WPF.View.UserControls;
using Seawars.WPF.View.Windows;

namespace Seawars.WPF.ViewModels
{
    public class UserPageViewModel : ViewModelBase
    {
        #region Profile data
        public string Name => $"Name:  {App.CuurentUser.Name}";
        public string Username => $"Username:  {App.CuurentUser.UserName}";
        public string TotalGamesCount => $"Total games count:  {App.CuurentUser.TotalGamesCount}";
        public string GamesWithComputer => $"Games with computer count:  {App.CuurentUser.GamesWithComputer}";
        public string CountOfWonGames => $"Win games count:  {App.CuurentUser.CountOfWonGames}";

        #endregion

        #region View
        private object _CurrentViewControl;
        public object CurrentViewControl
        {
            get => _CurrentViewControl;
            set => Set(ref _CurrentViewControl, value);
        }
        #endregion

        #region Collections

        private ObservableCollection<Games> _Games;

        public ObservableCollection<Games> Games
        {
            get => _Games;
            set => Set(ref _Games, value);
        }

        private ObservableCollection<Steps> _Steps;

        public ObservableCollection<Steps> Steps
        {
            get => _Steps;
            set => Set(ref _Steps, value);
        }

        #endregion

        #region DataGrid

        public Games SelectedGame { get; set; }
        #endregion

        #region Commands

        public ICommand ShowStatisticCommand { get; }
        public ICommand StartGameCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand GameDetailsCommand { get; }
        
        #endregion

        public UserPageViewModel()
        {
            ShowStatisticCommand = new Command(ShowStatistic, x => true);
            StartGameCommand = new Command(StartGame, x => true);
            BackCommand = new Command(Back, x => true);
            GameDetailsCommand = new Command(GameDetails, x => true);
        }

        private void Back(object obj) =>
            CurrentViewControl
                = CurrentViewControl is GamesStatisticControl
                ?  new ProfileControl()
                :  new GamesStatisticControl();
        private void GameDetails(object obj)
        {
            if(SelectedGame is null) return;

            var steps = ServicesLocator.StepRepository
                .GetAll()
                .Where(x => x.Game is not null && x.Game.Id == SelectedGame.Id)
                .Select((x, y) => new Steps(x.X, x.Y, y + 1, x.Move))
                .ToList();

            Steps = new ObservableCollection<Steps>(steps);

            CurrentViewControl = new StepsStatisticControl();

            SelectedGame = null;
        }
        private void ShowStatistic(object obj)
        {
            var games = ServicesLocator.GameRepository
                .GetAll()
                .Where(x => x.User is not null &&  x.User.Id == App.CuurentUser.Id)
                .Select((x,y) => new Games(y+1, x.Status, x.Id))
                .ToList();

                Games = new ObservableCollection<Games>(games);

                CurrentViewControl = new GamesStatisticControl();

        }
 
        private void StartGame(object obj)
        {
            App.UserProfileWindow.Hide();

            new GameWindow().Show();
        }
    }
}
