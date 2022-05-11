using System;

namespace RestMinSharp.Notifications
{
    public class Notification
    {
        public Notification()
        {
        }

        public Notification(string key, string message)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public Notification(string key, string message, string expected) : this(key, message)
        {
            Expected = expected ?? throw new ArgumentNullException(nameof(expected));
        }

        public string Key { get; set; }
        public string Message { get; set; }
        public string Expected { get; set; }
    }
}
