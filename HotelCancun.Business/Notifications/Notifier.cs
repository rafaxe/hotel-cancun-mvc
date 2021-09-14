using System.Collections.Generic;
using System.Linq;
using HotelCancun.Business.Interfaces;

namespace HotelCancun.Business.Notifications
{
    public class Notifier : INotifier
    {
        private readonly List<Notification> _notifications;

        public Notifier()
        {
            _notifications = new List<Notification>();
        }

        public void Handle(Notification notifications)
        {
            _notifications.Add(notifications);
        }

        public List<Notification> GetNotifications()
        {
            return _notifications;
        }

        public bool HasNotification()
        {
            return _notifications.Any();
        }
    }
}