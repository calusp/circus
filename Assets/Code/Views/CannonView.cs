using Code.Presenters;
using Code.ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;

namespace Code.Views
{
    public class CannonView : BaseActionableView
    {
        public Transform innerCannon;
        private PlayerPresenter _playerPresenter;

        [SerializeField] private float maxRotationZ;
        [SerializeField] private float minRotationZ;
        [SerializeField] private GameConfiguration gameConfiguration;
        private int direction;
        private void Start()
        {
            // transform.rotation.Set(transform.rotation.x, transform.rotation.y, minRotaionZ, transform.rotation.w);
            direction = 1;
        }
        public override void Execute()
        {
            _playerPresenter.LaunchFromCannon();
        }

        public override void Attach(PlayerPresenter playerPresenter)
        {
            _playerPresenter = playerPresenter;
            _playerPresenter.EnterCannon(innerCannon.position);
        }

        private void FixedUpdate()
        {
            float moveAmount = Time.fixedDeltaTime * gameConfiguration.CannonSpeed * 100;
            if (innerCannon.rotation.z >= Quaternion.Euler(0,0, maxRotationZ- moveAmount).z) 
                direction = -1;
            if (innerCannon.rotation.z <= Quaternion.Euler(0, 0, minRotationZ - moveAmount).z) 
                direction = 1;
            
            innerCannon.rotation =  Quaternion.Euler(0,
                0,
                innerCannon.rotation.eulerAngles.z + moveAmount * direction
                );

        }

    }
}