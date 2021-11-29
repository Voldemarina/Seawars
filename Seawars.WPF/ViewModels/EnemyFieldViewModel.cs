using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.BL;
using Seawars.Domain.Models;
using Seawars.Infrastructure.Data;
using Seawars.WPF.Common;
using Seawars.WPF.Interfaces;
using Seawars.WPF.Model;

namespace Seawars.WPF.ViewModels
{
    public class EnemyFieldViewModel : ViewModelBase, IViewModelData
    {
        private ObservableCollection<Ship> _Ships;
        private ObservableCollection<Button> _Buttons;

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
        public Field Field { get; set; }
        public EnemyFieldViewModel()
        {
            var _ships = Enumerable.Range(0, 121).Select(i => new Ship());
            var _buttons = Enumerable.Range(0, 121).Select(i => new Button()
            {
                Border = new System.Windows.Thickness(0.3),
                Content = new System.Windows.Controls.Image()
            });

            Ships = new ObservableCollection<Ship>(_ships);
            Buttons = new ObservableCollection<Button>(_buttons);
            Field = new Field();

            Field = ComputerIntelligence.FieldAutoGeneration(Field);
            Ships = ShipsAssignment();

        }

        #region Private methods
        private ObservableCollection<Ship> ShipsAssignment()
        {
            for (int i = 0; i < 121; i++)
            {
                if (Field.field[i / 11, i % 11] is ShipsMark.Ship && Ships[i].isOnField is false)
                {
                    var ship = Field.ShowShipsOptions(new Cell(i / 11, i % 11));
                    ship.NumberOfHintDeck = 0;
                    Ships[i] = new Ship(ship);
                }
            }
            Field.OneDeckShip = 4;
            Field.TwoDeckShip = 3;
            Field.ThrieDeckShip = 2;
            Field.FourDeckShip = 1;
            return Ships;
        }
        #endregion
    }
}
