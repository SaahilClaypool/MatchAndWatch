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

using Tmdb.Services;

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
            [FromRoute] string SessionId,
            [FromServices] MovieMeta movieMeta
        ) {
            Logger.LogDebug($"fetching new");
            var movie = await Mediator.Send(new NewRating.Command(SessionId));
            var poster = await movieMeta.PosterPath(movie.MovieId);
            // TODO: get poster from API
            return Ok(new MovieInformationResponseDTO() {
                MovieId = movie.MovieId,
                MovieTitle = movie.MovieTitle,
                PosterPartialPath = poster
            });
        }
    }
}
