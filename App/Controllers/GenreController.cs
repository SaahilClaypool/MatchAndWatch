using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Interfaces;
using Core.UseCases.Genre;

using Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
