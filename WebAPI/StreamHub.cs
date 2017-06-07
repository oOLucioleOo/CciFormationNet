using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using Microsoft.AspNet.SignalR;

namespace WebAPI
{
    public class StreamHub : Hub
    {
        public void SendMessage(string name, string message)
        {
            Clients.All.sendMessage(name, message);
        }
    }
}