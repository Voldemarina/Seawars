using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Application.BL;
using Newtonsoft.Json;
using Seawars.Domain.Enums;
using Seawars.Domain.Models;
using Seawars.Infrastructure.Data;
using Seawars.Infrastructure.Extentions;
using Seawars.Infrastructure.Validation;
using Seawars.WPF.Common;
using Seawars.WPF.Common.Commands.Base;
using Seawars.WPF.Common.Data;
using Seawars.WPF.Infrastructure;
using Seawars.WPF.Interfaces;
using Seawars.WPF.Model;
using Seawars.WPF.Services;
using Seawars.WPF.View.Pages.Game;

namespace Seawars.WPF.ViewModels
{
    public class UserFieldPageViewModel : ViewModelBase, IViewModelData
    {
        #region Commands
        public ICommand ShipsAutoGenerationCommand { get; }
        public ICommand CleanFieldCommand { get; }
        public ICommand ReadyCommand { get; }
        public ICommand TakeShipCommand { get; }
        #endregion

        public Field Field { get; set; }

        public IEnumerable<Button> _buttons { get; set; }
        public IEnumerable<Ship> _ships { get; set; }
        public IEnumerable<SolidColorBrush> _colors { get; set; }

        #region Private Data
        private bool Direction = true;
        private int DecksInShipCount = default;
        private int FieldNumber;
        private bool CanUseCommands = true;

        private string Path = ConnectionStrings.ApiPath;
        #endregion

        #region Ships

        private int _OneDeckShip = 4;
        private int _TwoDeckShip = 3;
        private int _ThrieDeckShip = 2;
        private int _FourDeckShip = 1;

        public int FourDeckShip
        {
            get => _FourDeckShip;
            set => Set(ref _FourDeckShip, value);
        }

        public int ThrieDeckShip
        {
            get => _ThrieDeckShip;
            set => Set(ref _ThrieDeckShip, value);
        }

        public int TwoDeckShip
        {
            get => _TwoDeckShip;
            set => Set(ref _TwoDeckShip, value);
        }

        public int OneDeckShip
        {
            get => _OneDeckShip;
            set => Set(ref _OneDeckShip, value);
        }

        #endregion

        #region Cells

        private ObservableCollection<Button> _Buttons;
        private ObservableCollection<Ship> _Ships;
        private ObservableCollection<Brush> _Color;

        public ObservableCollection<Brush> Color
        {
            get => _Color;
            set => Set(ref _Color, value);
        }

        public ObservableCollection<Ship> Ships
        {
            get => _Ships;
            set => Set(ref _Ships, value);
        }

        public ObservableCollection<Button> Buttons
        {
            get => _Buttons;
            set => Set(ref _Buttons, value);
        }

        #endregion

        public UserFieldPageViewModel()
        {
            TakeShipCommand = new Command(TakeShipCommandAction, x => CanUseCommands);
            ShipsAutoGenerationCommand = new Command(ShipsAutoGenerationAction, x => CanUseCommands);
            ReadyCommand = new Command(ReadyCommandAction, x => true);
            CleanFieldCommand = new Command(CleanFieldCommandAction, x => CanUseCommands);

            _buttons = Enumerable.Range(0, 121).Select(i => new Button());
            _ships = Enumerable.Range(0, 121).Select(i => new Ship());
            _colors = Enumerable.Range(0, 121).Select(i => new SolidColorBrush(Colors.White));

            Buttons = new ObservableCollection<Button>(_buttons);
            Ships = new ObservableCollection<Ship>(_ships);
            Color = new ObservableCollection<Brush>(_colors);

            Field = UpdateFieldState();

            FieldNumber = GameState.GetState().CurrentUserIsHost is true ? 1 : 2;

        }

        private void CleanFieldCommandAction(object obj) => CleanField();
        private void ReadyCommandAction(object obj) => Ready();
        private void ShipsAutoGenerationAction(object obj) => AutoGeneration();
        private void TakeShipCommandAction(object obj) => Take(obj);

        #region Private Methods

        private void Ready()
        {
            if (CanUseCommands is false) return;

            if (Validator.NotNullElementsExist(OneDeckShip, TwoDeckShip, ThrieDeckShip, FourDeckShip)) return;

            if (GameState.GetState().IsGameWithComputer is true) GameWithComputer();

            if (GameState.GetState().IsGameWithComputer is not true) GameWithUser();
        }

        private void GameWithUser()
        {
            Task.Run(() =>
            {
                CanUseCommands = false;

                var field = JsonConvert.SerializeObject(Field);

                string response = HttpRequest.Get(Path + "FieldCreation/ReadyToStart",
                    $"?fields={FieldNumber}&Field={field}");

                var _game = JsonConvert.DeserializeObject<GameState>(response);

                GameState.GetState(GameState.GetState().CurrentUserIsHost, _game);

                StopWatch.StartTimer();

                MessageBox.Show("Wait for your oponent");

                do
                {
                    Thread.Sleep(500);
                } while (GameState.GetState().IsFirstUserReadyToStartGame != true ||
                         GameState.GetState().IsSecondUserReadyToStartGame != true);


                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Field = UpdateFieldState();
                    CloneShipsCount();
                    ServicesLocator.GamePageService.SetPage(new BattleGroundPage((GameMode)2));
                }));
            });
        }
        private void GameWithComputer()
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ResetShipsCount();
                Field.ResetShips();
                ServicesLocator.GamePageService.SetPage(new BattleGroundPage((GameMode)1));
            }));
        }


        private void Take(object _ship)
        {
            var ship = _ship as System.Windows.Controls.Image;

            if (ship is null) return;

            DecksInShipCount = DetermineShipSize(ship);

            DragDrop.DoDragDrop(ship, ship.Source, DragDropEffects.Copy);

            ShowShipsOnField();
        }
        private void AutoGeneration()
        {
            Buttons = new ObservableCollection<Button>(_buttons);
            Ships = new ObservableCollection<Ship>(_ships);

            Field = Field.FieldAutoGeneration();

            ShowShipsOnField();
        }
        private void CleanField()
        {
            Color = new ObservableCollection<Brush>(_colors);
            Ships = new ObservableCollection<Ship>(_ships);
            Buttons = new ObservableCollection<Button>(_buttons);

            ResetShipsCount();

            Field = Field.ReloadField();

        }


        private void ShowShipsOnField()
        {
            for (int i = 0; i < 121; i++)
            {
                if (Field.field[i / 11, i % 11] is ShipsMark.Ship && Ships[i].isOnField is false)
                {
                    var ship = Field.ShowShipsOptions(new Cell(i / 11, i % 11));

                    if (ship.isHorizontal is true)
                        ShowShips(i, ship, PathToShipContent.HorizontalShips, 1);

                    if (ship.isHorizontal is false)
                        ShowShips(i, ship, PathToShipContent.VerticalShips, 11);
                }
            }

            CloneShipsCount();

        }
        private void ShowShips(int Cell, Ship ship, Dictionary<int, string> Path, int nextCell)
        {
            switch (ship.DecksCount)
            {
                default: break;
                case 1:
                    CreateShip(Cell, Path[1], ship);
                    break;
                case 2:
                    CreateShip(Cell, Path[2], ship);
                    CreateShip(Cell + nextCell, Path[3], ship);
                    break;
                case 3:
                    CreateShip(Cell, Path[4], ship);
                    CreateShip(Cell + nextCell, Path[5], ship);
                    CreateShip(Cell + nextCell * 2, Path[6], ship);
                    break;
                case 4:
                    CreateShip(Cell, Path[7], ship);
                    CreateShip(Cell + nextCell, Path[8], ship);
                    CreateShip(Cell + nextCell * 2, Path[9], ship);
                    CreateShip(Cell + nextCell * 3, Path[10], ship);
                    break;
            }
        }
        private void CreateShip(int Cell, string Path, Ship ship)
        {
            Buttons[Cell] = new Button
            {
                Content = new System.Windows.Controls.Image
                {
                    Source = new BitmapImage(new Uri(Path, UriKind.Relative)),
                    Stretch = Stretch.Fill
                },
                Border = new Thickness(1)
            };
            Ships[Cell] = new Ship
            {
                isOnField = true,
                isHorizontal = ship.isHorizontal,
                DecksCount = ship.DecksCount,
                Position = new Cell(Cell % 11, Cell / 11)
            };

        }
        private void CloneShipsCount()
        {
            OneDeckShip = Field.OneDeckShip;
            TwoDeckShip = Field.TwoDeckShip;
            ThrieDeckShip = Field.ThrieDeckShip;
            FourDeckShip = Field.FourDeckShip;
        }
        private void ResetShipsCount()
        {
            OneDeckShip = 4;
            TwoDeckShip = 3;
            ThrieDeckShip = 2;
            FourDeckShip = 1;
        }
        private Field UpdateFieldState()
        {
            return GameState.GetState().CurrentUserIsHost is true
                ? GameState.GetState().FirstUserField
                : GameState.GetState().SecondUserField;
        }
        private int DetermineShipSize(System.Windows.Controls.Image ship)
        {
            string ShipsName = ship.Name.ToString();
            int DecksCount = default;
            switch (ShipsName)
            {
                default: return -1;
                case "FourDeckShip":
                    DecksCount = 4;
                    if (FourDeckShip is 0) return -1;
                    break;
                case "ThrieDeckShip":
                    DecksCount = 3;
                    if (ThrieDeckShip is 0) return -1;
                    break;
                case "DoubleDeckShip":
                    DecksCount = 2;
                    if (TwoDeckShip is 0) return -1;
                    break;
                case "OneDeckShip":
                    DecksCount = 1;
                    if (OneDeckShip is 0) return -1;
                    break;
            }

            return DecksCount;
        }

        #endregion

        #region DragDrop
        internal void DropAction(object sender, DragEventArgs args)
        {
            DragAndDrop.Drop(args);
        }
        internal void DragEnter(object sender, DragEventArgs args)
        {
            DragAndDrop.Enter(args, DecksInShipCount, Direction);
        }
        internal void DragLeave(object sender, DragEventArgs args)
        {
            DragAndDrop.Leave(args);
        }
        internal void DragTheShipOnTheFieldAction(object sender, MouseButtonEventArgs args)
        {
            if (CanUseCommands is false) return;
            var btn = sender as System.Windows.Controls.Button;
            if (btn.Content is null) return;
            int Cell = btn.Name.DetermineCellNumber();
            var ship = Field.ShowShipsOptions(new Cell(Cell.ConvertCellToIndexes().Item1,
                Cell.ConvertCellToIndexes().Item2));
            DecksInShipCount = ship.DecksCount;
            Direction = ship.isHorizontal;
            DragAndDrop.Drag(ship, btn);
            ShowShipsOnField();
        }
        #endregion
    }
} 