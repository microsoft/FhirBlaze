using FhirBlaze.SharedComponents.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace FhirBlaze.SharedComponents.Services;

public class NotificationService
{
    private readonly int _minutesToLive = 15;

    public NotificationService(int minutesToLive = 15, TimeSpan? updateStateInterval = null)
    {
        _minutesToLive = minutesToLive;
        _timer = new Timer(RemoveExpiredNotifications, null, TimeSpan.FromSeconds(1), updateStateInterval ?? TimeSpan.FromMinutes(1));
    }

    private readonly Timer _timer;
    public readonly List<Notification> Notifications = new();

    public void AddNotification(Notification notitication)
    {
        Notifications.Add(notitication);

        NotifyStateChanged();
    }

    public void RemoveNotification(Notification notitication)
    {
        Notifications.Remove(notitication);

        NotifyStateChanged();
    }

    private void RemoveExpiredNotifications(object? stateInfo)
    {
        for (int i = Notifications.Count - 1; i >= 0; i--)
        {
            var difference = DateTime.Now - Notifications[i].CreatedAt;

            if (difference >= TimeSpan.FromMinutes(_minutesToLive))
                Notifications.RemoveAt(i);
        }

        NotifyStateChanged();
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}

