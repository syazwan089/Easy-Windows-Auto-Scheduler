using Plugin.LocalNotification;

namespace ShutdownSch.Helpers
{
    public static class NotificationHelper
    {
        static int bdNo = 0;
        public static void ShowNotification(string title,string subtitle, string message)
        {
            var request = new NotificationRequest
            {
                Title = title,
                Subtitle = subtitle,
                Description = message,
                BadgeNumber = bdNo,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddMinutes(5)
                }
            };

            LocalNotificationCenter.Current.Show(request);
            bdNo++;
        }

       
    }
}
