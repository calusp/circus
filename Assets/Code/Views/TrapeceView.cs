
using Code.Presenters;
using Code.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Views
{
    public class TrapeceView : MonoBehaviour
    {
        [SerializeField, Range(-1,1)] private float maxRotationZ = 0.3f;
        [SerializeField, Range(-1, 1)] private float minRotationZ = 0.3f;
        [SerializeField] private GameConfiguration gameConfiguration;
        [SerializeField] private Transform playerSpot;

        public Vector3 PlayerJumpDirection => playerSpot.position.normalized;

        public Action<Vector2> EnterTrapece { get; set; }
        public Action<Vector2> TrapecePositionUpdated { get; set; }

        private int direction;
        private BoxCollider2D _trigger;
        private bool _startBounce = false;

        private void Start()
        {
            // transform.rotation.Set(transform.rotation.x, transform.rotation.y, minRotaionZ, transform.rotation.w);
            direction = -1;
            _trigger = GetComponent<BoxCollider2D>();
            _trigger.isTrigger = true;
        }

        private void FixedUpdate()
        {
            if (!_startBounce) return;
            transform.rotation = direction == 1 ?
               Quaternion.LerpUnclamped(transform.rotation, new Quaternion(0, 0, maxRotationZ, 1), gameConfiguration.TrapeceSpeed) :
               Quaternion.LerpUnclamped(transform.rotation, new Quaternion(0, 0, minRotationZ, 1), gameConfiguration.TrapeceSpeed);


            if (transform.rotation.z >= maxRotationZ - gameConfiguration.TrapeceSpeed)
                direction = -1;
            if (transform.rotation.z < minRotationZ + gameConfiguration.TrapeceSpeed)
                direction = 1;

            TrapecePositionUpdated(transform.position);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name.Contains("Player"))
            {
                EnterTrapece(playerSpot.position);
                _trigger.enabled = false;
                _startBounce = true;
            }
        }
    }
}
