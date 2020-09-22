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
    HeadphoneMotionData _motionData;
    HeadphoneMotionData MotionData
    {
        get
        {
            lock (lockObj) return _motionData;
        }
        set
        {
            lock (lockObj) _motionData = value;
        }
    }

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
            MotionData = data;
            needUpdate = true;
        };

        UpdateInfo(_motionData);
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
            HeadphoneMotionData data = MotionData;
            UpdateInfo(data);
            UpdateAxis(data);
            needUpdate = false;
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

    void UpdateInfo(in HeadphoneMotionData data)
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

    void UpdateAxis(in HeadphoneMotionData data)
    {
        axis.transform.localRotation = data.rotation;
    }
}
