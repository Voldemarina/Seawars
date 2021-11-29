using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Application.BL;
using Seawars.Domain.Models;
using Seawars.Infrastructure.Data;
using Seawars.Infrastructure.Extentions;
using Seawars.WPF.Model;
using Seawars.WPF.Services;
using Seawars.WPF.ViewModels;

namespace Seawars.WPF.Infrastructure
{
    public static class DragAndDrop
    {
        #region Data
        private static int Cell = default;
        private static bool Direction = default;
        private static int DecksCount = default;
        private static bool isDroped = default;
        private static int initialCell = default;
        private static int DesignatedDeck = default;
        private static Cell CellIndex;
        #endregion

        #region Action
        internal static void Drop(DragEventArgs args)
        {
            var vm = ServicesLocator.UserFieldPageViewModel;

            vm.Color = new ObservableCollection<Brush>(vm._colors);

            var Source = args.Source as FrameworkElement;

            isDroped = true;

            int cell = Source.Name.DetermineCellNumber();

            var items = cell.ConvertCellToIndexes();

            Cell index = new Cell(items.Item1, items.Item2);

            if (initialCell == cell)
            {
                CreateShip(vm.Field, index);

                var ship = vm.Field.ShowShipsOptions(index);

                vm.Field = vm.Field.ChangeShipDirection(Cell);

                var newShip = vm.Field.ShowShipsOptions(index);

                if (ship.isHorizontal == newShip.isHorizontal && newShip.DecksCount != 1)

                    RemoveShipCount(ship.DecksCount, vm.Field);

                return;
            }

            bool canPut = vm.Field.CanPutShip(index.Y, index.X, DecksCount, Direction);

            if (canPut is false) { isDroped = false; return; }

            vm.Field.PutShip(cell, DecksCount, Direction);
        }
        internal static void Enter(DragEventArgs args, int DecksInShip, bool direction)
        {
            var vm = ServicesLocator.UserFieldPageViewModel;

            isDroped = false;

            var feSource = args.Source as FrameworkElement;

            Cell = feSource.Name.DetermineCellNumber();

            DecksCount = DecksInShip;

            Direction = direction;

            if (Cell is -1) return;

            var items = Cell.ConvertCellToIndexes();

            CellIndex = new Cell(items.Item1, items.Item2);

            if (direction is true) Hint(vm.Field, vm, NextCell: 1);

            if (direction is false) Hint(vm.Field, vm, NextCell: 11);

        }
        internal static void Leave(DragEventArgs args)
        {
            var vm = ServicesLocator.UserFieldPageViewModel;

            if (Cell is -1) return;

            for (int i = 0; i < DecksCount; i++)
            {
                if (Direction is true) ReduceColor(vm, (Cell + i), 120);

                if (Direction is false) ReduceColor(vm, (Cell + Cell.ConverIndexToCell(i, 0)), 121);
            }
            isDroped = true;
        }
        internal static void Drag(Ship ship, System.Windows.Controls.Button btn)
        {
            var vm = ServicesLocator.UserFieldPageViewModel;

            initialCell = initialCell.ConverIndexToCell(ship.Position.Y, ship.Position.X);

            DesignatedDeck = ship.NumberOfHintDeck;

            object content = btn.Content;

            DeleateShip(ship, vm.Field, vm);

            DragDrop.DoDragDrop(btn, content, DragDropEffects.Copy);

            if (isDroped is false)
            {
                int i = ship.Position.Y, j = ship.Position.X;
                if (ship.isHorizontal is false) i -= ship.NumberOfHintDeck;
                else j -= ship.NumberOfHintDeck;
                vm.Field.PutShip(i.ConverIndexToCell(i, j), ship.DecksCount, ship.isHorizontal);
            }
            AddShipCount(ship.DecksCount, vm.Field);
        }
        #endregion

        #region Private Methods
        private static void ShowGrinHint(int i_index, int Direction, UserFieldPageViewModel vm)
        {
            vm.Color[Cell + i_index * Direction] = new SolidColorBrush(Colors.Green);
            vm.Color[Cell + i_index * Direction].Opacity = 0.4;

        }
        private static void ShowRedHint(int index, int Direction, UserFieldPageViewModel vm)
        {
            for (int j = 0; j < DecksCount; j++)
            {
                if (Direction is 1) if (index + j == 11) break;
                if (Cell + j * Direction >= 121) break;
                vm.Color[Cell + j * Direction] = new SolidColorBrush(Colors.Red);
                vm.Color[Cell + j * Direction].Opacity = 0.4;
            }
        }
        private static void Hint(Field field, UserFieldPageViewModel vm, int NextCell)
        {
            for (int i = DecksCount - 1; i >= 0; i--)
            {
                if (!field.CanPutShip(CellIndex.Y, CellIndex.X, DecksCount, Direction))
                {
                    ShowRedHint(CellIndex.X, NextCell, vm);
                    return;
                }
                ShowGrinHint(i, NextCell, vm);
            }
        }
        private static void ReduceColor(UserFieldPageViewModel vm, int value, int ComparebleNumber)
        {
            if (value > ComparebleNumber) return;
            vm.Color[value] = new SolidColorBrush(Colors.White);
            vm.Color[value].Opacity = 1;

        }
        private static void DeleateShip(Ship ship, Field field, UserFieldPageViewModel vm)
        {
            int Cell = default;
            if (ship.isHorizontal is true)
            {
                int FirstDeckOfHorizontalShip = ship.Position.X - ship.NumberOfHintDeck;
                for (int i = 0; i < ship.DecksCount; i++)
                {
                    field.field[ship.Position.Y, FirstDeckOfHorizontalShip + i] = null;
                    vm.Buttons[Cell.ConverIndexToCell(ship.Position.Y, FirstDeckOfHorizontalShip + i)] = new Button();
                    vm.Ships[Cell.ConverIndexToCell(ship.Position.Y, FirstDeckOfHorizontalShip + i)] = new Ship();
                }
            }

            if (ship.isHorizontal is false)
            {
                int FirstDeckOfVerticalShip = ship.Position.Y - ship.NumberOfHintDeck;
                for (int i = 0; i < ship.DecksCount; i++)
                {
                    field.field[FirstDeckOfVerticalShip + i, ship.Position.X] = null;
                    vm.Buttons[Cell.ConverIndexToCell(FirstDeckOfVerticalShip + i, ship.Position.X)] = new Button();
                    vm.Ships[Cell.ConverIndexToCell(FirstDeckOfVerticalShip + i, ship.Position.X)] = new Ship();
                }
            }
        }
        private static void CreateShip(Field field, Cell index)
        {
            for (int i = 0; i < DecksCount; i++)
            {
                if (Direction is true) field.field[index.Y, index.X - DesignatedDeck + i] = ShipsMark.Ship;
                else field.field[index.Y - DesignatedDeck + i, index.X] = ShipsMark.Ship;
            }
        }
        private static void AddShipCount(int DecksCount, Field field)
        {
            switch (DecksCount)
            {
                default: break;
                case 1: field.OneDeckShip++; break;
                case 2: field.TwoDeckShip++; break;
                case 3: field.ThrieDeckShip++; break;
                case 4: field.FourDeckShip++; break;
            }
        }
        private static void RemoveShipCount(int DecksCount, Field field)
        {
            switch (DecksCount)
            {
                default: break;
                case 1: field.OneDeckShip--; break;
                case 2: field.TwoDeckShip--; break;
                case 3: field.ThrieDeckShip--; break;
                case 4: field.FourDeckShip--; break;
            }
        }
        #endregion
    }
}
