using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccousticApi.Controllers
{
    using System.Linq;
    using System.Threading;
    using DbContext;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    [Route("[controller]")]
    public class AcUsersController : ControllerBase
    {
        private readonly AcUsersDbContext _context;
        private readonly ILogger<AcUsersController> _logger;

        public AcUsersController(AcUsersDbContext context, ILogger<AcUsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken token)
        {
            try
            {
                var acUsers = await _context.AcUsers.ToListAsync(token);
                return Ok(acUsers);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Critical, e, "Critical exception during [Get] AcUsers");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("findby/{startingLetters}")]
        public async Task<IActionResult> FindByEmail([FromRoute]string startingLetters, CancellationToken token)
        {
            try
            {
                var acUsers = await _context.AcUsers.Where(acUser => acUser.Email.StartsWith(startingLetters))
                    .OrderBy(acUser => acUser.Email).ToListAsync(token);
                return Ok(acUsers);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Critical, e, "Critical exception during [Get] AcUsers");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
