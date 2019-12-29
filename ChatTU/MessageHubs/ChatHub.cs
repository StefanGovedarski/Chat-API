using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ChatTU.Services.Interfaces;
using Microsoft.AspNet.SignalR;

namespace ChatTU.MessageHubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private string username;

        public ChatHub(IMessageService messageService, IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        //public void AddMessage(string message)
        //{
        //    var chatMessage = _messageService.CreateNewMessage("Juergen", message);
        //    // Call the MessageAdded method to update clients.
        //    Clients.All.InvokeAsync("MessageAdded", chatMessage);
        //}

        public override Task OnConnected()
        {
           return _userService.MarkLoggedInStatusAs(username, true);
	    }
        public override Task OnReconnected()
        {
            return _userService.MarkLoggedInStatusAs(username, true);
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            return _userService.MarkLoggedInStatusAs(username, false);
        }
    }
}