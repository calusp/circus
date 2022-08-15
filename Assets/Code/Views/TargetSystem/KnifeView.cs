using Code.Views;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Views.TargetSystem
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class KnifeView : MonoBehaviour
    {
        [SerializeField] private float speed = 1.0f;

        private Transform endPosition;
        private BoxCollider2D trigger;

        public Vector2 EndPosition => endPosition.position;

        // Use this for initialization
        void Start()
        {
            trigger = GetComponent<BoxCollider2D>();
            trigger.isTrigger = true;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, endPosition.position, speed * Time.deltaTime);
            if (Mathf.Abs(transform.position.x - endPosition.position.x) <= 0.1f) trigger.isTrigger = false ;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var target = collision.GetComponent<PlayerView>();
            target?.DieFromKnife(this);
            
        }

        public void SetDestination(Transform target)
        {
            this.endPosition = target;
            this.endPosition.position = new Vector3(target.position.x, target.position.y, target.position.y);
        }
    }
}