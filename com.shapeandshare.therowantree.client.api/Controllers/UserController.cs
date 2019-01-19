using System;
using Microsoft.AspNetCore.Mvc;

using com.shapeandshare.therowantree.client.api.Dtos;
using com.shapeandshare.therowantree.client.api.Models;
using System.Linq;

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
            Guid _guid = Guid.NewGuid();
            User newUser;

            try
            {
                // Create the user to get the user_id primary key
                newUser = new User
                {
                    Guid = _guid.ToString()
                };
                _context.User.Add(newUser);
                _context.SaveChanges(); // .. and commit

                // Create default user feature
                Feature newFeature = new Feature
                {
                    UserId = newUser.UserId,
                    FeatureId = 1
                };
                _context.Feature.Add(newFeature);

                // Set default user game state
                UserGameState newUserGameState = new UserGameState
                {
                    UserId = newUser.UserId,
                    ActiveFeature = 1,
                    GameTemperatureId = 1,
                    GameFireStateId = 1,
                    BuilderLevel = -1
                };
                _context.UserGameState.Add(newUserGameState);

                // Set default user feature state
                UserFeatureState newUserFeatureState = new UserFeatureState
                { 
                    UserId = newUser.UserId,
                    FeatureId = 1,
                    FeatureIndex = 1
                };
                _context.UserFeatureState.Add(newUserFeatureState);

                _context.SaveChanges(); // .. and commit
            }
            catch (Exception e)
            {
                // TODO: log exception.
                return BadRequest(new ResponseUserCreate
                {
                    Guid = _guid,
                    Message = new string[] {
                            "Unable to commit changes. :(",
                            e.InnerException.Message
                        }
                });
            }

            return Created(_guid.ToString(), new ResponseUserCreate 
                { 
                    Guid = _guid, 
                    Message = new string[] { }
                });
        }


    }
}
