using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace HeadphoneMotion
{

    // CMDeviceMotionSensorLocation
    public enum DeviceSensorLocation
    {
        Default = 0,
        HeadphoneLeft = 1,
        HeadphoneRight = 2,
    }

    /// <summary>
    /// typedef struct {
    ///   CMAcceleration userAcceleration;
    ///   CMQuaternion rotation;
    ///   CMDeviceMotionSensorLocation location;
    /// } HeadphoneMotionData;
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct HeadphoneMotionData
    {
        public Vector3 userAcceleration { get; }
        public Quaternion rotation { get; }
        public DeviceSensorLocation location { get; }
    }

    public class HeadphoneMotionManager
    {
        public static bool IsAvailable => _unityHeadphoneDeviceMotionIsAvailable();
        public static bool IsActive => _unityHeadphoneDeviceMotionIsActive();

        public static event Action OnConnected;
        public static event Action OnDisconnected;
        public static event Action<HeadphoneMotionData> OnUpdated;

        public static void Start()
        {
            _unityHeadphoneMotionSetEventCallback(OnMotionEvent);
            _unityHeadphoneMotionStart(OnMotionUpdate);
        }

        public static void Stop()
        {
            _unityHeadphoneMotionStop();
        }


        // typedef void (*UnityHeadphoneMotionCallback)(HeadphoneMotionData motion);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
        private delegate void HeadphoneMotionDelegate(HeadphoneMotionData motionData);

        [AOT.MonoPInvokeCallback(typeof(HeadphoneMotionDelegate))]
        private static void OnMotionUpdate(HeadphoneMotionData motionData)
        {
            if (OnUpdated != null)
            {
                OnUpdated(motionData);
            }
        }

        // typedef void (*UnityHeadphoneMotionEventCallback)(BOOL connected);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
        private delegate void HeadphoneMotionEventDelegate(bool connected);

        [AOT.MonoPInvokeCallback(typeof(HeadphoneMotionEventDelegate))]
        private static void OnMotionEvent(bool connected)
        {
            // IsConnected = connected;
            if (OnConnected != null)
            {
                OnConnected();
            }
            if (OnDisconnected != null)
            {
                OnDisconnected();
            }
        }

        #region DllImport

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern bool _unityHeadphoneDeviceMotionIsAvailable();
        [DllImport("__Internal")]
        private static extern bool _unityHeadphoneDeviceMotionIsActive();
        [DllImport("__Internal")]
        private static extern void _unityHeadphoneMotionStart(HeadphoneMotionDelegate callback);
        [DllImport("__Internal")]
        private static extern void _unityHeadphoneMotionStop();
        [DllImport("__Internal")]
        private static extern void _unityHeadphoneMotionSetEventCallback(HeadphoneMotionEventDelegate callback);
#else
        private static bool _unityHeadphoneDeviceMotionIsAvailable() => false;
        private static bool _unityHeadphoneDeviceMotionIsActive() => false;
        private static void _unityHeadphoneMotionStart(HeadphoneMotionDelegate callback) { }
        private static void _unityHeadphoneMotionStop() { }
        private static void _unityHeadphoneMotionSetEventCallback(HeadphoneMotionEventDelegate callback) { }
#endif

        #endregion // DllImport
    }
}
