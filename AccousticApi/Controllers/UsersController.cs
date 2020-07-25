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
        private readonly int _pageSize = 10;

        //TODO: common error handler

        public AcUsersController(AcUsersDbContext context, ILogger<AcUsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("count/{searchText}")]
        public async Task<IActionResult> Get([FromRoute] string searchText, CancellationToken token)
        {
            var count = await _context.AcUsers.CountAsync(acUser => acUser.Email.StartsWith(searchText), token);
            return Ok(count);
        }

        [HttpGet]
        [Route("search/{searchText}/{page}")]
        public async Task<IActionResult> Get([FromRoute] string searchText, [FromRoute] int page,
            CancellationToken token)
        {
            return await this.TryHandleRequest(async () =>
            {
                var _offset = page * _pageSize;
                var acUsers = await _context.AcUsers.Where(acUser => acUser.Email.StartsWith(searchText)).Skip(_offset)
                    .Take(_pageSize).ToListAsync(token);
                return Ok(acUsers);
            });
        }

        [HttpGet]
        [Route("findBy/{startingLetters}")]
        public async Task<IActionResult> FindByEmail([FromRoute] string startingLetters, CancellationToken token)
        {
            return await this.TryHandleRequest(async () =>
            {
                var acUsers = await _context.AcUsers.Where(acUser => acUser.Email.StartsWith(startingLetters))
                    .OrderBy(acUser => acUser.Email).Take(10).ToListAsync(token);
                return Ok(acUsers);
            });
        }

        private async Task<IActionResult> TryHandleRequest(Func<Task<IActionResult>> fn)
        {
            try
            {
                return await fn();
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Critical, e, "Critical exception!!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
