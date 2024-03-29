﻿using System;
using UnityEngine;
using UnityEngine.XR;

namespace Code.Views
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private AnimationCurve curve;
        private float _acc;
        private bool startAccumulateEnery;
        public Action<float> Action { get; set; }
        public Action<float> Move { get; set; }

        private void Update()
        {
            ClickHandling();
            Move(Input.GetAxis("Horizontal"));
        }

        private void ClickHandling()
        {
            //JumpCharged();
            if (Input.GetButtonDown("Jump"))
            {
                Action(curve.Evaluate(0.8f));
            }

        }

        private void JumpCharged()
        {
            if (startAccumulateEnery)
            {
                _acc += Time.deltaTime * 4;
            }


            if (Input.GetButtonDown("Jump"))
            {
                startAccumulateEnery = true;

            }
            if (Input.GetButtonUp("Jump") || _acc >= 1)
            {
                startAccumulateEnery = false;
                Action(curve.Evaluate(_acc));
                _acc = 0;
            }
        }
    }
}