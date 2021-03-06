using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.UseCases.Session;

using DTO.Rating;
using DTO.Session;

using Extensions;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using App.Helpers;

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
            var result = await Mediator.Send(new CreatedByCurrentUser.Command());
            Logger.LogDebug("fetch session");
            return Ok(result);
        }

        [HttpGet("participating")]
        public async Task<ActionResult<IEnumerable<SessionDTO>>> Participating() {
            var result = await Mediator.Send(new ParticipatedByCurrentUser.Command());
            Logger.LogDebug("fetch session where user is participating");
            var resultDTO = result.Select(session => session.ToDTO());
            return Ok(resultDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SessionDTO>> Get(
            [FromRoute] string id
        ) {
            Logger.LogDebug($"fetch session with id {id}");
            var result = await Mediator.Send(new GetSession.Command(id));
            return Ok(result.ToDTO());
        }

        [HttpGet("{id}/full")]
        public async Task<ActionResult<FullSessionDTO>> Full(
            [FromRoute] string id
        ) {
            Logger.LogDebug($"fetch session full with id {id}");
            var result = await Mediator.Send(new GetSession.Command(id) { IncludeRatings = true });
            return Ok(new FullSessionDTO() {
                Id = result.Id,
                Genres = result.Genres,
                Name = result.Name,
                Ratings = result.Ratings.Select(rating => rating.ToDTO())
            });
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
