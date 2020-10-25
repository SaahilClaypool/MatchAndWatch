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

    public SessionController(IMediator mediator) {
      Mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> Create(
      [FromBody] Create.Command command
    ) {
      var result = await Mediator.Send(command);
      return Ok(result);
    }
  }
}
