using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using com.shapeandshare.therowantree.client.api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace com.shapeandshare.therowantree.client.api.Controllers
{
    [Route("[controller]")]
    public class HealthController : Controller
    {
        private readonly TrtDbContext _context;

        public HealthController(TrtDbContext context)
        {
            _context = context;
        }

        // GET: health
        [Route("")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "health", "true" };
        }

        // GET /health/plain
        [Route("plain")]
        [HttpGet]
        public string GetHealthPlain()
        {
            return "true";
        }
    }
}
