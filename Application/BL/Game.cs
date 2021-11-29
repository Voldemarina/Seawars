using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BL
{
    public class Game
    {
        public string CryptedGameId { get; set; }

        public Field FirstUserField { get; set; }
        public Field SecondUserField { get; set; }

        public bool IsFirstUserMove { get; set; } = true;
        public bool IsFirstUserReadyToStartGame { get; set; }
        public bool IsSecondUserReadyToStartGame { get; set; }
        public bool IsGameOver { get; set; }
        public bool IsFirstUserWin { get; set; }
        public bool DidEnemyConnect { get; set; }
        public bool IsGameWithComputer { get; set; }


        public Game(string Id)
        {
            CryptedGameId = Id;

            FirstUserField = new Field();
            SecondUserField = new Field();
        }

        public Field this[int index]
            => index switch
            {
                1 => FirstUserField,
                2 => SecondUserField,
                { } => null
            };
    }
}
