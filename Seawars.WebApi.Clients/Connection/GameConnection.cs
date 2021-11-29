using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Seawars.DAL.GamesBase;
using Seawars.Domain.Models;
using Seawars.Infrastructure.Encryption;
using Seawars.Infrastructure.Validation;

using Seawars.Interfaces.Services;

namespace Seawars.WebApi.Clients.Connection
{
    public class GameConnection : IConnection
    {
        public string CreateGame()
        {
            Collection collection = Collection.GetGame();

            int Id = StopWatch.TotalGamesCounnt;

            StopWatch.TotalGamesCounnt++;

            var CryptedId = TripleDes.Encrypted(Id);

            collection.Games.Add(Id, new Application.BL.Game(CryptedId));

            if(StopWatch.IsActive is not true) StopWatch.StartTimer();

            var resault = JsonConvert.SerializeObject(collection.Games[Id]);

            return resault;
           
        }

        public string JoinGame(string Id)       
        {
            Collection collection = Collection.GetGame();

            var DecryptedId = TripleDes.Decrypted(Id);

            bool isCorectId = Validator.DoesTheGameExist(collection.Games.Keys.ToArray(), DecryptedId);

            if (isCorectId is false) return null;

            if (collection.Games[DecryptedId].DidEnemyConnect is true) return null;

            collection.Games[DecryptedId].DidEnemyConnect = true;
            collection.Games[DecryptedId].IsGameWithComputer = false;

            return JsonConvert.SerializeObject(collection.Games[DecryptedId]);
        }

    }
}
