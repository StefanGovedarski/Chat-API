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
    }
}