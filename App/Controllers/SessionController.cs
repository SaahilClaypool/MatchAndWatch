using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.UseCases.Session;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Controllers {
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase {

        private readonly IMediator Mediator;
        private readonly ILogger Logger;

        public SessionController(IMediator mediator, ILogger<SessionController> logger) {
            Mediator = mediator;
            Logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Core.Models.Session>>> Get() {
            var result = await Mediator.Send(new GetAllForCurrentUser.Command());
            Logger.LogDebug("fetch session");
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(
          [FromBody] DTO.Session.CreateSessionCommand commandDTO
        ) {
            var command = new Create.Command(
                commandDTO.Genres,
                commandDTO.Name
            );
            Logger.LogDebug("create session");
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
