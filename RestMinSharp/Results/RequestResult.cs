using RestMinSharp.Notifications;
using System.Collections.Generic;
using System.Linq;

namespace RestMinSharp.Results
{
    public class RequestResult<T>
    {
        public List<Notification> Notifications { get; set; } = new List<Notification>();
        public bool HasNotifications { get => Notifications.Any(); }
        public T Data { get; set; }
    }
}
