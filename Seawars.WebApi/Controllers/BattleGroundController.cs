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
    public class BattleGroundController : ControllerBase
    {
        private readonly IBattleGround _bg;

        public BattleGroundController(IBattleGround _bg) => this._bg = _bg;

        [HttpGet("Attack")]
        public ActionResult<string> Attack([FromHeader] string Id, Fields fields, int Cell) => _bg.Attack(Id, fields, Cell);

        [HttpGet("CanAttack")]
        public ActionResult<bool> CanAttack([FromHeader] string Id, Fields fields, int Cell) => _bg.CanAttack(Id, fields, Cell);
    }
}
