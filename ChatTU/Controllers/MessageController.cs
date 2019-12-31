using ChatTU.DTOs;
using ChatTU.Mappings;
using ChatTU.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        [Authorize(Roles = "ADMIN,CLIENT")]
        [Route("MessageHistory")]
        [ResponseType(typeof(IEnumerable<Message>))]
        public HttpResponseMessage GetMessageHistory([FromUri] int conversationId, int count)
        {
            var currentUser = HttpContext.Current.User.Identity.Name;
            var response = _messageService.GetMessages(conversationId, count);

            if (conversationId <= 0 || count <= 0)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            return Request.CreateResponse(HttpStatusCode.OK, response.Select(x => MessageMapping.ToMessageDto(x, currentUser)));
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

        [HttpPost]
        [Route("SaveFile")]
        [Authorize(Roles = "ADMIN,CLIENT")]
        public HttpResponseMessage SaveFile([FromUri] int conversationId)
        {
            var currentUsername = HttpContext.Current.User.Identity.Name;

            //Create HTTP Response.
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            //Check if Request contains File.
            if (HttpContext.Current.Request.Files.Count == 0)
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var messageEntity = _messageService.SaveFile(HttpContext.Current.Request.Files[0], currentUsername, conversationId);

            return Request.CreateResponse(HttpStatusCode.OK, new { message = MessageMapping.ToMessageDto(messageEntity, currentUsername) });
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN,CLIENT")]
        [Route("GetFile")]
        public HttpResponseMessage GetFile([FromUri] int messageId)
        {
            //Create HTTP Response.
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            //Get the File data from Database.
            var file = _messageService.GetFile(messageId);

            //Set the Response Content.
            response.Content = new ByteArrayContent(file.Data);

            //Set the Response Content Length.
            response.Content.Headers.ContentLength = file.Data.LongLength;

            //Set the Content Disposition Header Value and FileName.
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = file.Name;

            //Set the File Content Type.
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return response;
        }

        // This endpoints deletes a specific message from the system.
        [HttpGet]
        [Authorize(Roles = "ADMIN,CLIENT")]
        [Route("DeleteMessage")]
        public IHttpActionResult DeleteMessage([FromUri] int messageId)
        {
            _messageService.DeleteMessage(messageId);

            return Ok();
        }
    }
}