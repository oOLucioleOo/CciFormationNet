using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WebAPI.Models
{
    public class HubManager
    {
        // Singleton instance
        private readonly static Lazy<HubManager> _instance = new Lazy<HubManager>(
            () => new HubManager(GlobalHost.ConnectionManager.GetHubContext<StreamHub>()));

        private IHubContext _context;

        private HubManager(IHubContext context)
        {
            _context = context;
        }

        public static HubManager Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void StartStream()
        {
            Notify("Titre", "Message");
        }

        public void Notify(string title, string message, string type = "info")
        {
            _context.Clients.All.notify(title, message, type);
        }
    }
}