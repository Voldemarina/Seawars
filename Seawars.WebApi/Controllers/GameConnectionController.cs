using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Seawars.Interfaces.Services;

namespace Seawars.WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class GameConnectionController : ControllerBase
    {
        private readonly IConnection _connection;

        public GameConnectionController(IConnection _connection) => this._connection = _connection;

        [HttpGet]
        public string HomeView() => "Good game!";

        [HttpGet("CreateGame")]
        public ActionResult<string> Create() => _connection.CreateGame();

        [HttpGet("JoinToGame")]
        public ActionResult<string> JoinAnExistingGame([FromHeader]string Id) => _connection.JoinGame(Id);
    }
}
