using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using com.shapeandshare.therowantree.client.api.Models;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace com.shapeandshare.therowantree.client.api.Controllers
{
    [Route("[controller]")]
    public class HealthController : Controller
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
        public JsonResult Get()
        {
            return Json(_response);
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
