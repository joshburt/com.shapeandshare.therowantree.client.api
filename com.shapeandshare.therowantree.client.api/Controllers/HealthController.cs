using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using com.shapeandshare.therowantree.client.api.Models;

namespace com.shapeandshare.therowantree.client.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly TrtDbContext _context;
        private readonly ResponseHealth _response;

        public HealthController(TrtDbContext context, IConfiguration config)
        {
            _context = context;
            _response = new ResponseHealth(_context, config);
        }

        // GET: /health
        [Route("")]
        [HttpGet]
        public ResponseHealth Get()
        {
            return _response;
        }

        // GET /health/plain
        [Route("plain")]
        [HttpGet]
        public string GetHealthPlain()
        {
            return _response.allHealthy.ToString().ToLower();
        }
    }
}
