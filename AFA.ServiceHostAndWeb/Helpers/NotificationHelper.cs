using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFA.ServiceHostAndWeb.Helpers.Enums;

namespace AFA.ServiceHostAndWeb.Helpers
{
    public static class NotificationHelper
    {
        public static void SuccessNotification(this Controller context, string message, bool persistForTheNextRequest = true)
        {
            AddNotification(context, NotifyType.Success, message, persistForTheNextRequest);
        }

        public static void ErrorNotification(this Controller context, string message, bool persistForTheNextRequest = true)
        {
            AddNotification(context, NotifyType.Error, message, persistForTheNextRequest);
        }

        public static void AddNotification(this Controller context, NotifyType type, string message, bool persistForTheNextRequest)
        {

            string dataKey = string.Format("afa.notifications.{0}", type);
            if (persistForTheNextRequest)
            {
                context.TempData[dataKey] = message;
            }
            else
            {
                context.ViewData[dataKey] = message;
            }

        }
    }
}