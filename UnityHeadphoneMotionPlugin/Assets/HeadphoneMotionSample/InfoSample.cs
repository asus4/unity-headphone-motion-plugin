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

    StringBuilder sb = new StringBuilder();

    void OnEnable()
    {
        toggleButton.onClick.AddListener(OnToggleButtonClick);
        UpdateInfo();
    }

    void OnDisable()
    {
        toggleButton.onClick.RemoveListener(OnToggleButtonClick);
        HeadphoneMotionManager.Stop();
    }

    void OnToggleButtonClick()
    {
        if (!HeadphoneMotionManager.IsActive || !HeadphoneMotionManager.IsAvailable)
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
        if (HeadphoneMotionManager.IsConnected)
        {
            HeadphoneMotionManager.Stop();
        }
        else
        {
            HeadphoneMotionManager.Start();
        }
    }

    void UpdateInfo()
    {
        sb.Clear();
        sb.AppendLine($"available: {HeadphoneMotionManager.IsAvailable}");
        sb.AppendLine($"active: {HeadphoneMotionManager.IsActive}");
        sb.AppendLine($"connected: {HeadphoneMotionManager.IsConnected}");
        infoLabel.text = sb.ToString();
    }
}
