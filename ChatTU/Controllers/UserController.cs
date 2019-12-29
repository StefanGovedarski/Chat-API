using ChatTU.DTOs;
using ChatTU.Infrastructure;
using ChatTU.Mappings;
using ChatTU.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ChatTU.Controllers
{
        [EnableCors("*", "*", "*")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN, CLIENT")]
        [Route("FindUsers")]
        public IEnumerable<User> FindUsers([FromUri] string searchQuery)
        {
            var username = HttpContext.Current.User.Identity.Name;

            return _userService.FindUsers(searchQuery.Trim(), username).Select(x => UserMappings.ToUserDto(x));
        }

        //ADMIN endpoints

        // Get data for a specific user from the system
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("GetUser")]
        public User GetUser([FromUri]string username, [FromUri] int id)
        {
            return UserMappings.ToUserDto(_userService.ADMIN_GetUser(username, id));
        }

        // Gets data for all users in the system
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("GetUsers")]
        public IEnumerable<User> GetAllUsers()
        {
            return _userService.ADMIN_GetAllUsers().Select(x => UserMappings.ToUserDto(x));
        }

        // This endpoints deletes a specific user with all conversation and message data.
        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        [Route("Remove")]
        public IHttpActionResult DeleteUser([FromUri]string username, [FromUri] int id)
        {
            if (string.IsNullOrEmpty(username) && id <= 0)
            {
                return BadRequest("Username or id has to be provided");
            }

            _userService.ADMIN_DeleteUser(username, id);

            return Ok();
        }
    }
}