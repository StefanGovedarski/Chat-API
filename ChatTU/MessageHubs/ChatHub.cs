using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using ChatTU.DTOs;
using ChatTU.Mappings;
using ChatTU.Services.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ChatTU.MessageHubs
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        private readonly ILifetimeScope _lifetimescope;
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        static List<UserDetail> ConnectedUsers = new List<UserDetail>();

        public ChatHub(IMessageService messageService, IUserService userService, ILifetimeScope lifetimeScope)
        {
            _lifetimescope = lifetimeScope.BeginLifetimeScope();
            _messageService = messageService;
            _userService = userService;
        }

        public void Connect(string username)
        {
            _userService.MarkLoggedInStatusAs(username, true);

            var id = Context.ConnectionId;

            if (!ConnectedUsers.Any(x => x.Username == username))
            {
                ConnectedUsers.Add(new UserDetail { ConnectionId = id, Username = username });
            }
            else
            {
                ConnectedUsers.First(x => x.Username == username).ConnectionId = id;
            }

            Clients.Caller.notifyConnected();
        }

        public void AddMessage(Message message)
        {
            string username = _messageService.GetUserNameForConversation(message.ConversationId, message.Sender);

            var messageEntity = _messageService.AddMessage(message.ConversationId, message.Sender, message.Content, message.Time);

            message = MessageMapping.ToMessageDto(messageEntity, message.Sender);

            var toUserConn = ConnectedUsers.FirstOrDefault(x => x.Username == username).ConnectionId;

            // Call addedMessage to update clients.
            Clients.Client(toUserConn).addedMessage(message);
            Clients.Caller.addedMessage(message);
        }

        public void SaveFile(Message message)
        {
            string targetUsername = _messageService.GetUserNameForConversation(message.ConversationId, message.Sender);

            var toUserConn = ConnectedUsers.FirstOrDefault(x => x.Username == targetUsername).ConnectionId;

            Clients.Client(toUserConn).savedFile(message);
            Clients.Caller.savedFile(message);
        }

        public void RemoveMessage(Message message)
        {
            string username = _messageService.GetUserNameForConversation(message.ConversationId, message.Sender);

            _messageService.DeleteMessage(message.Id);

            var toUserConn = ConnectedUsers.FirstOrDefault(x => x.Username == username).ConnectionId;

            // Call addedMessage to update clients.
            Clients.Client(toUserConn).removedMessage(message);
            Clients.Caller.removedMessage(message);
        }

        public void Disconnect(string username)
        {
            _userService.MarkLoggedInStatusAs(username, false);
            var user = ConnectedUsers.First(x => x.Username == username);
            ConnectedUsers.Remove(user);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
        public override Task OnReconnected()
        {

            return base.OnReconnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
        protected override void Dispose(bool disposing)
        {
            // Dispose the hub lifetime scope when the hub is disposed.
            if (disposing && _lifetimescope != null)
            {
                _lifetimescope.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    public class UserDetail
    {
        public string Username { get; set; }
        public string ConnectionId { get; set; }
    }
}