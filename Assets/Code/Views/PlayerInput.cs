using System;
using UnityEngine;
using UnityEngine.XR;

namespace Code.Views
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private AnimationCurve curve;
        private float _acc;
        private bool startAccumulate;
        public Action<float> Action { get; set; }
        

        private void Update()
        {
            if(startAccumulate)
                _acc += Time.deltaTime *2 ;
            if (Input.GetButtonDown("Action"))
            {
                startAccumulate = true;
            }
            if (Input.GetButtonUp("Action")|| _acc >=1)
            {
                //Debug.Log($"ACC: {_acc} - Power{curve.Evaluate(_acc)}");
                startAccumulate = false;
                if(_acc >0) Action(curve.Evaluate(_acc));
                _acc = 0;
            }    
        }
    }
}