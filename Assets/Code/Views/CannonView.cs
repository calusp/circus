using Code.Presenters;
using Code.ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;

namespace Code.Views
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CannonView : BaseActionableView
    {
        public Transform innerCannon;


        [SerializeField] private float maxRotationZ;
        [SerializeField] private float minRotationZ;
        [SerializeField] private GameConfiguration gameConfiguration;
        [SerializeField] private Transform playerSpot;

        private int direction;
        private PlayerPresenter _playerPresenter;
        private BoxCollider2D _trigger;
        private void Start()
        {
            // transform.rotation.Set(transform.rotation.x, transform.rotation.y, minRotaionZ, transform.rotation.w);
            direction = 1;
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
            float moveAmount = Time.fixedDeltaTime * gameConfiguration.CannonSpeed * 100;
            if (innerCannon.rotation.z >= Quaternion.Euler(0, 0, maxRotationZ - moveAmount).z)
                direction = -1;
            if (innerCannon.rotation.z <= Quaternion.Euler(0, 0, minRotationZ - moveAmount).z)
                direction = 1;

            innerCannon.rotation = Quaternion.Euler(0,
                0,
                innerCannon.rotation.eulerAngles.z + moveAmount * direction
                );
            _playerPresenter.UpdatePlayerRotation(playerSpot.position, playerSpot.rotation);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name.Contains("Player"))
            {
                _playerPresenter.EnterCannon(playerSpot.position,playerSpot.rotation);
            }

        }

    }
}