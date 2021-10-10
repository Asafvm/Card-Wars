using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class NotificationsHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI notificationText;
    void Start()
    {
        UpdateNotification("");
    }

    public void UpdateNotification(string message)
    {
        notificationText.text = message;
    }

    
}
