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
    public class FieldCreationController : ControllerBase
    {
        private readonly IUserField _field;

        public FieldCreationController(IUserField _field) => this._field = _field;

        [HttpGet("ReadyToStart")]
        public ActionResult<string> ReadyTostart([FromHeader] string Id, Fields fields, string Field) => _field.Ready(Id, fields, Field);
    }
}
