using System.Collections.Generic;
using HotelCancun.Business.Notifications;

namespace HotelCancun.Business.Interfaces
{
    public interface INotifier
    {
        bool HasNotification();
        List<Notification> GetNotifications();
        void Handle(Notification notifications);
    }
}