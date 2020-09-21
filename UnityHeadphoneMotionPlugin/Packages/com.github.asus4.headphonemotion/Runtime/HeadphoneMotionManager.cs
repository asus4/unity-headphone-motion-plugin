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
    /// CMDeviceMotion
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct HeadphoneMotionData
    {
        public Vector3 userAcceleration { get; }
        public Quaternion rotation { get; }
        public DeviceSensorLocation location { get; }
    }

    /// <summary>
    /// A simple bridge of CMHeadphoneMotionManager
    /// https://developer.apple.com/documentation/coremotion/cmheadphonemotionmanager
    /// </summary>
    public class HeadphoneMotionManager
    {
        /// <summary>
        /// A Boolean value that indicates whether the current device supports the headphone motion manager.
        /// </summary>
        /// <returns>A Boolean value</returns>
        public static bool IsAvailable => _unityHeadphoneDeviceMotionIsAvailable();
        
        /// <summary>
        /// A Boolean value that indicates whether the headphone motion manager is active.
        /// </summary>
        /// <returns>A Boolean value</returns>
        public static bool IsActive => _unityHeadphoneDeviceMotionIsActive();

        /// <summary>
        /// A event when you connect headphones.
        /// </summary>
        public static event Action OnConnected;

        /// <summary>
        /// A event when you disconnect headphones.
        /// </summary>
        public static event Action OnDisconnected;

        /// <summary>
        /// A event when you received the headphones-motion event.
        /// </summary>
        public static event Action<HeadphoneMotionData> OnUpdated;

        /// <summary>
        /// Starts device-motion updates
        /// </summary>
        public static void Start()
        {
            _unityHeadphoneMotionSetEventCallback(OnMotionEvent);
            _unityHeadphoneMotionStart(OnMotionUpdate);
        }

        /// <summary>
        /// Stops device-motion updates
        /// </summary>
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
