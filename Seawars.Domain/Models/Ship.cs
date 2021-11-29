using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seawars.Domain.Models
{
    public class Ship
    {
        public bool isOnField { get; set; } = false;
        public bool isHorizontal { get; set; } = true;
        public int NumberOfHintDeck { get; set; } = default;
        public int DecksCount { get; set; } = default;
        public Cell Position { get; set; }
        public bool isDead { get; set; } = false;

        public Ship(Ship ship)
        {
            this.isDead = ship.isDead;
            this.isOnField = ship.isOnField;
            this.isHorizontal = ship.isHorizontal;
            this.NumberOfHintDeck = ship.NumberOfHintDeck;
            this.DecksCount = ship.DecksCount;
            this.Position = ship.Position;
        }
        public Ship()
        {
            
        }
    }
}
