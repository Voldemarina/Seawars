using System.Linq;
using Newtonsoft.Json;
using Seawars.DAL.GamesBase;
using Seawars.Domain.Models;
using Seawars.Infrastructure.Encryption;
using Seawars.Infrastructure.Extentions;
using Seawars.Infrastructure.Validation;
using Seawars.Interfaces.Services;

namespace Seawars.WebApi.Clients.Game
{
    public class BattleGround : IBattleGround
    {
        public string Attack(string Id, Fields fields, int Cell)
        {
            Collection collection = Collection.GetGame();//99

            var DecryptedId = TripleDes.Decrypted(Id);// 143 140

            bool isCorectId = Validator.DoesTheGameExist(collection.Games.Keys.ToArray(), DecryptedId);//68 754

            if (isCorectId is false) return null;

            var _Cell = Cell.ConvertCellToIndexes();

            var Field = collection.Games[DecryptedId][(int)fields].Attack(new Cell(_Cell.Item1, _Cell.Item2));

            if (Field.Item2 is true)//isMissed
                collection.Games[DecryptedId].IsFirstUserMove = !collection.Games[DecryptedId].IsFirstUserMove;

            return JsonConvert.SerializeObject(Field.Item1);
        }

        public bool CanAttack(string Id, Fields fields, int Cell)
        {
            return Collection
                .GetGame().Games[TripleDes.Decrypted(Id)][(int)fields]
                .CanAttackCell(Cell);
        }
    }
}
