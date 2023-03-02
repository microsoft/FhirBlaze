using System;

namespace FhirBlaze.SharedComponents.Models;

public class Notification
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string TimeSinceStr
    {
        get
        {
            var difference = DateTime.Now - CreatedAt;

            if (difference.Days > 0)
                return difference.Days.ToString() + "day(s) ago";

            if (difference.Hours > 0)
                return difference.Hours.ToString() + "hour(s) ago";

            return difference.Minutes.ToString() + "min(s) ago";
        }
    }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
