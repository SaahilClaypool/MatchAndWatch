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
using Core.UseCases.Invite;

namespace App.Controllers {
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class JoinController : ControllerBase {

        private readonly IMediator Mediator;
        private readonly ILogger Logger;

        public JoinController(IMediator mediator, ILogger<JoinController> logger) {
            Mediator = mediator;
            Logger = logger;
        }

        [HttpPost("j/{token}")]
        public async Task<ActionResult<string>> JoinSession(
        [FromRoute] string token
        ) {
            var code = await Mediator.Send(new JoinSession.Command(token));
            return Ok(code);
        }

        [HttpPost("token/{SessionId}")]
        public async Task<ActionResult<string>> GetToken(
            [FromRoute] string SessionId
        ) {
            var code = await Mediator.Send(new GetToken.Command(SessionId));
            return Ok(code);
        }
    }
}
