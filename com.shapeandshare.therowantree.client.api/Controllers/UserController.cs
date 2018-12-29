using System;
using Microsoft.AspNetCore.Mvc;

using com.shapeandshare.therowantree.client.api.Models;

namespace com.shapeandshare.therowantree.client.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TrtDbContext _context;

        public UserController(TrtDbContext context)
        {
            _context = context;
        }
    }
}
