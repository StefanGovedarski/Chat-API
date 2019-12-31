using ChatTU.DTOs;
using ChatTU.Mappings;
using ChatTU.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ChatTU.Controllers
{
    [RoutePrefix("admin")]
    public class AdminController : ApiController
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        //user admin endpoints

        // Get data for a specific user from the system
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("GetUser")]
        public User GetUser([FromUri]string username, [FromUri] int id)
        {
            return UserMappings.ToUserDto(_adminService.ADMIN_GetUser(username, id));
        }

        // Edit data for a specific user from the system
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        [Route("EditUser")]
        public IHttpActionResult EditUser([FromBody] User_Admin user)
        {
            _adminService.ADMIN_EditUser(user.Id, user.Username, user.Password, user.Firstname, user.Lastname);
            return Ok();
        }

        // Gets data for all users in the system
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("GetUsers")]
        public IEnumerable<User> GetAllUsers()
        {
            return _adminService.ADMIN_GetAllUsers().Select(x => UserMappings.ToUserDto(x));
        }

        // This endpoints deletes a specific user with all conversation and message data.
        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        [Route("RemoveUser")]
        public IHttpActionResult DeleteUser([FromUri]string username, [FromUri] int id)
        {
            if (string.IsNullOrEmpty(username) && id <= 0)
            {
                return BadRequest("Username or id has to be provided");
            }

            _adminService.ADMIN_DeleteUser(username, id);

            return Ok();
        }

        // Messages admin endpoints
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("GetConversations")]
        public IEnumerable<Conversation> GetAllConversationsForUser([FromUri] string username)
        {
            return _adminService.ADMIN_GetConvesationsForUser(username).Select(x => ConversationMapping.ToConversationDto(x, username));
        }

        // Returns all messages for one specific conversation.
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("GetMessages")]
        public IEnumerable<Message> GetAllMessagesForConversation([FromUri] int conversationId)
        {
            return _adminService.ADMIN_GetMessagesForConversation(conversationId).Select(x => MessageMapping.ToMessageDto(x, HttpContext.Current.User.Identity.Name));
        }

        // Edits data for a specific message in the system.
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        [Route("EditMessage")]
        public IHttpActionResult EditMessage([FromBody] int messageId, [FromBody] string content)
        {

            _adminService.ADMIN_EditMessage(messageId, content);
            return Ok();
        }

        // This endpoint deletes a specific conversation with all messages connected to it.
        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        [Route("DeleteConversation")]
        public IHttpActionResult DeleteConversation([FromUri] int conversationId)
        {
            _adminService.ADMIN_DeleteConversation(conversationId);

            return Ok();
        }

        // This endpoints deletes a specific file from the system.
        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        [Route("DeleteFile")]
        public IHttpActionResult DeleteFile([FromUri] int fileId)
        {
            _adminService.ADMIN_DeleteFile(fileId);

            return Ok();
        }
    }
}
