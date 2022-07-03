using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections.Generic;

namespace Code.Views
{
    public class ClownBoxView : MonoBehaviour
    {
        [SerializeField] List<AnimationClip> open = new List<AnimationClip>();
        [SerializeField] Animator animator;

        public void Awake()
        {
            Show();
        }
        public void Show()
        {
           
        }

      
        public Action  AnimationEnded { get; set; }
      
    }
}