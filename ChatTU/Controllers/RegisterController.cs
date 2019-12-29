using ChatTU.DTOs;
using ChatTU.Enums;
using ChatTU.Infrastructure;
using ChatTU.Services.Interfaces;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ChatTU.Controllers
{
    public class RegisterController : ApiController
    {
        private readonly IUserService _userService;

        public RegisterController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public HttpResponseMessage RegisterUser([FromBody] Registration userModel)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            //For improvement: Error handling for username duplication alert.
            _userService.Register(userModel.Username, userModel.Password, userModel.Firstname, userModel.Lastname, Roles.CLIENT.ToString());
            var token = GetLoginResponse(userModel.Username, userModel.Password);

            return Request.CreateResponse(HttpStatusCode.OK, token);
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN,CLIENT")]
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            _userService.MarkLoggedInStatusAs(HttpContext.Current.User.Identity.Name, false);

            return Ok();
        }

        //ADMIN endpoints

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [Route("RegisterAdmin")]
        public IHttpActionResult RegisterAdmin([FromBody] RegistrationAdmin userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _userService.Register(userModel.Username, userModel.Password, userModel.Firstname, userModel.Lastname, string.IsNullOrEmpty(userModel.Role) ? Roles.CLIENT.ToString() : userModel.Role);

            return Ok();
        }

        private JToken GetLoginResponse(string username, string password)
        {
            var tokenPath = Request.RequestUri.GetLeftPart(UriPartial.Authority) + "/token";
            var reqData = string.Format("grant_type=password&username={0}&password={1}", username, password);

            using (var client = new WebClient())
            {
                return JObject.Parse(client.UploadString(tokenPath, reqData));
            }
        }
    }
}
