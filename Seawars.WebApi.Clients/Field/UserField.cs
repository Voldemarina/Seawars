using Newtonsoft.Json;
using Seawars.DAL.GamesBase;
using Seawars.Infrastructure.Encryption;
using Seawars.Interfaces.Services;

namespace Seawars.WebApi.Clients.Field
{
    public class UserField : IUserField
    {
        public string Ready(string Id, Fields fields, string Field)
        {
            if (fields is (Fields)1)
            {
                Collection.GetGame().Games[TripleDes.Decrypted(Id)].IsFirstUserReadyToStartGame = true;
                Collection.GetGame().Games[TripleDes.Decrypted(Id)].FirstUserField = JsonConvert.DeserializeObject<Application.BL.Field>(Field.Replace(" ", "+"));
                Collection.GetGame().Games[TripleDes.Decrypted(Id)].FirstUserField.ResetShips();
            }
            if (fields is (Fields)2)
            {
                Collection.GetGame().Games[TripleDes.Decrypted(Id)].IsSecondUserReadyToStartGame = true;
                Collection.GetGame().Games[TripleDes.Decrypted(Id)].SecondUserField = JsonConvert.DeserializeObject<Application.BL.Field>(Field.Replace(" ", "+"));
                Collection.GetGame().Games[TripleDes.Decrypted(Id)].SecondUserField.ResetShips();
            }

            return JsonConvert.SerializeObject(Collection.GetGame().Games[TripleDes.Decrypted(Id)]);
        }
    }
}
