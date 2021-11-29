using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Application.BL;
using Seawars.Domain.Entities;
using Seawars.Domain.Enums;
using Seawars.Domain.Models;
using Seawars.Infrastructure.Data;
using Seawars.Infrastructure.Extentions;
using Seawars.Interfaces.Game;
using Seawars.WPF.Common;
using Seawars.WPF.Common.Commands.Base;
using Seawars.WPF.Common.Data;
using Seawars.WPF.Interfaces;
using Seawars.WPF.Model;
using Seawars.WPF.Services;

namespace Seawars.WPF.ViewModels
{
    public class BattleGroundDataWithComputerViewModel : ViewModelBase, IBattleGroundData
    {
        #region Data

        private string _attackHint = "";
        private int _enemyShipsCount = 10;
        private int _missCounter = 0;

        private bool isComputerMove = default;
        private bool IsGameOver = default;

        private EnemyFieldViewModel Enemy;
        private UserFieldPageViewModel User;

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

        #region Commands
        public ICommand AttackCommand { get; set; }
        #endregion

        public BattleGroundDataWithComputerViewModel()
        {
            AttackCommand = new Command(AttackCommandAction, x => !isComputerMove && !IsGameOver);

            Enemy = ServicesLocator.EnemyFieldViewModel;

            User = ServicesLocator.UserFieldPageViewModel;
        }

        private void AttackCommandAction(object obj)
        {
            int cell = obj.DetermineCellNumber();

            var points = cell.ConvertCellToIndexes();

            var indexes = new Cell(points.Item1, points.Item2);

            if (Enemy.Field.CanAttackCell(cell) is false) return;

            var items = Enemy.Field.Attack(indexes);

            var isMissed = items.Item2;

            isComputerMove = isMissed;

            Enemy.Field = items.Item1;

            Enemy.Buttons = ShowEnemyShips(indexes, isMissed);

            AddStepToDb(cell, Move.Your);

            ComputerAttack();
        }

        private void ComputerAttack()
        {
            if(isComputerMove is false) return;

            var items = ComputerIntelligence.ComputerAttack(User.Field);

            var isMissed = items.Item2;

            isComputerMove = !isMissed;

            User.Field.field = items.Item1;

            User.Buttons = ShowUserShip();
        }

        #region Private methods
        private ObservableCollection<Button> ShowUserShip()
        {
            bool isMissed = true;
            Task.Run(() =>
            {
                for (int i = 0; i < 11; i++)
                {
                    for (int j = 0; j < 11; j++)
                    {
                        if (User.Ships[ConverIndexToCell(i,j)].isDead is false && User.Field.field[i, j] is ShipsMark.DeadShip)
                        {
                            isMissed = false;
                            AttackHint = AttackHint.ShowAttackHint(i, j);
                            AddStepToDb(ConverIndexToCell(i,j), Move.Enimy);
                            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                ShowKilledShip(i, j, User);
                                ReduceTheShipsCount();
                            }), DispatcherPriority.Normal);
                            Thread.Sleep(500);
                        }
                    }
                }

                if (isMissed)
                    Thread.Sleep(700);
                MissedMarkAssignment(isMissed);
            });
            return User.Buttons;
        }
        private ObservableCollection<Button> ShowEnemyShips(Cell indexes, bool isMissed)
        {
            int _cell = ConverIndexToCell(indexes.Y, indexes.X);

            if (isMissed is true)
            {
                CreateShip(Enemy, _cell, PathToShipContent.MissedMark, 0.5, new Ship());

                ChangeButtonView(false);

                MissCounter++;
                return Enemy.Buttons;
            }

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (Enemy.Ships[ConverIndexToCell(i, j)].isDead is false && Enemy.Field.field[i, j] is ShipsMark.Missed)
                    {
                        CreateShip(Enemy, ConverIndexToCell(i, j), PathToShipContent.MissedMark, 0.5, new Ship());
                        continue;
                    }

                    if (Enemy.Ships[ConverIndexToCell(i, j)].isDead is false &&
                        Enemy.Field.field[i, j] is ShipsMark.DeadShip)
                    {
                        var ship = Enemy.Field.ShowShipsOptions(indexes);

                        bool isKilled = IsKilled(ship, indexes, Enemy.Field);

                        if (isKilled is false)
                            CreateShip(Enemy, _cell, PathToShipContent.KilledShip, 1, ship);

                        if (isKilled is true)
                        {
                            int cell = default;
                            cell = cell.ConverIndexToCell(i, j);

                            if (ship.isHorizontal is true)
                                ShowDeadHorizontalShips(Enemy, ship, cell - ship.NumberOfHintDeck);
                            if (ship.isHorizontal is false)
                                ShowDeadVerticalShips(Enemy, ship, cell - ship.NumberOfHintDeck * 11);

                            EnemyShipsCount--;

                            if (EnemyShipsCount is 0) GameOverSettings("You win!", true);

                        }
                    }
                }
            }
            return Enemy.Buttons;
        }
        private bool IsKilled(Ship ship, Cell cell, Field field)
        {

            MarkTheShip(cell, ShipsMark.DeadShip, field);

            for (int i = 0; i < ship.DecksCount; i++)
            {
                int I = cell.Y;
                int J = cell.X;

                _ = ship.isHorizontal is true
                    ? J = cell.X - ship.NumberOfHintDeck + i
                    : I = cell.Y - ship.NumberOfHintDeck + i;

                if (field.field[I, J] is ShipsMark.DeadShip)
                    continue;

                return false;

            }

            return true;
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
        private void CreateShip(IViewModelData Data, int cell, string Path, double Thickness, Ship ship)
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

                CreateShip(vm, Cell, PathToShipContent.Horizontal_Dead_Ships[Key], 1, vm.Ships[Cell]);
            }
            if (vm.Ships[Cell].isHorizontal is false)
            {
                Key = PathToShipContent.VerticalShips
                    .FirstOrDefault(x => x.Value == Path).Key;

                if (Key is 0) return;

                CreateShip(vm, Cell, PathToShipContent.Vertical_Dead_Ships[Key], 1, vm.Ships[Cell]);
            }

        }
        private int ConverIndexToCell(int indexX, int indexY)
        {
            return indexX * 11 + indexY;
        }
        private void MarkTheShip(Cell Indexes, string Mark, Field field)
        {
            field.field[Indexes.Y, Indexes.X] = Mark;
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
                                                                                                                                                                                                                                                                                        private void MissedMarkAssignment(bool isMissed)
        {
            int Cell = default;
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                for (int i = 0; i < 11; i++)
                {
                    for (int j = 0; j < 11; j++)
                    {
                        if (User.Ships[ConverIndexToCell(i, j)].isDead is false && User.Field.field[i, j] is ShipsMark.Missed)
                        {
                            if (isMissed is true)
                            {
                                AttackHint = AttackHint.ShowAttackHint(i, j);
                                AddStepToDb(ConverIndexToCell(i,j), Move.Enimy);
                            }
                            CreateShip(User, ConverIndexToCell(i, j), PathToShipContent.MissedMark, 0.5, User.Ships[ConverIndexToCell(i, j)]);
                        }
                    }
                }
                ChangeButtonView(true);
                if (Cell.IsAllElementsIsNull<int>(User.OneDeckShip, User.TwoDeckShip, User.ThrieDeckShip, User.FourDeckShip))
                {
                    GameOverSettings("You loose", false);
                    return;
                }
                isComputerMove = false;
            }));
        }
        private void GameOverSettings(string text, bool IsWin)
        {
            MessageBox.Show(text, "", MessageBoxButton.OK, MessageBoxImage.Information);
            IsGameOver = true;
            AddGameToDb(IsWin);
        }

        private void AddGameToDb(bool IsWin)
        {
            var game = ServicesLocator.GameRepository.GetById(App.CurrentGame.Id);

            game.Status = IsWin is true? Status.Win: Status.Loose;

            var user = ServicesLocator.UserRepository.GetById(App.CuurentUser.Id);

            user.CountOfWonGames += IsWin is true ? 1 : 0;
            user.GamesWithComputer++;
            user.TotalGamesCount++;

            ServicesLocator.Repository.Save();
        }

        private void ReduceTheShipsCount()
        {
            User.OneDeckShip = User.Field.OneDeckShip;
            User.TwoDeckShip = User.Field.TwoDeckShip;
            User.ThrieDeckShip = User.Field.ThrieDeckShip;
            User.FourDeckShip = User.Field.FourDeckShip;
        }
        #region View

        private void ShowDeadVerticalShips(IViewModelData Data, Ship ship, int cell)
        {
            ship.isHorizontal = false;
            switch (ship.DecksCount)
            {
                default:
                    break;

                case 1:
                    CreateShip(Data, cell, PathToShipContent.Vertical_Dead_OneDeckShip, 1, ship);
                    break;
                case 2:
                    CreateShip(Data, cell, PathToShipContent.Vertical_Dead_TwoDeckShip_FirstDeck, 1, ship);
                    CreateShip(Data, cell + 11, PathToShipContent.Vertical_Dead_TwoDeckShip_SecondDeck, 1, ship);
                    break;
                case 3:
                    CreateShip(Data, cell, PathToShipContent.Vertical_Dead_ThrieDeckShip_FirstDeck, 1, ship);
                    CreateShip(Data, cell + 11, PathToShipContent.Vertical_Dead_ThrieDeckShip_SecondDeck, 1, ship);
                    CreateShip(Data, cell + 22, PathToShipContent.Vertical_Dead_ThrieDeckShip_ThirdDeck, 1, ship);
                    break;
                case 4:
                    CreateShip(Data, cell, PathToShipContent.Vertical_Dead_FourDeckShip_FirstDeck, 1, ship);
                    CreateShip(Data, cell + 11, PathToShipContent.Vertical_Dead_FourDeckShip_SecondDeck, 1, ship);
                    CreateShip(Data, cell + 22, PathToShipContent.Vertical_Dead_FourDeckShip_ThirdDeck, 1, ship);
                    CreateShip(Data, cell + 33, PathToShipContent.Vertical_Dead_FourDeckShip_FourDeck, 1, ship);
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
                    CreateShip(Data, cell, PathToShipContent.Horizontal_Dead_Ships[1], 1, ship);
                    break;
                case 2:
                    CreateShip(Data, cell, PathToShipContent.Horizontal_Dead_Ships[2], 1, ship);
                    CreateShip(Data, cell + 1, PathToShipContent.Horizontal_Dead_Ships[3], 1, ship);
                    break;
                case 3:
                    CreateShip(Data, cell, PathToShipContent.Horizontal_Dead_Ships[4], 1, ship);
                    CreateShip(Data, cell + 1, PathToShipContent.Horizontal_Dead_Ships[5], 1, ship);
                    CreateShip(Data, cell + 2, PathToShipContent.Horizontal_Dead_Ships[6], 1, ship);
                    break;
                case 4:
                    CreateShip(Data, cell, PathToShipContent.Horizontal_Dead_Ships[7], 1, ship);
                    CreateShip(Data, cell + 1, PathToShipContent.Horizontal_Dead_Ships[8], 1, ship);
                    CreateShip(Data, cell + 2, PathToShipContent.Horizontal_Dead_Ships[9], 1, ship);
                    CreateShip(Data, cell + 3, PathToShipContent.Horizontal_Dead_Ships[10], 1, ship);
                    break;

            }
        }

        #endregion
        #endregion
    }
}
