using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.UseCases.Session;

using DTO.Session;

using MediatR;

using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<IEnumerable<Core.Models.Session>>> Index() {
            var result = await Mediator.Send(new GetAllForCurrentUser.Command());
            Logger.LogDebug("fetch session");
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SessionDTO>> Get(
            [FromRoute] string id
        ) {
            Logger.LogDebug($"fetch session with id {id}");
            var result = await Mediator.Send(new GetSession.Command(id));
            return Ok(new SessionDTO() { Id = result.Id, Genres = result.Genres, Name = result.Name });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CreateSessionResponse>> Update(
        [FromBody] CreateSessionCommand commandDTO,
        [FromRoute] string id
        ) {
            Logger.LogDebug($"Update session with id {id}");
            var command = new Update.Command(
                id,
                commandDTO.Genres,
                commandDTO.Name
            );
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CreateSessionResponse>> Create(
          [FromBody] CreateSessionCommand commandDTO
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
