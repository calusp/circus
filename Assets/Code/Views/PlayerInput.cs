using System;
using UnityEngine;
using UnityEngine.XR;

namespace Code.Views
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private float threshold = 0.5f;
        [SerializeField] private AnimationCurve curve;
        private float _acc;
        private float _doubleClickAcc;
        private bool startAccumulateEnery;
        private bool isAccDoubleClick;
        public Action<float> Action { get; set; }
        public Action Stop { get; set; }

        private void Update()
        {
            if (startAccumulateEnery)
            {
                _acc += Time.deltaTime * 2;
            }
            if( isAccDoubleClick)
                _doubleClickAcc += Time.deltaTime * 2;

            if (_doubleClickAcc > threshold)
            {
                _doubleClickAcc = 0;
                isAccDoubleClick = false;
            }

            if (Input.GetButtonDown("Action"))
            {
                startAccumulateEnery = true;
                isAccDoubleClick = true;
                if (_doubleClickAcc > 0 && _doubleClickAcc <= threshold)
                {
                    Stop();
                    _doubleClickAcc = 0;
                    _acc = 0;
                    startAccumulateEnery = false;
                    isAccDoubleClick = false;
                }
            }
            if (Input.GetButtonUp("Action") || _acc >= 1+ threshold)
            { 
                startAccumulateEnery = false;
                if (_acc > threshold) Action(curve.Evaluate(_acc - threshold)) ;
                _acc = 0;
            }

          
        }
    }
}