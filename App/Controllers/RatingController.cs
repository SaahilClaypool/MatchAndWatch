using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.UseCases.Session;

using MediatR;

using Microsoft.AspNetCore.Http;
using DTO.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Controllers {
    [Authorize]
    [ApiController]
    [Route("api/Session/{[controller]")]
    public class RatingController : ControllerBase {
    }
}
