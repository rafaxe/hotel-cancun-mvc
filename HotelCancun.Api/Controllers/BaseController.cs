using System.Collections.Generic;
using HotelCancun.Business.Interfaces;
using HotelCancun.Business.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace HotelCancun.Api.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly INotifier _notifier;

        protected BaseController(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected bool ValidOperation()
        {
            return !_notifier.HasNotification();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        protected List<Notification> GetNotifications()
        {
            return _notifier.GetNotifications();
        }
    }
}