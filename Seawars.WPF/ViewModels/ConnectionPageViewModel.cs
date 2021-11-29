using System;
using System.Configuration;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Newtonsoft.Json;
using Seawars.Domain.Enums;
using Seawars.Infrastructure.Data;
using Seawars.Infrastructure.Encryption;
using Seawars.WPF.Services;
using Seawars.WPF.Common;
using Seawars.WPF.Common.Commands.Base;
using Seawars.WPF.Infrastructure;
using Seawars.WPF.View.Pages.Game;
using Seawars.WPF.View.Pages.Game.Connection;

namespace Seawars.WPF.ViewModels
{
    class ConnectionPageViewModel : ViewModelBase
    {
        #region Commands

        public ICommand PlayWithUserCommand { get; }
        public ICommand StartGameCommand { get; }

        public ICommand CreateNewGameCommand { get; }
        public ICommand JoinTheExistGameCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand ConnectAndStartCommand { get; }

        #endregion

        #region Game id

        private string _gameId;

        public string GameId
        {
            get => _gameId;
            set => Set(ref _gameId, value);
        }

        #endregion

        #region Data
        private readonly string Path = ConnectionStrings.ApiPath;
        #endregion

        public ConnectionPageViewModel()
        {
            PlayWithUserCommand = new Command(PlayWithUserCommandAction, x => true);
            StartGameCommand = new Command(StartGameCommandAction, x => true);
            CreateNewGameCommand = new Command(CreateNewGameCommandAction, x => true);
            JoinTheExistGameCommand = new Command(JoinTheGameCommandAction, x => true);
            BackCommand = new Command(BackCommandAction, x => true);
            ConnectAndStartCommand = new Command(ConnectAndStartCommandAction, x => true);
        }

        #region Commands actions

        private void PlayWithUserCommandAction(object obj) =>
            ServicesLocator.GamePageService.SetPage(new UsersConnectionPage());

        private void BackCommandAction(object obj)
        {
            StopWatch.StopTimer();
            ServicesLocator.GamePageService.SetPage(new ConnectionPage());
        }

        private void CreateNewGameCommandAction(object obj)
        {
            Task.Run(() =>
            {
                GameId = string.Empty;

                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() => ServicesLocator.GamePageService.SetPage(new NewGameCreationPage())));

                var content = new HttpRequestMessage(HttpMethod.Get, $"{Path}GameConnection/CreateGame");

                var request = new HttpClient().SendAsync(content);

                var response = request.Result.Content.ReadAsStringAsync().Result.ToString();

                var Game = JsonConvert.DeserializeObject<GameState>(response);

                GameId = TripleDes.Decrypted(Game.CryptedGameId).ToString();

                GameState.GetState(true, Game);

                StopWatch.StartTimer();

                while (GameState.GetState().DidEnemyConnect != true) Thread.Sleep(500);

                GameState.GetState(true, Game);

                StopWatch.StopTimer();

                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() => StartGameCommandAction(null)));

            });

        }

        private void ConnectAndStartCommandAction(object obj)
        {
            if (!int.TryParse(GameId, out int Id))
            {
                MessageBox.Show("Incorrect Id", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var content = new HttpRequestMessage(HttpMethod.Get, $"{Path}GameConnection/JoinToGame");

            content.Headers.Add("Id", TripleDes.Encrypted(Id));

            var request = new HttpClient().SendAsync(content);

            var response = request.Result.Content.ReadAsStringAsync().Result.ToString();

            var Game = JsonConvert.DeserializeObject<GameState>(response);

            if (Game is null)
            {
                MessageBox.Show("This this game is already running", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            GameState.GetState(false, Game);

            StartGameCommandAction(null);
        }

        private void JoinTheGameCommandAction(object obj)
        {
            GameId = string.Empty;

            ServicesLocator.GamePageService.SetPage(new ExistGameConnectionPage());
        }

        private void StartGameCommandAction(object obj)
        {
            var Game = new Domain.Entities.Game()
            {
                Status = (Status?) 3,
                User = App.CuurentUser,
            };

            App.CurrentGame = Game;

            ServicesLocator.GameRepository.Add(Game);

            ServicesLocator.GamePageService.SetPage(new UserFieldPage());
        }

        #endregion
    }
} 