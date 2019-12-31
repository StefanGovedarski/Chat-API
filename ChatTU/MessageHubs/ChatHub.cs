using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ChatTU.DTOs;
using ChatTU.Mappings;
using ChatTU.Services.Interfaces;
using Microsoft.AspNet.SignalR;

namespace ChatTU.MessageHubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        static List<UserDetail> ConnectedUsers = new List<UserDetail>();

        public ChatHub(IMessageService messageService, IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        public void Connect(string UserName)
        {
            var id = Context.ConnectionId;

            if (!ConnectedUsers.Any(x =>x.ConnectionId == id))
            {
                ConnectedUsers.Add(new UserDetail { ConnectionId = id, Username = UserName});
            }
        }

        public void AddMessage(Message message)
        {
            string username = _messageService.GetUserNameForConversation(message.ConversationId, HttpContext.Current.User.Identity.Name);

            _messageService.AddMessage(message.ConversationId, message.Sender, message.Content, message.Time);

            var toUserConn = ConnectedUsers.FirstOrDefault(x => x.Username == username).ConnectionId;

            // Call the MessageAdded method to update clients.
            Clients.Client(toUserConn).Invoke("AddMessage", message);
        }

        public void SaveFile(HttpPostedFile file, string currentUsername, int conversationId)
        {
            var messageFile = _messageService.SaveFile(file, currentUsername, conversationId);
            Clients.All.Invoke("SaveFile", MessageMapping.ToMessageDto(messageFile, currentUsername));
        }

        public override Task OnConnected()
        {
            return _userService.MarkLoggedInStatusAs(HttpContext.Current.User.Identity.Name, true);
        }
        public override Task OnReconnected()
        {
            return _userService.MarkLoggedInStatusAs(HttpContext.Current.User.Identity.Name, true);
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            return _userService.MarkLoggedInStatusAs(HttpContext.Current.User.Identity.Name, false);
        }
    }

    public class UserDetail
    {
        public string Username { get; set; }
        public string ConnectionId { get; set; }
    }
}