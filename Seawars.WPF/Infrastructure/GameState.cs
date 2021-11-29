using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.BL;
using Seawars.Domain.Models;

namespace Seawars.WPF.Infrastructure
{
    public class GameState
    {
        private static GameState game = null;
        protected GameState() { }
        protected GameState(GameState game, bool CurrentUserIsHost)
        {
            this.CryptedGameId = game.CryptedGameId;
            this.DidEnemyConnect = game.DidEnemyConnect;
            this.FirstUserField = game.FirstUserField;
            this.SecondUserField = game.SecondUserField;
            this.IsFirstUserMove = game.IsFirstUserMove;
            this.IsFirstUserReadyToStartGame = game.IsFirstUserReadyToStartGame;
            this.IsSecondUserReadyToStartGame = game.IsSecondUserReadyToStartGame;
            this.IsGameOver = game.IsGameOver;
            this.IsFirstUserWin = game.IsFirstUserWin;
            this.IsGameWithComputer = game.IsGameWithComputer;
            this.CurrentUserIsHost = CurrentUserIsHost;
        }

        public static GameState GetState(bool CurrentUserIsHost = false, GameState Game = null)
        {
            if (Game != null)
            {
                game = new GameState(Game, CurrentUserIsHost);
                return game;
            }

            if (game is null) game = new GameState();

            return game;
        }

        public string CryptedGameId { get; set; }
        public bool DidEnemyConnect { get; set; } = false;
        public Field FirstUserField { get; set; } = new Field();
        public Field SecondUserField { get; set; } = new Field();

        public bool IsFirstUserMove { get; set; } = true;
        public bool IsFirstUserReadyToStartGame { get; set; }
        public bool IsSecondUserReadyToStartGame { get; set; }
        public bool IsGameOver { get; set; }
        public bool IsFirstUserWin { get; set; }
        public bool IsGameWithComputer { get; set; } = true;
        public bool CurrentUserIsHost { get; set; } = true;

        internal Field this[int index]
        {
            get
            {
                switch (index)
                {
                    default: break;
                    case 1: return FirstUserField;
                    case 2: return SecondUserField;
                }
                return null;
            }
            set
            {
                switch (index)
                {
                    default: break;
                    case 1: FirstUserField = value; break;
                    case 2: SecondUserField = value; break;
                }
            }
        }
    }
}
