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
            Key = key ?? throw new ArgumentNullException("Key");
            Message = message ?? throw new ArgumentNullException("Message");
        }

        public Notification(string key, string message, string expected) : this(key, message)
        {
            Expected = expected ?? throw new ArgumentNullException("Expected");
        }

        public string Key { get; set; }
        public string Message { get; set; }
        public string Expected { get; set; }
    }
}
