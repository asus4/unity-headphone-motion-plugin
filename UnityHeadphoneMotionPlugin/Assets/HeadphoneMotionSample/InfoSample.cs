using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using HeadphoneMotion;

public class InfoSample : MonoBehaviour
{
    [SerializeField] Text infoLabel = null;
    [SerializeField] Button toggleButton = null;
    [SerializeField] Transform axis = null;

    StringBuilder sb = new StringBuilder();
    volatile bool needUpdate = true;
    object lockObj = new object();
    HeadphoneMotionData motionData;

    void OnEnable()
    {
        toggleButton.onClick.AddListener(OnToggleButtonClick);

        HeadphoneMotionManager.OnConnected += () =>
        {
            Debug.Log("headphone connected");
            needUpdate = true;
        };
        HeadphoneMotionManager.OnDisconnected += () =>
        {
            Debug.Log("headphone OnDisconnected");
            needUpdate = true;
        };
        HeadphoneMotionManager.OnUpdated += (HeadphoneMotionData data) =>
        {
            lock (lockObj)
            {
                motionData = data;
            }
            needUpdate = true;
        };

        UpdateInfo(motionData);
    }

    void OnDisable()
    {
        toggleButton.onClick.RemoveListener(OnToggleButtonClick);
        HeadphoneMotionManager.Stop();
    }

    void Update()
    {
        if (needUpdate)
        {
            HeadphoneMotionData data;
            lock (lockObj)
            {
                data = motionData;
            }
            needUpdate = false;
            UpdateInfo(data);
            UpdateAxis(data);
        }
    }
    void OnToggleButtonClick()
    {
        if (!HeadphoneMotionManager.IsAvailable)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Debug.LogWarning("Please make sure your AirPods is connected and the firmware is updated");
            }
            else
            {
                Debug.Log($"HeadphoneMotion is not supported on platform: {Application.platform}");
            }
            return;
        }
        if (HeadphoneMotionManager.IsActive)
        {
            HeadphoneMotionManager.Stop();
        }
        else
        {
            HeadphoneMotionManager.Start();
        }
    }

    void UpdateInfo(HeadphoneMotionData data)
    {
        bool isActive = HeadphoneMotionManager.IsActive;
        sb.Clear();
        sb.AppendLine($"available: {HeadphoneMotionManager.IsAvailable}");
        sb.AppendLine($"active: {isActive}");
        if (isActive)
        {
            sb.AppendLine("motion:");
            sb.AppendLine($"userAcceleration: {data.userAcceleration}");
            sb.AppendLine($"rotation: {data.rotation}");
            sb.AppendLine($"DeviceSensorLocation: {data.location}");
        }
        infoLabel.text = sb.ToString();
    }

    void UpdateAxis(HeadphoneMotionData data)
    {
        axis.transform.localRotation = data.rotation;
    }
}
