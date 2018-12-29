using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace com.shapeandshare.therowantree.client.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        public readonly string _version;

        public VersionController(IConfiguration config)
        {
            _version = config.GetValue<string>("API_VERSION");
        }

        // GET: /api/version
        [Route("")]
        [HttpGet]
        public string Get()
        {
            return _version;
        }
    }
}
