using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Application.BL;
using Newtonsoft.Json;
using Seawars.DAL.Repositories;
using Seawars.Domain.Entities;
using Seawars.Domain.Enums;
using Seawars.Domain.Models;
using Seawars.Infrastructure.Data;
using Seawars.Infrastructure.Extentions;
using Seawars.Interfaces.Game;
using Seawars.WPF.Common;
using Seawars.WPF.Common.Commands.Base;
using Seawars.WPF.Common.Data;
using Seawars.WPF.Infrastructure;
using Seawars.WPF.Interfaces;
using Seawars.WPF.Model;
using Seawars.WPF.Services;

namespace Seawars.WPF.ViewModels
{
    public class BattleGroundDataWithUserViewModel : ViewModelBase, IBattleGroundData
    {

        #region Private data
        private object locker = new object();
        private bool IsGameOver = default;
        private int NumberOfHostsField;
        private string Path = ConnectionStrings.ApiPath;
        GameState Game = GameState.GetState();
        private EnemyFieldViewModel Enemy;
        private UserFieldPageViewModel User;
        #endregion

        #region Public Data
        private string _attackHint = "";
        private int _enemyShipsCount = 10;
        private int _missCounter = 0;

        public string AttackHint
        {
            get => _attackHint;
            set => Set(ref _attackHint, value);
        }

        public int EnemyShipsCount
        {
            get => _enemyShipsCount;
            set => Set(ref _enemyShipsCount, value);
        }

        public int MissCounter
        {
            get => _missCounter;
            set => Set(ref _missCounter, value);
        }
        #endregion

        public ICommand AttackCommand { get; set; }

        public BattleGroundDataWithUserViewModel()
        {
            AttackCommand = new Command(AttackCommandAction, CanUseAttackCommand);

            Enemy = ServicesLocator.EnemyFieldViewModel;

            User = ServicesLocator.UserFieldPageViewModel;

            NumberOfHostsField = Game.CurrentUserIsHost is true ? 1 : 2;

            ChangeButtonView(GameState.GetState().CurrentUserIsHost);

            StopWatch.UpdateGameState += NativeShipAssignment;
        }

        #region Commands
        private bool CanUseAttackCommand(object arg) => !IsGameOver;

        private void AttackCommandAction(object obj)
        {
            int FieldForAttack = GameState.GetState().CurrentUserIsHost is true ? 2 : 1;

            var cell = obj.DetermineCellNumber();

            var game = GameState.GetState();

            if (game[FieldForAttack].CanAttackCell(cell) is false) return;

            var response = HttpRequest.Get(Path + "battleground/Attack", $"?fields={FieldForAttack}&Cell={cell}");

            game[FieldForAttack] = JsonConvert.DeserializeObject<Field>(response);

            Enemy.Buttons = ShipsAssignment(cell, game[FieldForAttack], Enemy);

            AddStepToDb(cell, Move.Your);

        }

        private void NativeShipAssignment()
        {
            lock (locker)
            {
                Game = GameState.GetState();

                ChangeButtonView
                (Game.CurrentUserIsHost && Game.IsFirstUserMove ||
                 !Game.CurrentUserIsHost && !Game.IsFirstUserMove);

                bool isMissed = true;

                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    isMissed = UpdateShipsViewState(isMissed);

                    UpdateMissedViewState(isMissed, Game[NumberOfHostsField], User);

                }));
            }
        }
        #endregion

        #region Private methods
        private void ShowKilledShip(int i, int j, IViewModelData vm)
        {
            int Key = 0;
            int Cell = Key.ConverIndexToCell(i, j);
            string Path = "/" + vm.Buttons[Key.ConverIndexToCell(i, j)].Content.Source
                .ToString()
                .Trim("pack://application:,,,"
                .ToCharArray())
                .ToString();

            if (vm.Ships[Cell].isHorizontal is true)
            {
                Key = PathToShipContent.HorizontalShips
                .FirstOrDefault(x => x.Value == Path).Key;

                if (Key is 0) return;

                ShipsOptions(vm, Cell, PathToShipContent.Horizontal_Dead_Ships[Key], 1, vm.Ships[Cell]);
            }
            if (vm.Ships[Cell].isHorizontal is false)
            {
                Key = PathToShipContent.VerticalShips
                .FirstOrDefault(x => x.Value == Path).Key;

                if (Key is 0) return;

                ShipsOptions(vm, Cell, PathToShipContent.Vertical_Dead_Ships[Key], 1, vm.Ships[Cell]);
            }

        }
        private void ChangeButtonView(bool isEnabled)
        {
            _ = Enumerable.Range(0, 121).Select(x =>
            {
                var _points = x.ConvertCellToIndexes();
                var _indexes = new Cell(_points.Item1, _points.Item2);
                if (_indexes.Y != 0 && _indexes.X != 0)
                    return Enemy.Buttons[x].CanUse = isEnabled;
                return Enemy.Buttons[x].CanUse = false;
            }).ToArray();
        }
        private void ShipsOptions(IViewModelData Data, int cell, string Path, double Thickness, Ship ship)
        {
            Data.Buttons[cell] = new Button
            {
                Content = new System.Windows.Controls.Image
                {
                    Source = new BitmapImage(new Uri(Path, UriKind.Relative)),
                    Stretch = Stretch.Fill
                },
                Border = new Thickness(Thickness),
                CanUse = false
            };
            ship.isDead = true;
            Data.Ships[cell] = new Ship(ship);
        }


        private void UpdateMissedViewState(bool isMissed, Field Field, IViewModelData vm)
        {
            int Cell = default;
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                for (int i = 0; i < 11; i++)
                {
                    for (int j = 0; j < 11; j++)
                    {
                        if (vm.Ships[Cell.ConverIndexToCell(i, j)].isDead is false && Field.field[i, j] is ShipsMark.Missed)
                        {
                            if (isMissed is true)
                            {
                                AttackHint = AttackHint.ShowAttackHint(i, j);
                                AddStepToDb(i.ConverIndexToCell(i, j), Move.Enimy);
                            }
                            ShipsOptions(vm, Cell.ConverIndexToCell(i, j), PathToShipContent.MissedMark, 0.5, vm.Ships[Cell.ConverIndexToCell(i, j)]);
                        }
                    }
                }
                if (User.IsAllElementsIsNull(User.OneDeckShip, User.TwoDeckShip, User.ThrieDeckShip, User.FourDeckShip))
                {
                    GameOverSettings("You loose!", false);
                }
            }));
        }
        private ObservableCollection<Button> ShipsAssignment(int Cell, Field Field, IViewModelData vm)
        {
            var indexes = Cell.ConvertCellToIndexes();
            var _cell = new Cell(indexes.Item1, indexes.Item2);

            if (Field.field[_cell.Y, _cell.X] is ShipsMark.Missed)
            {
                if (Game.IsGameWithComputer) GameState.GetState().IsFirstUserMove = false;

                ChangeButtonView(false);

                ShipsOptions(Enemy, Cell, PathToShipContent.MissedMark, 0.5, new Ship());
                MissCounter++;
                return vm.Buttons;
            }

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (vm.Ships[Cell.ConverIndexToCell(i, j)].isDead is false && Field.field[i, j] is ShipsMark.Missed)
                    {
                        ShipsOptions(vm, Cell.ConverIndexToCell(i, j), PathToShipContent.MissedMark, 0.5, new Ship());
                        continue;
                    }
                    if (vm.Ships[Cell.ConverIndexToCell(i, j)].isDead is false && Field.field[i, j] is ShipsMark.DeadShip)
                    {
                        var ship = Field.ShowShipsOptions(_cell);
                        bool isKilled = IsKilled(ship, _cell, Field.field);

                        if (isKilled is false) ShipsOptions(vm, Cell.ConverIndexToCell(i, j), PathToShipContent.KilledShip, 1, ship);
                        if (isKilled is true)
                        {
                            int cell = default;
                            cell = cell.ConverIndexToCell(i, j);

                            if (ship.isHorizontal is true) ShowDeadHorizontalShips(vm, ship, cell - ship.NumberOfHintDeck);
                            if (ship.isHorizontal is false) ShowDeadVerticalShips(vm, ship, cell - ship.NumberOfHintDeck * 11);

                            EnemyShipsCount--;

                            if (EnemyShipsCount is 0) GameOverSettings("You win!", true);

                        }
                    }
                }
            }
            return vm.Buttons;
        }

        private void AddGameToDb(GameState _game)
        {
            var game = ServicesLocator.GameRepository.GetById(App.CurrentGame.Id);

            game.Status = _game.CurrentUserIsHost is true && _game.IsFirstUserWin
                          || !_game.CurrentUserIsHost && !_game.IsFirstUserWin
                ? Status.Win
                : Status.Loose;

            var user = ServicesLocator.UserRepository.GetById(App.CuurentUser.Id);

            user.CountOfWonGames += game.Status is Status.Win ? 1 : 0;
            user.GamesWithComputer += Game.IsGameWithComputer is true ? 1 : 0;
            user.TotalGamesCount++;

            ServicesLocator.Repository.Save();
        }
        private void AddStepToDb(int cell, Move move)
        {
            var step = new Step()
            {
                Y = cell / 11,
                X = cell % 11,
                Move = move,
                Game = App.CurrentGame,
            };

            ServicesLocator.StepRepository.Add(step);
        }

        private bool UpdateShipsViewState(bool isMissed)
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (User.Ships[i.ConverIndexToCell(i, j)].isDead is false && Game[NumberOfHostsField].field[i, j] is ShipsMark.DeadShip)
                    {
                        isMissed = false;
                        AttackHint = AttackHint.ShowAttackHint(i, j);
                        ShowKilledShip(i, j, User);
                        ReduceTheShipsCount(Game[NumberOfHostsField]);
                        AddStepToDb(i.ConverIndexToCell(i, j), Move.Enimy);
                    }
                }
            }

            return isMissed;
        }

        private void GameOverSettings(string text, bool IsWin)
        {
            var game = GameState.GetState();
            StopWatch.StopTimer();
            StopWatch.UpdateGameState -= NativeShipAssignment;
            IsGameOver = true;
            game.IsGameOver = true;
            game.IsFirstUserWin = NumberOfHostsField is 1 ? IsWin : !IsWin;
            AddGameToDb(game);
            MessageBox.Show(text, "", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ReduceTheShipsCount(Field field)
        {
            User.OneDeckShip = field.OneDeckShip;
            User.TwoDeckShip = field.TwoDeckShip;
            User.ThrieDeckShip = field.ThrieDeckShip;
            User.FourDeckShip = field.FourDeckShip;
        }
        private bool IsKilled(Ship ship, Cell indexes, string[,] field)
        {
            bool isKilled = false;
            for (int k = 0; k < ship.DecksCount; k++)
            {
                int I = indexes.Y;
                int J = indexes.X;
                _ = ship.isHorizontal is true ? J += k - ship.NumberOfHintDeck : I += k - ship.NumberOfHintDeck;
                if (I > 10 || J > 10) continue;
                if (field[I, J] is ShipsMark.DeadShip) { isKilled = true; continue; }
                isKilled = false; break;
            }
            return isKilled;
        }

        #region ShipsView
        private void ShowDeadVerticalShips(IViewModelData Data, Ship ship, int cell)
        {
            ship.isHorizontal = false;
            switch (ship.DecksCount)
            {
                default:
                    break;

                case 1:
                    ShipsOptions(Data, cell, PathToShipContent.Vertical_Dead_OneDeckShip, 1, ship);
                    break;
                case 2:
                    ShipsOptions(Data, cell, PathToShipContent.Vertical_Dead_TwoDeckShip_FirstDeck, 1, ship);
                    ShipsOptions(Data, cell + 11, PathToShipContent.Vertical_Dead_TwoDeckShip_SecondDeck, 1, ship);
                    break;
                case 3:
                    ShipsOptions(Data, cell, PathToShipContent.Vertical_Dead_ThrieDeckShip_FirstDeck, 1, ship);
                    ShipsOptions(Data, cell + 11, PathToShipContent.Vertical_Dead_ThrieDeckShip_SecondDeck, 1, ship);
                    ShipsOptions(Data, cell + 22, PathToShipContent.Vertical_Dead_ThrieDeckShip_ThirdDeck, 1, ship);
                    break;
                case 4:
                    ShipsOptions(Data, cell, PathToShipContent.Vertical_Dead_FourDeckShip_FirstDeck, 1, ship);
                    ShipsOptions(Data, cell + 11, PathToShipContent.Vertical_Dead_FourDeckShip_SecondDeck, 1, ship);
                    ShipsOptions(Data, cell + 22, PathToShipContent.Vertical_Dead_FourDeckShip_ThirdDeck, 1, ship);
                    ShipsOptions(Data, cell + 33, PathToShipContent.Vertical_Dead_FourDeckShip_FourDeck, 1, ship);
                    break;

            }
        }
        private void ShowDeadHorizontalShips(IViewModelData Data, Ship ship, int cell)
        {
            ship.isHorizontal = true;
            switch (ship.DecksCount)
            {
                default:
                    break;

                case 1:
                    ShipsOptions(Data, cell, PathToShipContent.Horizontal_Dead_Ships[1], 1, ship);
                    break;
                case 2:
                    ShipsOptions(Data, cell, PathToShipContent.Horizontal_Dead_Ships[2], 1, ship);
                    ShipsOptions(Data, cell + 1, PathToShipContent.Horizontal_Dead_Ships[3], 1, ship);
                    break;
                case 3:
                    ShipsOptions(Data, cell, PathToShipContent.Horizontal_Dead_Ships[4], 1, ship);
                    ShipsOptions(Data, cell + 1, PathToShipContent.Horizontal_Dead_Ships[5], 1, ship);
                    ShipsOptions(Data, cell + 2, PathToShipContent.Horizontal_Dead_Ships[6], 1, ship);
                    break;
                case 4:
                    ShipsOptions(Data, cell, PathToShipContent.Horizontal_Dead_Ships[7], 1, ship);
                    ShipsOptions(Data, cell + 1, PathToShipContent.Horizontal_Dead_Ships[8], 1, ship);
                    ShipsOptions(Data, cell + 2, PathToShipContent.Horizontal_Dead_Ships[9], 1, ship);
                    ShipsOptions(Data, cell + 3, PathToShipContent.Horizontal_Dead_Ships[10], 1, ship);
                    break;

            }
        }
        #endregion
        #endregion
    }
} 
