using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Seawars.DAL.GamesBase;
using Seawars.Infrastructure.Encryption;

namespace Seawars.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameStateController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get([FromHeader] string Id) =>
            JsonConvert.SerializeObject(Collection.GetGame().Games[TripleDes.Decrypted(Id)]);
    }
}
