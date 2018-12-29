using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace com.shapeandshare.therowantree.client.api.Controllers
{
    [Route("[controller]")]
    public class VersionController : Controller
    {
        public readonly string _version;

        public VersionController(IConfiguration config)
        {
            _version = config.GetValue<string>("API_VERSION");
        }

        // GET: /version
        [Route("")]
        [HttpGet]
        public string Get()
        {
            return _version;
        }
    }
}
