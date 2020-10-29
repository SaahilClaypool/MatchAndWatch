using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core.UseCases.Session;

namespace App.Controllers {
  [Authorize]
  [ApiController]
  [Route("[controller]")]
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
      [FromBody] Create.Command command
    ) {
      var result = await Mediator.Send(command);
      Logger.LogDebug("create session");
      return Ok(result);
    }
  }
}
