using System;
using System.Collections;
using UnityEngine;

namespace Code.Views
{
    [RequireComponent(typeof(Camera))]
    public class CameraView : MonoBehaviour
    {
        private Camera _camera;
        private Vector3 _startPosition;

        public float NextPositionOnAxisX { get; private set; }
        public Action<float> SetUpdatedCameraBounds { get;  set; }
        public float ExtraPlayerAmount { get;  set; }

        public void Finish()
        {
            transform.position = _startPosition;
        }

        // Use this for initialization
        void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        public void Reset()
        {
            _startPosition = transform.position;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            transform.position = new Vector3(NextPositionOnAxisX, transform.position.y, transform.position.z);
            SetUpdatedCameraBounds(NextPositionOnAxisX + _camera.orthographicSize);
        }

        public void Move(float moveAmount)
        {
            NextPositionOnAxisX = moveAmount + transform.position.x + ExtraPlayerAmount;     
        }


    }
}