using RestMinSharp.Notifications;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RestMinSharp.Results
{
    public class MemoryStreamRequestResult
    {
        public bool IsAuthorized { get; internal set; }
        public List<Notification> Notifications { get; set; } = new List<Notification>();
        public bool HasNotifications { get => Notifications.Any(); }
        public MemoryStream Stream { get; set; }
    }
}
