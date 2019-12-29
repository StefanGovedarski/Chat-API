using ChatTU.DTOs;
using ChatTU.Mappings;
using ChatTU.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace ChatTU.Controllers
{
    /// <summary>
    /// Handles all chat and messagin connected api calls.
    /// </summary>
    public class MessageController : ApiController
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;

        public MessageController(IMessageService messageService, IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN,CLIENT")]
        [Route("StartChat")]
        [ResponseType(typeof(IEnumerable<ChatData>))]
        public HttpResponseMessage StartChat([FromUri]string targetUsername, [FromUri] int messageCount)
        {
            var currentUsername = HttpContext.Current.User.Identity.Name;
            var conversation = _messageService.GetOrCreateConversation(currentUsername, targetUsername);
            var messages = _messageService.GetMessages(conversation.Id, messageCount);
            var loggedIn = _userService.GetUsersLoggedInStatus(targetUsername);
            var isFullChatHistory = _messageService.IsFullHistory(conversation.Id, messageCount);

            var respone = ChatDataMapping.ToChatDataDto(conversation, messages, currentUsername, targetUsername, loggedIn, isFullChatHistory);


            if (string.IsNullOrEmpty(targetUsername))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respone);
        }


        /// <summary>
        /// Must return the users and if there is undelivered messages from the previous conversations for the logged in user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Collection of previous chat messages.</returns>
        [HttpGet]
        [Authorize(Roles = "ADMIN,CLIENT")]
        [Route("ConversationHistory")]
        [ResponseType(typeof(IEnumerable<Conversation>))]
        public HttpResponseMessage GetConversationsHistory()
        {
            var username = HttpContext.Current.User.Identity.Name;

            if (string.IsNullOrEmpty(username))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, _messageService.GetConversationHistory(username)
                                                                                            .Select(x => ConversationMapping.ToConversationDto(x, username)));
        }

        /// <summary>
        /// Must return the chat history that the logged in user has with the client with the specified username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Collection of previous chat messages.</returns>
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("MessageHistory")]
        [ResponseType(typeof(IEnumerable<Message>))]
        public HttpResponseMessage GetMessageHistory([FromUri] int conversationId, int count)
        {
            var currentUser = HttpContext.Current.User.Identity.Name;
            var response = _messageService.GetMessages(conversationId, count);

            if (conversationId <= 0 || count <= 0)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, response.Select(x => MessageMapping.ToMessageDto(x, currentUser)));
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,CLIENT")]
        [Route("AddMessage")]
        public IHttpActionResult AddMessage([FromBody] Message model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _messageService.AddMessage(model.ConversationId, model.Sender, model.Content, model.Time);

            return Ok();
        }

        // ADMIN endpoints

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