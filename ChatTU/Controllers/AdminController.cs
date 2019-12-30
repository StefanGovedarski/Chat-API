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
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;

        public AdminController(IMessageService messageService, IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        //user admin endpoints

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
        [Route("RemoveUser")]
        public IHttpActionResult DeleteUser([FromUri]string username, [FromUri] int id)
        {
            if (string.IsNullOrEmpty(username) && id <= 0)
            {
                return BadRequest("Username or id has to be provided");
            }

            _userService.ADMIN_DeleteUser(username, id);

            return Ok();
        }

        // Messages admin endpoints
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("GetConversations")]
        public IEnumerable<Conversation> GetAllConversationsForUser([FromUri] string username)
        {
            return _messageService.ADMIN_GetConvesationsForUser(username).Select(x => ConversationMapping.ToConversationDto(x, username));
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("GetMessages")]
        public IEnumerable<Message> GetAllMessagesForConversation([FromUri] int conversationId)
        {
            return _messageService.ADMIN_GetMessagesForConversation(conversationId).Select(x => MessageMapping.ToMessageDto(x, HttpContext.Current.User.Identity.Name));
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        [Route("EditMessage")]
        public IHttpActionResult EditMessage([FromBody] int messageId, [FromBody] string content)
        {

            _messageService.ADMIN_EditMessage(messageId, content);
            return Ok();
        }

        // This endpoints deletes a specific message from the system.
        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        [Route("RemoveMessage")]
        public IHttpActionResult DeleteMessage([FromUri] int messageId)
        {
            _messageService.ADMIN_DeleteMessage(messageId);

            return Ok();
        }

        // This endpoint deletes a specific conversation with all messages connected to it.
        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        [Route("RemoveConversation")]
        public IHttpActionResult DeleteConversation([FromUri] int conversationId)
        {
            _messageService.ADMIN_DeleteConversation(conversationId);

            return Ok();
        }
    }
}
