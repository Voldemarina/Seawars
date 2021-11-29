using System.Collections.Generic;
using Application.BL;


namespace Seawars.DAL.GamesBase
{
    public class Collection
    {
        public Dictionary<int, Game> Games { get; set; } = new();

        private static Collection _collection;
        private Collection() { }
        public static Collection GetGame() => _collection ??= _collection = new Collection();

    }
}
