using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using com.shapeandshare.therowantree.client.api.Dtos;
using com.shapeandshare.therowantree.client.api.Models;

namespace com.shapeandshare.therowantree.client.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly TrtDbContext _context;
        private readonly IConfiguration _config;

        public HealthController(TrtDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: /health
        [Route("")]
        [HttpGet]
        public ResponseHealth Get()
        {
            return new ResponseHealth(_context, _config);
        }

        // GET /health/plain
        [Route("plain")]
        [HttpGet]
        public string GetHealthPlain()
        {
            return new ResponseHealth(_context, _config).allHealthy.ToString().ToLower();
        }
    }
}
