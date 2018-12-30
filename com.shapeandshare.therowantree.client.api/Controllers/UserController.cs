using System;
using Microsoft.AspNetCore.Mvc;

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
                newUser = new User
                {
                    Guid = _guid.ToString()
                };
                _context.User.Add(newUser);
                _context.SaveChanges();
             }
            catch (Exception e) {
                // TODO: log exception.
                return BadRequest(new ResponseUserCreate 
                    {
                        Guid = _guid,
                        Message=new string[] { 
                            "Unable to create user.",
                            e.InnerException.Message 
                        } 
                    });
            }

            try
            {
                Feature newFeature = new Feature
                {
                    UserId = newUser.UserId,
                    FeatureId = _context.FeatureType.FirstOrDefault(ft => ft.FeatureId == 1).FeatureId
                };
                _context.Feature.Add(newFeature);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                // TODO: log exception.
                return BadRequest(new ResponseUserCreate 
                    { 
                        Guid = _guid, 
                        Message = new string[] {
                            String.Format("Unable to add features to new user, userid:({0}).", newUser.UserId),
                            e.InnerException.Message
                        } 
                    });
            }

            try
            {
                UserGameState newUserGameState = new UserGameState
                {
                    UserId = newUser.UserId,
                    ActiveFeature = 1,
                    GameTemperatureId = 1,
                    GameFireStateId = 1,
                    BuilderLevel = -1
                };
                _context.UserGameState.Add(newUserGameState);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                // TODO: log exception.
                return BadRequest(new ResponseUserCreate 
                    { 
                        Guid = _guid, 
                        Message = new string[] {
                            String.Format("Unable to add game state to new user, userid:({0}).", newUser.UserId),
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
