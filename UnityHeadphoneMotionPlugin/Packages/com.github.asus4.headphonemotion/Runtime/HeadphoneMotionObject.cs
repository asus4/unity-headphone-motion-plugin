using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeadphoneMotion
{
    public class HeadphoneMotionObject : MonoBehaviour
    {
        [Tooltip("The distance between left and right ear")]
        [SerializeField, Range(0f, 1f)] float earDistance = 0.215f;

        [SerializeField] bool autoStart = false;


        private volatile bool needUpdate = true;
        private object lockObj = new object();
        private HeadphoneMotionData _motionData;
        public HeadphoneMotionData MotionData
        {
            get
            {
                lock (lockObj) return _motionData;
            }
            private set
            {
                lock (lockObj) _motionData = value;
            }
        }

        private void OnEnable()
        {
            HeadphoneMotionManager.OnUpdated += OnHeadphoneMotionUpdate;
            if (autoStart)
            {
                HeadphoneMotionManager.Start();
            }
        }

        private void OnDisable()
        {
            HeadphoneMotionManager.OnUpdated -= OnHeadphoneMotionUpdate;
            if (autoStart)
            {
                HeadphoneMotionManager.Stop();
            }
        }

        private void Update()
        {
            if (!needUpdate) return;

            transform.rotation = MotionData.rotation;
            needUpdate = false;
        }

        private void OnDrawGizmos()
        {

        }

        private void OnHeadphoneMotionUpdate(HeadphoneMotionData data)
        {
            MotionData = data;
            needUpdate = true;
        }
    }
}