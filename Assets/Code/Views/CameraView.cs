using System;
using System.Collections;
using UnityEngine;

namespace Code.Views
{
    [RequireComponent(typeof(Camera))]
    public class CameraView : MonoBehaviour
    {
        public float NextPositionOnAxisX { get; private set; }
        public Action<float> SetUpdatedCameraBounds { get;  set; }
        public float ExtraPlayerAmount { get;  set; }

     
        // Use this for initialization
        void Awake()
        {
           
        }

        public void Reset()
        {
          
        }

        // Update is called once per frame
        void FixedUpdate()
        {
           
        }

      
    }
}