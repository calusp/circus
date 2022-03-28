
using Code.Presenters;
using Code.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Views
{
    public class TrapeceView : BaseActionableView
    {
        [SerializeField, Range(-1,1)] private float maxRotationZ = 0.3f;
        [SerializeField, Range(-1, 1)] private float minRotationZ = 0.3f ;
        [SerializeField] private GameConfiguration gameConfiguration;
        [SerializeField] private Transform playerSpot;

        private int direction;
        private PlayerPresenter _playerPresenter;
        private BoxCollider2D _trigger;
        private bool _startBounce = false;

        private void Start()
        {
            // transform.rotation.Set(transform.rotation.x, transform.rotation.y, minRotaionZ, transform.rotation.w);
            direction = -1;
            _trigger = GetComponent<BoxCollider2D>();
            _trigger.isTrigger = true;
        }
        public override void Execute()
        {
            //_playerPresenter.LaunchFromCannon();
        }

        public override void Attach(PlayerPresenter playerPresenter)
        {
            _playerPresenter = playerPresenter;
        }

        private void FixedUpdate()
        {
            if (!_startBounce) return;
            float moveAmount = Time.fixedDeltaTime * gameConfiguration.TrapeceSpeed;
            transform.rotation = new Quaternion(0,
               0,
               transform.rotation.z + moveAmount * direction,1
               );

            if (transform.rotation.z >= maxRotationZ)
                direction = -1;
            if (transform.rotation.z < minRotationZ)
                direction = 1;

           
            _playerPresenter.UpdatePlayerRotation(playerSpot.position, playerSpot.rotation);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name.Contains("Player"))
            {
                _playerPresenter.EnterTrapece(playerSpot.position);
                _trigger.enabled = false;
                _startBounce = true;
            }
        }
    }
}
