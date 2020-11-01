using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core.UseCases.Genre;
using Core.Interfaces;
using Infrastructure;

namespace App.Controllers {
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class GenreController : ControllerBase {

    private readonly IMediator Mediator;
    private readonly ILogger Logger;

    public GenreController(IMediator mediator, ILogger<GenreController> logger) {
      Mediator = mediator;
      Logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> Get() {

      var result = await Mediator.Send(new GetUniqueGenreNames.Command());
      return Ok(result);
    }
  }
}
