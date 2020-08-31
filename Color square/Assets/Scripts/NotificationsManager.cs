using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

public class NotificationsManager : MonoBehaviour
{
    void Start()
    {
        createNotificationsChannel();
    }

    void createNotificationsChannel()
    {
        AndroidNotificationChannel channel = new AndroidNotificationChannel()
        {
            Id = "channel_0",
            Name = "Channel",
            Importance = Importance.High,
            Description = "Notifications channel",
        };
    }

    static void sendNotification(string title, string text, string iconId, System.DateTime dateTime)
    {
        AndroidNotification notification = new AndroidNotification();

        notification.Title = title;
        notification.Text = text;
        notification.LargeIcon = iconId;
        notification.FireTime = dateTime;

        AndroidNotificationCenter.SendNotification(notification, "channel_0");
    }
}
