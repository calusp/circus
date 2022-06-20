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
        private readonly SharedGameState sharedGameState;
        private bool _isGrounded;
        private bool _isOnTrampoline;
        private bool _jumpedFromTrampoline;
        private bool _isInCannon;
        private bool _launchedFromCannon;
        private bool _isInTrapece;
        private bool _launchedFromTrapece;
        private bool playerDied;

        public PlayerPresenter(PlayerView view, ISubject<float> actionActivated, GamePlayView gamePlayView, GameConfiguration gameConfiguration, ISubject<float> moved, SharedGameState sharedGameState)
        {
            _view = view;
            _view.IsGrounded = SetGrounded;
            _actionActivated = actionActivated;
            _gamePlayView = gamePlayView;
            this.gameConfiguration = gameConfiguration;
            this.moved = moved;
            this.sharedGameState = sharedGameState;
            this.sharedGameState.PlayerDied.Subscribe(_ => playerDied = true);
            this.sharedGameState.OnTrampolineHit.Subscribe(_ => TrampolineJump());
            _actionActivated.Subscribe(ActivateAction);
            this.moved.Subscribe(Move);
            _view.DieFromSmash = DieSmashed;
            _view.DieFromKnife = DieFromKnife;
            _view.DieFromBanana = DieFromBanana;
            _view.DieFromBottle = DieFromBottle;
            playerDied = false;
        }

        private void DieFromBottle()
        {
            sharedGameState.PlayerDied.OnNext(Unit.Default);
            _view.DieBottle()
                 .Subscribe(_ => _gamePlayView.Finish());
        }

        private void DieFromBanana()
        {
            sharedGameState.PlayerDied.OnNext(Unit.Default);
            _view.DieFallBanana()
                 .Subscribe(_ => _gamePlayView.Finish());
        }

        private void Move(float speed)
        {
            if (playerDied)
            {
                _view.Move( - gameConfiguration.CameraSpeed);
                return;
            }
            if (speed != 0) _view.SetWalking();
            else _view.SetStopping();
           _view.Move(speed * gameConfiguration.PlayerSpeed - gameConfiguration.CameraSpeed);
        }

        private void DieFromKnife(KnifeView knife)
        {
            sharedGameState.PlayerDied.OnNext(Unit.Default);
            _view.DieKnifed(knife)
                .DoOnCompleted(_gamePlayView.Finish)
                .Subscribe();
        }

        private void ActivateAction(float power)
        {
            if (playerDied) return;
            if (_isGrounded)
            {
                _view
                    .Jump(power * gameConfiguration.JumpForce.x, power * gameConfiguration.JumpForce.y);
            }
        }


        public void DieSmashed()
        {
            sharedGameState.PlayerDied.OnNext(Unit.Default);
            _view.DieSmashed().Subscribe(_ => _gamePlayView.Finish());
        }

        public void DieBurnt()
        {
            sharedGameState.PlayerDied.OnNext(Unit.Default);
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
            _view.Jump(gameConfiguration.TrampolineForce.x, gameConfiguration.TrampolineForce.y);
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