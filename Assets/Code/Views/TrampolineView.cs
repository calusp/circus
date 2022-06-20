using System;
using Code.Presenters;
using Code.ScriptableObjects;
using UniRx;
using UnityEngine;

namespace Code.Views
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TrampolineView : MonoBehaviour
    {
        [SerializeField] private SharedGameState sharedGameState;
        [SerializeField] private Animator animator;
        private BoxCollider2D boxCollider2D;

        private readonly int Jumping = Animator.StringToHash("jumping");

        public void Start()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            boxCollider2D.isTrigger = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            sharedGameState.OnTrampolineHit.OnNext(Unit.Default);
            animator.SetTrigger(Jumping);
        }
    }
    
}
