using System;
using System.Collections.Generic;
using Assets.Code.Views.TargetSystem;
using Code.ScriptableObjects;
using Code.Views;
using UniRx;
using UnityEngine;

namespace Code.Presenters
{
    public class PlayerPresenter
    {
        private readonly PlayerView _view;
        private readonly ISubject<float> _actionActivated;


        private readonly GamePlayView _gamePlayView;
        private readonly GameConfiguration gameConfiguration;
        private readonly ISubject<float> moved;
        private bool _isGrounded;
        private bool _isOnTrampoline;
        private bool _jumpedFromTrampoline;
        private bool _isInCannon;
        private bool _launchedFromCannon;
        private bool _isInTrapece;
        private bool _launchedFromTrapece;

        public PlayerPresenter(PlayerView view, ISubject<float> actionActivated, GamePlayView gamePlayView, GameConfiguration gameConfiguration, ISubject<float> moved)
        {
            _view = view;
            _view.IsGrounded = SetGrounded;
            _actionActivated = actionActivated;
            _gamePlayView = gamePlayView;
            this.gameConfiguration = gameConfiguration;
            this.moved = moved;
            _actionActivated.Subscribe(ActivateAction);
            this.moved.Subscribe(Move);
            _view.DieFromSmash = DieSmashed;
            _view.DieFromKnife = DieFromKnife;
        }

        private void Move(float speed)
        {
            if (speed != 0) _view.SetWalking();
            else _view.SetStopping();
           _view.Move(speed);
        }

        private void DieFromKnife(KnifeView knife)
        {
            _view.DieKnifed(knife)
                .DoOnCompleted(_gamePlayView.Finish)
                .Subscribe();
        }

        private void ActivateAction(float power)
        {
            if (_isOnTrampoline) _jumpedFromTrampoline = true;
            else if (_isInCannon) LaunchFromCannon();
            else if (_isInTrapece) LaunchFromTrapece();
            else if (_isGrounded)
            {
                _view
                    .Jump(power * gameConfiguration.JumpForce.x, power * gameConfiguration.JumpForce.y);
                _jumpedFromTrampoline = false;
            }
        }


        public void DieSmashed()
        {
            _view.DieSmashed().Subscribe(_ => _gamePlayView.Finish());
        }

        public void DieBurnt()
        {
            _view.DieBurnt().Subscribe(_ => _gamePlayView.Finish());
        }

        public void UpdatePlayerRotation(Vector3 position, Quaternion rotation)
        {
            if (_isInCannon || _isInTrapece)
                _view.UpdatePlayerInCannon(position, rotation);
        }


        private void SetGrounded(bool isGrounded)
        {
            _isGrounded = isGrounded;
        }

        public void Dismiss()
        {
            _view.Init();
        }

        private void SetOnTrampoline(bool isOnTrampoline)
        {
            _isOnTrampoline = isOnTrampoline;
        }

        public void TrampolineJump()
        {
            if (!_jumpedFromTrampoline)
            {
                SetOnTrampoline(true);
                _view.Jump(0, 0);
            }
            else
            {
                SetOnTrampoline(false);
                _view.Jump(gameConfiguration.TrampolineForce.x, gameConfiguration.TrampolineForce.y);
            }
        }

        public void Initialize()
        {
            _view.Init();
            SetGrounded(true);
        }

        public void EnterCannon(Vector2 cannon, Quaternion rotation)
        {
            if (_launchedFromCannon)
            {
                _isInCannon = false;
                return;
            }
            if (_isInCannon) return;
            _view.StopMovement = true;
            _view.GetPlayerInCannon(cannon, rotation);
            _isInCannon = true;
        }

        private void LaunchFromCannon()
        {
            //usar cannon force
            _isInCannon = false;
            _view.Jump(gameConfiguration.TrampolineForce.x * _view.transform.position.normalized.x, gameConfiguration.TrampolineForce.y * _view.transform.position.normalized.y);
            _view.RestartMovement();
        }

        public void EnterTrapece(Vector3 trapeceSpot)
        {
            if (_launchedFromTrapece)
            {
                _isInTrapece = false;
                return;
            }
            _view.GetInTrapece(trapeceSpot);
            _view.StopMovement = true;
            _isInTrapece = true;
        }

        private void LaunchFromTrapece()
        {
            _isInTrapece = false;
            _view.Jump(
                gameConfiguration.TrapeceForce.x * _view.transform.localPosition.normalized.x * Mathf.Sign(_view.transform.rotation.z),
                gameConfiguration.TrapeceForce.y * _view.transform.localPosition.normalized.y);
            _view.RestartMovement();
        }


    }
}