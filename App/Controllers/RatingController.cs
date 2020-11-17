using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.UseCases.Rating;

using DTO.Rating;
using DTO.Session;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Controllers {
    [Authorize]
    [ApiController]
    [Route("api/Session/{SessionId}/[controller]")]
    public class RatingController : ControllerBase {
        private readonly ILogger<RatingController> Logger;

        public RatingController(IMediator mediator, ILogger<RatingController> logger) {
            Mediator = mediator;
            Logger = logger;
        }

        public IMediator Mediator { get; }

        [HttpGet]
        public async Task<ActionResult<MovieInformationResponseDTO>> Get(
            [FromRoute] string SessionId
        ) {
            Logger.LogDebug($"fetching new");
            var result = await Mediator.Send(new NewRating.Command(SessionId));
            // TODO: get poster from API
            return Ok(new MovieInformationResponseDTO() {
                MovieId = result.MovieId,
                MovieTitle = result.MovieTitle
            });
        }
    }
}
