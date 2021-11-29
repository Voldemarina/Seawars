using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seawars.WPF.Common.Data
{
    public static class PathToShipContent
    {
        public static Dictionary<int, string> HorizontalShips { get; } = new Dictionary<int, string>()
        {
            { 1, "/Seawars.WPF;component/Resources/Images/HorizontalShips/OneDeckShip.png"},
            { 2, "/Seawars.WPF;component/Resources/Images/HorizontalShips/FirstDeck_DoubleDeckShip.png" },
            { 3, "/Seawars.WPF;component/Resources/Images/HorizontalShips/SecondDeck_DoubleDeckShip.png"},
            { 4, "/Seawars.WPF;component/Resources/Images/HorizontalShips/FirstDeck_ThrieDeckShip.png"},
            { 5, "/Seawars.WPF;component/Resources/Images/HorizontalShips/SecondDeck_ThrieDeckShip.png"},
            { 6, "/Seawars.WPF;component/Resources/Images/HorizontalShips/ThirdDeck_ThrieDeckShip.png"},
            { 7, "/Seawars.WPF;component/Resources/Images/HorizontalShips/FirstDeck_FourDeckShip.png"},
            { 8, "/Seawars.WPF;component/Resources/Images/HorizontalShips/SecondDeck_FourDeckShip.png"},
            { 9, "/Seawars.WPF;component/Resources/Images/HorizontalShips/ThrieDeck_FourDeckShip.png"},
            { 10,"/Seawars.WPF;component/Resources/Images/HorizontalShips/FourDeck_FourDeckShip.png"},
        };
        public static Dictionary<int, string> VerticalShips { get; } = new Dictionary<int, string>()
        {
            { 1, "/Seawars.WPF;component/Resources/Images/VerticalShips/OneDeckShip.png"},
            { 2, "/Seawars.WPF;component/Resources/Images/VerticalShips/FirstDeck_DoubleDeckShip.png"},
            { 3, "/Seawars.WPF;component/Resources/Images/VerticalShips/SecondDeck_DoubleDeckShip.png"},
            { 4, "/Seawars.WPF;component/Resources/Images/VerticalShips/FirstDeck_ThrieDeckShip.png"},
            { 5, "/Seawars.WPF;component/Resources/Images/VerticalShips/SecondDeck_ThrieDeckShip.png"},
            { 6, "/Seawars.WPF;component/Resources/Images/VerticalShips/ThirdDeck_ThrieDeckShip.png"},
            { 7, "/Seawars.WPF;component/Resources/Images/VerticalShips/FirstDeck_FourDeckShip.png"},
            { 8, "/Seawars.WPF;component/Resources/Images/VerticalShips/SecondDeck_FourDeckShip.png"},
            { 9, "/Seawars.WPF;component/Resources/Images/VerticalShips/ThrieDeck_FourDeckShip.png"},
            { 10,"/Seawars.WPF;component/Resources/Images/VerticalShips/FourDeck_FourDeckShip.png"},
        };
        public static Dictionary<int, string> Vertical_Dead_Ships { get; } = new Dictionary<int, string>()
        {
            { 1, "/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/OneDeckShip.png"},
            { 2, "/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/FirstDeck_DoubleDeckShip.png"},
            { 3, "/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/SecondDeck_DoubleDeckShip.png"},
            { 4, "/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/FirstDeck_ThrieDeckShip.png"},
            { 5, "/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/SecondDeck_ThrieDeckShip.png"},
            { 6, "/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/ThirdDeck_ThrieDeckShip.png"},
            { 7, "/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/FirstDeck_FourDeckShip.png"},
            { 8, "/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/SecondDeck_FourDeckShip.png"},
            { 9, "/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/ThrieDeck_FourDeckShip.png"},
            { 10,"/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/FourDeck_FourDeckShip.png"},
        };
        public static Dictionary<int, string> Horizontal_Dead_Ships { get; } = new Dictionary<int, string>()
        {
            { 1, "/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/OneDeckShip.png"},
            { 2, "/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/FirstDeck_DoubleDeckShip.png"},
            { 3, "/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/SecondDeck_DoubleDeckShip.png"},
            { 4, "/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/FirstDeck_ThrieDeckShip.png"},
            { 5, "/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/SecondDeck_ThrieDeckShip.png"},
            { 6, "/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/ThirdDeck_ThrieDeckShip.png"},
            { 7, "/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/FirstDeck_FourDeckShip.png"},
            { 8, "/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/SecondDeck_FourDeckShip.png"},
            { 9, "/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/ThrieDeck_FourDeckShip.png"},
            { 10,"/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/FourDeck_FourDeckShip.png"},
        };
        #region Path
        public static string EmptyCell { get; } = "/Seawars.WPF;component/Resources/Images/HorizontalShips/EmptyCell.png";
        public static string KilledShip { get; } = "/Seawars.WPF;component/Resources/Images/HorizontalShips/Vertical_KilledShip.png";
        public static string MissedMark { get; } = "/Seawars.WPF;component/Resources/Images/HorizontalShips/MissedMark.png";
        #region Horizontal
        public static string OneDeckShip { get; } = "/Seawars.WPF;component/Resources/Images/HorizontalShips/OneDeckShip.png";
        public static string TwoDeckShip_FirstDeck { get; } = "/Seawars.WPF;component/Resources/Images/HorizontalShips/FirstDeck_DoubleDeckShip.png";
        public static string TwoDeckShip_SecondDeck { get; } = "/Seawars.WPF;component/Resources/Images/HorizontalShips/SecondDeck_DoubleDeckShip.png";
        public static string ThrieDeckShip_FirstDeck { get; } = "/Seawars.WPF;component/Resources/Images/HorizontalShips/FirstDeck_ThrieDeckShip.png";
        public static string ThrieDeckShip_SecondDeck { get; } = "/Seawars.WPF;component/Resources/Images/HorizontalShips/SecondDeck_ThrieDeckShip.png";
        public static string ThrieDeckShip_ThirdDeck { get; } = "/Seawars.WPF;component/Resources/Images/HorizontalShips/ThirdDeck_ThrieDeckShip.png";
        public static string FourDeckShip_FirstDeck { get; } = "/Seawars.WPF;component/Resources/Images/HorizontalShips/FirstDeck_FourDeckShip.png";
        public static string FourDeckShip_SecondDeck { get; } = "/Seawars.WPF;component/Resources/Images/HorizontalShips/SecondDeck_FourDeckShip.png";
        public static string FourDeckShip_ThirdDeck { get; } = "/Seawars.WPF;component/Resources/Images/HorizontalShips/ThrieDeck_FourDeckShip.png";
        public static string FourDeckShip_FourDeck { get; } = "/Seawars.WPF;component/Resources/Images/HorizontalShips/FourDeck_FourDeckShip.png";
        #endregion
        #region Vertical
        public static string Vertical_OneDeckShip { get; } = "/Seawars.WPF;component/Resources/Images/VerticalShips/OneDeckShip.png";
        public static string Vertical_TwoDeckShip_FirstDeck { get; } = "/Seawars.WPF;component/Resources/Images/VerticalShips/FirstDeck_DoubleDeckShip.png";
        public static string Vertical_TwoDeckShip_SecondDeck { get; } = "/Seawars.WPF;component/Resources/Images/VerticalShips/SecondDeck_DoubleDeckShip.png";
        public static string Vertical_ThrieDeckShip_FirstDeck { get; } = "/Seawars.WPF;component/Resources/Images/VerticalShips/FirstDeck_ThrieDeckShip.png";
        public static string Vertical_ThrieDeckShip_SecondDeck { get; } = "/Seawars.WPF;component/Resources/Images/VerticalShips/SecondDeck_ThrieDeckShip.png";
        public static string Vertical_ThrieDeckShip_ThirdDeck { get; } = "/Seawars.WPF;component/Resources/Images/VerticalShips/ThirdDeck_ThrieDeckShip.png";
        public static string Vertical_FourDeckShip_FirstDeck { get; } = "/Seawars.WPF;component/Resources/Images/VerticalShips/FirstDeck_FourDeckShip.png";
        public static string Vertical_FourDeckShip_SecondDeck { get; } = "/Seawars.WPF;component/Resources/Images/VerticalShips/SecondDeck_FourDeckShip.png";
        public static string Vertical_FourDeckShip_ThirdDeck { get; } = "/Seawars.WPF;component/Resources/Images/VerticalShips/ThrieDeck_FourDeckShip.png";
        public static string Vertical_FourDeckShip_FourDeck { get; } = "/Seawars.WPF;component/Resources/Images/VerticalShips/FourDeck_FourDeckShip.png";
        #endregion
        #region HorizontalDead
        public static string Dead_OneDeckShip { get; } = "/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/OneDeckShip.png";
        public static string Dead_TwoDeckShip_FirstDeck { get; } = "/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/FirstDeck_DoubleDeckShip.png";
        public static string Dead_TwoDeckShip_SecondDeck { get; } = "/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/SecondDeck_DoubleDeckShip.png";
        public static string Dead_ThrieDeckShip_FirstDeck { get; } = "/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/FirstDeck_ThrieDeckShip.png";
        public static string Dead_ThrieDeckShip_SecondDeck { get; } = "/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/SecondDeck_ThrieDeckShip.png";
        public static string Dead_ThrieDeckShip_ThirdDeck { get; } = "/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/ThirdDeck_ThrieDeckShip.png";
        public static string Dead_FourDeckShip_FirstDeck { get; } = "/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/FirstDeck_FourDeckShip.png";
        public static string Dead_FourDeckShip_SecondDeck { get; } = "/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/SecondDeck_FourDeckShip.png";
        public static string Dead_FourDeckShip_ThirdDeck { get; } = "/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/ThrieDeck_FourDeckShip.png";
        public static string Dead_FourDeckShip_FourDeck { get; } = "/Seawars.WPF;component/Resources/Images/Horizontal_DeadShip/FourDeck_FourDeckShip.png";
        #endregion
        #region VerticalDead
        public static string Vertical_Dead_OneDeckShip { get; } = "/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/OneDeckShip.png";
        public static string Vertical_Dead_TwoDeckShip_FirstDeck { get; } = "/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/FirstDeck_DoubleDeckShip.png";
        public static string Vertical_Dead_TwoDeckShip_SecondDeck { get; } = "/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/SecondDeck_DoubleDeckShip.png";
        public static string Vertical_Dead_ThrieDeckShip_FirstDeck { get; } = "/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/FirstDeck_ThrieDeckShip.png";
        public static string Vertical_Dead_ThrieDeckShip_SecondDeck { get; } = "/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/SecondDeck_ThrieDeckShip.png";
        public static string Vertical_Dead_ThrieDeckShip_ThirdDeck { get; } = "/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/ThirdDeck_ThrieDeckShip.png";
        public static string Vertical_Dead_FourDeckShip_FirstDeck { get; } = "/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/FirstDeck_FourDeckShip.png";
        public static string Vertical_Dead_FourDeckShip_SecondDeck { get; } = "/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/SecondDeck_FourDeckShip.png";
        public static string Vertical_Dead_FourDeckShip_ThirdDeck { get; } = "/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/ThrieDeck_FourDeckShip.png";
        public static string Vertical_Dead_FourDeckShip_FourDeck { get; } = "/Seawars.WPF;component/Resources/Images/Vertical_DeadShip/FourDeck_FourDeckShip.png";
        #endregion
        #endregion
    }
}
