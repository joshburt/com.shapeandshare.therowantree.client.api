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

        [HttpGet]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Route("create")]
        public IActionResult UserCreate()
        {
            User newUser = new User
            {
                Guid = Guid.NewGuid().ToString()
            };

            try
            {
                _context.User.Add(newUser);
                _context.SaveChanges();
            }
            catch (Exception e) {
                // TODO: log exception.
                return BadRequest(new ResponseUserCreate { Guid=Guid.Empty, Message="Failed to create new user." });
            }
            return Created(newUser.Guid, new ResponseUserCreate { Guid = Guid.Parse(newUser.Guid), Message="" });
        }
    }
}
