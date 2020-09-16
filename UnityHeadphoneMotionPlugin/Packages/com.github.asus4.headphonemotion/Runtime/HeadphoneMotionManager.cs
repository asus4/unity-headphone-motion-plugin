using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace HeadphoneMotion
{
    /*

typedef struct {
    CMAcceleration userAcceleration;
    CMQuaternion rotation;
    CMDeviceMotionSensorLocation location;
} HeadphoneMotionData;
typedef void (*UnityHeadphoneMotionCallback)(HeadphoneMotionData motion);
typedef void (*UnityHeadphoneMotionEventCallback)(BOOL connected);

extern bool _unityHeadphoneDeviceMotionIsAvailable(void);
extern bool _unityHeadphoneDeviceMotionIsActive(void);
extern void _unityHeadphoneMotionStart(UnityHeadphoneMotionCallback callback);
extern void _unityHeadphoneMotionStop(void);
extern void _unityHeadphoneMotionSetEventCallback(UnityHeadphoneMotionEventCallback callback);
    */


    public class HeadphoneMotionManager
    {
        public enum SensorLocation
        {
            Default = 0,
            Left = 1,
            Right = 2,
        }

        public struct HeadphoneMotionData
        {
            public double3 userAcceleration;
            public double4 rotation;
            public SensorLocation location;
        }

        public static bool IsAvailable => false;
        public static bool IsActive => false;
        public static bool IsConnected => false;

        public event Action OnConnected;
        public event Action OnDisconnected;
        public event Action<HeadphoneMotionData> OnUpdated;

        public void Start()
        {

        }

        public void Stop()
        {

        }
    }
}
