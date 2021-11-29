using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.BL;
using Seawars.Domain.Models;

namespace Seawars.WPF.ViewModels
{
    public static class ComputerIntelligence
    {
        #region Private Data 
        private static int CountOfAttackInOneDirection = 0;
        private static int AttackInOnePoint = 0;
        private static int totalShipsCount = 10;
        private static bool isHorizontal = true;
        private static bool isPositiveDirection = true;
        private static bool isKilled = false;
        private static bool isMissed = true;
        private static bool isHintButNotKilled = false;
        private static int fixed_I;
        private static int fixed_J;

        private const string Ship = "O";
        private const string KilledMark = "X";
        private const string MissedMark = " ";
        #endregion

        public static (string[,], bool) ComputerAttack(Field UserField)
        {
            bool isComputerMove = true;
            Cell indexes = new Cell();
            CountOfAttackInOneDirection = 0;
            AttackInOnePoint = 0;

            while (isComputerMove != false)
            {
                if (isHintButNotKilled is false)
                {
                    indexes = SearchRandomCell(UserField);
                }
                if (isHintButNotKilled is true)
                {
                    indexes = ChangeCellNumberToNextAttack(indexes, UserField);
                }

                isMissed = IsMissed(indexes, UserField);

                if (isMissed is true)
                {
                    AssignShipsMark(indexes, MissedMark, UserField);
                    isComputerMove = false;
                    if (CountOfAttackInOneDirection is 0)
                        isHorizontal = !isHorizontal;
                    if (CountOfAttackInOneDirection != 0)
                        isPositiveDirection = !isPositiveDirection;
                    return (UserField.field, isMissed);
                }

                AssignShipsMark(indexes, KilledMark, UserField);

                isKilled = IsKilled(indexes, UserField);

                if (isKilled is true)
                {
                    ShipsFuneral(indexes, UserField);
                    if (totalShipsCount is 0)
                        return (UserField.field, isMissed);
                    continue;
                }
                isHintButNotKilled = true;
                CountOfAttackInOneDirection++;
            }

            return (UserField.field, isMissed);
        }
        public static Field FieldAutoGeneration(Field ComputerField)
        {
            return ComputerField.FieldAutoGeneration();
        }

        #region Private Methods
        private static int GetCell(Cell index)
        {
            return index.Y * 11 + index.X;
        }
        private static Cell SearchRandomCell(Field UserField)
        {
            bool canDamage = false;
            Random random = new Random();
            Cell index = new Cell();

            while (canDamage != true)
            {
                index.Y = random.Next(1, 11);
                index.X = random.Next(1, 11);

                int Cell = GetCell(index);

                canDamage = UserField.CanAttackCell(Cell);
            }

            fixed_I = index.Y;
            fixed_J = index.X;

            return index;
        }
        private static Cell ChangeAttackDirection(Cell indexes, Cell IndexToAdd)
        {
            ChangeDirection:
            if (isHorizontal is true && isPositiveDirection is true)
            {
                if (IndexToAdd.X + 1 <= 10 && IndexToAdd.X >= 1)
                {
                    indexes.X = IndexToAdd.X + 1;
                    indexes.Y = IndexToAdd.Y;
                    isPositiveDirection = true;
                    return indexes;
                }
                isPositiveDirection = !isPositiveDirection;
            }
            if (isHorizontal is true && isPositiveDirection is false)
            {
                if (IndexToAdd.X - 1 >= 1 && IndexToAdd.X <= 10)
                {
                    indexes.X = IndexToAdd.X - 1;
                    indexes.Y = IndexToAdd.Y;
                    isPositiveDirection = false;
                    return indexes;
                }
                isPositiveDirection = !isPositiveDirection;
                goto ChangeDirection;
            }

            if (isHorizontal is false && isPositiveDirection is true)
            {
                if (IndexToAdd.Y + 1 <= 10 && IndexToAdd.Y >= 1)
                {
                    indexes.Y = IndexToAdd.Y + 1;
                    indexes.X = IndexToAdd.X;
                    isPositiveDirection = true;
                    return indexes;
                }
                isPositiveDirection = !isPositiveDirection;
            }
            if (isHorizontal is false && isPositiveDirection is false)
            {
                if (IndexToAdd.Y - 1 >= 1 && IndexToAdd.Y <= 10)
                {
                    indexes.Y = IndexToAdd.Y - 1;
                    indexes.X = IndexToAdd.X;
                    return indexes;
                }
                isPositiveDirection = !isPositiveDirection;
                goto ChangeDirection;
            }
            return indexes;
        }
        private static Cell ChangeCellNumberToNextAttack(Cell NewIndex, Field UserField)
        {
            ChangeAttack:
            Cell PreviousIndex = new Cell(fixed_I, fixed_J);

            if (CountOfAttackInOneDirection >= 1)
            {
                ChangeAttackDirection(NewIndex, NewIndex);
            }
            else
            {
                ChangeAttackDirection(NewIndex, PreviousIndex);
            }

            if (UserField.field[NewIndex.Y, NewIndex.X] is KilledMark ||
                UserField.field[NewIndex.Y, NewIndex.X] is MissedMark)
            {
                isPositiveDirection = !isPositiveDirection;
                CountOfAttackInOneDirection = 0;
                if (AttackInOnePoint is 2)
                {
                    AttackInOnePoint = 0;
                    isHorizontal = !isHorizontal;
                    goto ChangeAttack;

                }
                AttackInOnePoint++;
                goto ChangeAttack;
            }


            return NewIndex;
        }
        private static bool IsMissed(Cell indexes, Field UserField)
        {
            if (UserField.field[indexes.Y, indexes.X] is "O")
                return false;
            return true;
        }
        private static void ShipsFuneral(Cell Indexes, Field UserField)
        {
            var ship = UserField.ShowShipsOptions(Indexes);

            switch (ship.DecksCount)
            {
                default: break;
                case 1: UserField.OneDeckShip--; break;
                case 2: UserField.TwoDeckShip--; break;
                case 3: UserField.ThrieDeckShip--; break;
                case 4: UserField.FourDeckShip--; break;
            }

            int y_Axis_Ships = Indexes.Y + 1;
            int x_Axis_Ships = Indexes.X + ship.DecksCount - ship.NumberOfHintDeck;

            int n = Indexes.Y - 1;
            int m = Indexes.X - 1 - ship.NumberOfHintDeck;

            if (ship.isHorizontal is false)
            {
                n = Indexes.Y - 1 - ship.NumberOfHintDeck;
                m = Indexes.X - 1;

                y_Axis_Ships = Indexes.Y + ship.DecksCount - ship.NumberOfHintDeck;
                x_Axis_Ships = Indexes.X + 1;
            }
            for (int N = n; N <= y_Axis_Ships; N++)
            {
                for (int M = m; M <= x_Axis_Ships; M++)
                {
                    if (N == 11 || N == -1) continue;
                    if (M == 11 || M == -1) break;
                    if (UserField.field[N, M] == KilledMark) continue;
                    UserField.field[N, M] = MissedMark;
                }
            }
            totalShipsCount--;
            isHintButNotKilled = false;
        }
        private static bool IsKilled(Cell Indexes, Field UserField)
        {
            var ship = UserField.ShowShipsOptions(Indexes);

            for (int i = 0; i < ship.DecksCount; i++)
            {
                int I = Indexes.Y;
                int J = Indexes.X;

                _ = ship.isHorizontal is true ?
                    J = Indexes.X - ship.NumberOfHintDeck + i :
                    I = Indexes.Y - ship.NumberOfHintDeck + i;

                if (UserField.field[I, J] is KilledMark)
                    continue;

                return false;

            }
            return true;
        }
        private static void AssignShipsMark(Cell Indexes, string Mark, Field UserField)
        {
            UserField.field[Indexes.Y, Indexes.X] = Mark;
        }
        #endregion
    }
}
