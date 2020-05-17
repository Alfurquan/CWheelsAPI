using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWheelsAPI.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CWheelsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BuggyController : ControllerBase
    {
        public BuggyController()
        {
        }

        [HttpGet("notfound")]
        public IActionResult GetNotFoundRequest()
        {
            return Ok();
        }
        [HttpGet("servererror")]
        public IActionResult GetServerError()
        {
            return Ok();
        }
        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("badrequest/{id}")]
        public IActionResult GetNotFoundRequest(int id)
        {
            return Ok();
        }
    }
}