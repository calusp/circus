using System;
using System.Collections.Generic;
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
        private bool _isGrounded;
        private bool _isOnTrampoline;
        private bool _jumpedFromTrampoline;
        private bool _isInCannon;
        private bool _launchedFromCannon;

        public PlayerPresenter(PlayerView view, ISubject<float> actionActivated, GamePlayView gamePlayView, GameConfiguration gameConfiguration)
        {
            _view = view;
            _view.IsGrounded = SetGrounded;
            _actionActivated = actionActivated;
            _gamePlayView = gamePlayView;
            this.gameConfiguration = gameConfiguration;
            _gamePlayView.MovePlayer = Move;
            _actionActivated.Subscribe(ActivateAction);
        }

        private void Move(float amount)
        {
            if(!_isOnTrampoline && !_isInCannon)
                _view.Move(amount);
        }

        private  void ActivateAction(float power)
        {
            if (_isOnTrampoline) _jumpedFromTrampoline = true;
            else if (_isInCannon) _launchedFromCannon = true;
            else if(_isGrounded)
            {
                _view.Jump(power * gameConfiguration.JumpForce.x, power * gameConfiguration.JumpForce.y);
                _jumpedFromTrampoline = false;
            }
        }

        public void DieSmashed()
        {
            _view.DieSmashed().Subscribe(_ => _gamePlayView.Finish());
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
                _view.Jump(0,0);
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
        }

        public void EnterCannon(Vector2 cannon)
        {
            _view.GetPlayerInCannon(cannon);
            _isInCannon = true;
        }

        public void LaunchFromCannon()
        {
            _view.Jump(gameConfiguration.TrampolineForce.x, gameConfiguration.TrampolineForce.y);
        }

    }
}