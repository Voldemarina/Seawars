using System;

namespace Seawars.Infrastructure.Extentions
{
    public static class ExtentionsForCell
    {
        public static (int, int) ConvertCellToIndexes(this int self)
        {
            return (self / 11, self % 11);
        }
        public static int ConverIndexToCell(this int number, int i, int j)
        {
            return i * 11 + j;
        }
        public static int DetermineCellNumber(this object number)
        {
            string CellsNumber = "";
            int NewCell = 0;

            for (int i = 1; i < number.ToString().Length; i++)
                CellsNumber += number.ToString()[i];

            bool isParsed = int.TryParse(CellsNumber, out NewCell);
            if (isParsed is false) return -1;

            return NewCell;
        }
        public static bool IsAllElementsIsNull<T>(this object number, params T[] elems)
        {
            for (int i = 0; i < elems.Length; i++)
                if (Convert.ToInt32(elems[i]) != 0 || elems[i] == null)
                    return false;
            return true;
        }
    }
}
