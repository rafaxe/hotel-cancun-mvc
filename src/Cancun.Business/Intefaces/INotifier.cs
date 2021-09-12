using System.Collections.Generic;
using Cancun.Business.Notifications;

namespace Cancun.Business.Intefaces
{
    public interface INotifier
    {
        bool HasNotification();
        List<Notification> GetNotifications();
        void Handle(Notification notifications);
    }
}