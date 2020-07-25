using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccousticApi.Controllers
{
    using DbContext;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    [Route("[controller]")]
    public class AcUsersController : ControllerBase
    {
        private readonly AcUsersDbContext _context;
        private readonly ILogger<AcUsersController> _logger;

        public AcUsersController(AcUsersDbContext context , ILogger<AcUsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var acUsers = await _context.AcUsers.ToListAsync();
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
