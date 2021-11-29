using System;
using System.Drawing;
using Seawars.Domain.Models;
using Seawars.Infrastructure.Data;
using Seawars.Infrastructure.Extentions;

namespace Application.BL
{
    public class Field
    {
        public string[,] field { get; set; } = new string[11, 11];
        public int OneDeckShip { get; set; } = 4;
        public int TwoDeckShip { get; set; } = 3;
        public int ThrieDeckShip { get; set; } = 2;
        public int FourDeckShip { get; set; } = 1;
        public DateTime Timer { get; set; } = DateTime.Now;

        private string Id;
        public Field(string Id) => this.Id = Id;
        public Field() { }

        public Field ChangeShipDirection(int _cell)
        {
            ResetTimer();

            if (_cell.DoesTheShipExist(field) is false) return this;

            var items = _cell.ConvertCellToIndexes();

            var indexes = new Cell(items.Item1, items.Item2);

            var ship = ShowShipsOptions(indexes);

            ShipsModification(indexes, ship.NumberOfHintDeck, ship.DecksCount, mark: null, ship.isHorizontal);

            bool canPutShip = CanPutShip(indexes.Y, indexes.X, ship.DecksCount, !ship.isHorizontal);

            if (canPutShip is false)
                ShipsModification(indexes, ship.NumberOfHintDeck, ship.DecksCount, mark: ShipsMark.Ship,
                    ship.isHorizontal);

            else field = PutShip(_cell, ship.DecksCount, !ship.isHorizontal).field;

            return this;
        }

        public Ship ShowShipsOptions(Cell cellIndexes)
        {
            var Ship = new Ship();

            Ship.NumberOfHintDeck = SearchTheFirstShipsDeck(cellIndexes);

            Ship.isHorizontal = DeterminingTheDirection(cellIndexes.Y, cellIndexes.X);

            Ship.DecksCount = CountingDecks(cellIndexes);

            Ship.Position = new Cell(cellIndexes.Y, cellIndexes.X);

            Ship.isOnField = true; //TODO: condition

            return Ship;

        }

        public Field PutShip(int Cell, int DecksCount, bool Direction)
        {
            ResetTimer();

            var items = Cell.ConvertCellToIndexes();

            var Indexes = new Cell(items.Item1, items.Item2);

            bool canPut = CanPutShip(Indexes.Y, Indexes.X, DecksCount, Direction);

            if (canPut is true)
            {
                for (int i = 0; i < DecksCount; i++)
                {
                    int firstIndex = Indexes.Y;
                    int secondIndex = Indexes.X;

                    _ = Direction is true ? secondIndex = Indexes.X + i : firstIndex = Indexes.Y + i;

                    field[firstIndex, secondIndex] = ShipsMark.Ship;
                }

                ReduceShipsCount(DecksCount);
            }

            return this;
        }

        public bool CanPutShip(int Current_I, int Current_J, int DeksCount, bool isHorizontal)
        {
            int CellsToCheckIn_X_Axis = 2;
            int CellsToCheckIn_Y_Axis = 2;

            int J_Iteration_Count = 0;
            int I_teration_Count = 0;

            _ = isHorizontal is false ? CellsToCheckIn_Y_Axis += DeksCount - 1 : CellsToCheckIn_X_Axis += DeksCount - 1;


            for (int i = Current_I - 1; i < Current_I + CellsToCheckIn_Y_Axis; i++)
            {
                J_Iteration_Count = 0;
                for (int j = Current_J - 1; j < Current_J + CellsToCheckIn_X_Axis; j++)
                {
                    if (j < 0 || j > 10 || i < 0 || i > 10) return false;
                    if (!string.IsNullOrEmpty(field[i, j])) return false;

                    J_Iteration_Count++;
                    if (J_Iteration_Count == CellsToCheckIn_X_Axis && j == 10)
                        break;
                }

                I_teration_Count++;
                if (I_teration_Count == CellsToCheckIn_Y_Axis && i == 10)
                    break;
            }

            return true;
        }

        public Field FieldAutoGeneration()
        {
            ReloadField();
            var Random = new Random();
            int ShipCount = 1;
            int DeksCount = 4;

            for (int i = ShipCount; i <= 4; i++)
            {
                for (int j = 1; j <= ShipCount; j++)
                {
                    int firstIndex = Random.Next(1, 11);
                    int secondIndex = Random.Next(1, 11);

                    int direction = Random.Next(1, 3);

                    bool isHorizontal = direction is 1 ? true : false;

                    bool canPutShip = CanPutShip(firstIndex, secondIndex, DeksCount, isHorizontal);

                    if (canPutShip is false)
                    {
                        j--;
                        continue;
                    }

                    if (canPutShip is true)
                    {
                        for (int k = 0; k < DeksCount; k++)
                        {
                            if (isHorizontal is true)
                                field[firstIndex, secondIndex + k] = ShipsMark.Ship;

                            if (isHorizontal is false)
                                field[firstIndex + k, secondIndex] = ShipsMark.Ship;
                        }

                    }
                }

                ShipCount++;
                DeksCount--;
            }

            OneDeckShip = 0;
            TwoDeckShip = 0;
            ThrieDeckShip = 0;
            FourDeckShip = 0;
            return this;
        }

        public bool CanAttackCell(int Cell)
        {
            var items = Cell.ConvertCellToIndexes();

            var Indexes = new Cell(items.Item1, items.Item2);

            if (field[Indexes.Y, Indexes.X] is ShipsMark.DeadShip ||
                field[Indexes.Y, Indexes.X] is ShipsMark.Missed)
                return false;
            return true;
        }

        public (Field, bool) Attack(Cell Indexes)
        {
            ResetTimer();

            bool isMissed = IsMissed(Indexes);

            if (isMissed is true)
            {
                MarkTheShip(Indexes, ShipsMark.Missed);
                return (this, isMissed);
            }

            var ship = ShowShipsOptions(Indexes);

            bool isShipKilled = IsKilled(Indexes, ship);

            if (isShipKilled is true) ShipsFuneral(Indexes, ship);

            return (this, isMissed);
        }

        public Field ReloadField()
        {
            ResetTimer();
            ResetShips();
            field = new string[11, 11];
            return this;
        }

        public void ResetShips()
        {
            ResetTimer();
            OneDeckShip = 4;
            TwoDeckShip = 3;
            ThrieDeckShip = 2;
            FourDeckShip = 1;
        }

        #region Private Methods

        private void ShipsFuneral(Cell Indexes, Ship ship)
        {
            switch (ship.DecksCount)
            {
                default: break;
                case 1:
                    OneDeckShip--;
                    break;
                case 2:
                    TwoDeckShip--;
                    break;
                case 3:
                    ThrieDeckShip--;
                    break;
                case 4:
                    FourDeckShip--;
                    break;
            }

            int Y_Axis_ShipsCount = Indexes.Y + 1;
            int X_Axis_ShipsCount = Indexes.X + ship.DecksCount - ship.NumberOfHintDeck;

            int FirstDeckIn_Y_Axis = Indexes.Y - 1;
            int FirstDeckIn_X_Axis = Indexes.X - 1 - ship.NumberOfHintDeck;

            if (ship.isHorizontal is false)
            {
                FirstDeckIn_Y_Axis = Indexes.Y - 1 - ship.NumberOfHintDeck;
                FirstDeckIn_X_Axis = Indexes.X - 1;

                Y_Axis_ShipsCount = Indexes.Y + ship.DecksCount - ship.NumberOfHintDeck;
                X_Axis_ShipsCount = Indexes.X + 1;
            }

            for (int N = FirstDeckIn_Y_Axis; N <= Y_Axis_ShipsCount; N++)
            {
                for (int M = FirstDeckIn_X_Axis; M <= X_Axis_ShipsCount; M++)
                {
                    if (N == 11 || N == -1) continue;
                    if (M == 11 || M == -1) break;
                    if (field[N, M] == ShipsMark.DeadShip) continue;
                    field[N, M] = ShipsMark.Missed;
                }
            }

        }

        private void MarkTheShip(Cell Indexes, string Mark)
        {
            field[Indexes.Y, Indexes.X] = Mark;
        }

        private bool IsKilled(Cell Indexes, Ship ship)
        {
            MarkTheShip(Indexes, ShipsMark.DeadShip);

            for (int i = 0; i < ship.DecksCount; i++)
            {
                int I = Indexes.Y;
                int J = Indexes.X;

                _ = ship.isHorizontal is true
                    ? J = Indexes.X - ship.NumberOfHintDeck + i
                    : I = Indexes.Y - ship.NumberOfHintDeck + i;

                if (field[I, J] is ShipsMark.DeadShip)
                    continue;

                return false;

            }

            return true;
        }

        private bool IsMissed(Cell indexes)
        {
            if (field[indexes.Y, indexes.X] is ShipsMark.Ship)
                return false;
            return true;
        }

        private int SearchTheFirstShipsDeck(Cell indexes)
        {
            int firstDeck = 0;
            bool Direction = DeterminingTheDirection(indexes.Y, indexes.X);

            for (int k = -1; k >= -4; k--)
            {
                int firstIndex = indexes.Y;
                int secondIndex = indexes.X;

                _ = Direction is true ? secondIndex = indexes.X + k : firstIndex = indexes.Y + k;

                if (secondIndex < 0 || firstIndex < 0) break;

                if (String.IsNullOrEmpty(field[firstIndex, secondIndex]) ||
                    (field[firstIndex, secondIndex] == ShipsMark.Missed))
                    break;

                firstDeck++;
            }

            return firstDeck;
        }

        private int CountingDecks(Cell indexes)
        {
            bool Direction = DeterminingTheDirection(indexes.Y, indexes.X);
            int GeneralDeksCountInShip = 1;

            for (int k = 1; k <= 4; k++)
            {
                int firstIndex = indexes.Y;
                int secondIndex = indexes.X;

                _ = Direction is true ? secondIndex = indexes.X + k : firstIndex = indexes.Y + k;

                if (secondIndex > 10 || firstIndex > 10) break;

                if (String.IsNullOrEmpty(field[firstIndex, secondIndex]) ||
                    (field[firstIndex, secondIndex] == ShipsMark.Missed))
                    break;

                GeneralDeksCountInShip++;
            }

            for (int k = -1; k >= -4; k--)
            {
                int firstIndex = indexes.Y;
                int secondIndex = indexes.X;

                _ = Direction is true ? secondIndex = indexes.X + k : firstIndex = indexes.Y + k;

                if (secondIndex < 0 || firstIndex < 0) break;

                if (String.IsNullOrEmpty(field[firstIndex, secondIndex]) ||
                    (field[firstIndex, secondIndex] == ShipsMark.Missed))
                    break;

                GeneralDeksCountInShip++;
            }


            return GeneralDeksCountInShip;

        }

        private bool DeterminingTheDirection(int firstIndex, int secondIndex)
        {
            bool isHorizontal = true;

            for (int i = firstIndex; i < 11; i++)
            {
                for (int j = secondIndex; j < 11; j++)
                {
                    if (field[i, j - 1] is ShipsMark.Ship || (j + 1 <= 10 && field[i, j + 1] is ShipsMark.Ship))
                        return true;
                    if (field[i, j - 1] is ShipsMark.DeadShip || (j + 1 <= 10 && field[i, j + 1] is ShipsMark.DeadShip))
                        return true;

                    if (field[i - 1, j] is ShipsMark.DeadShip || (i + 1 <= 10 && field[i + 1, j] is ShipsMark.DeadShip))
                        return false;
                    if (field[i - 1, j] is ShipsMark.Ship || (i + 1 <= 10 && field[i + 1, j] is ShipsMark.Ship))
                        return false;
                }
            }

            return isHorizontal;
        }

        private void ReduceShipsCount(int decksCount)
        {
            switch (decksCount)
            {
                default: break;
                case 1:
                    OneDeckShip--;
                    break;
                case 2:
                    TwoDeckShip--;
                    break;
                case 3:
                    ThrieDeckShip--;
                    break;
                case 4:
                    FourDeckShip--;
                    break;
            }
        }

        private void ShipsModification(Cell indexes, int CurrentDeck, int DecksCount, string mark, bool direction)
        {
            if (direction is true)
            {
                int FirstDeckOfHorizontalShip = indexes.X - CurrentDeck;
                for (int i = 0; i < DecksCount; i++)
                    field[indexes.Y, FirstDeckOfHorizontalShip + i] = mark;
            }

            if (direction is false)
            {
                int FirstDeckOfVerticalShip = indexes.Y - CurrentDeck;
                for (int i = 0; i < DecksCount; i++)
                    field[FirstDeckOfVerticalShip + i, indexes.X] = mark;
            }
        }

        private void ResetTimer() => Timer = DateTime.Now;

        #endregion
    }
}
