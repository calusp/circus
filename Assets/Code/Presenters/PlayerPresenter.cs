using System.Collections.Generic;
using Code.Views;
using UniRx;

namespace Code.Presenters
{
    public class PlayerPresenter
    {
        private readonly PlayerView _view;
        private readonly ISubject<float> _actionActivated;
        private readonly GamePlayView _gamePlayView;
        private bool _isGrounded;
        private bool _isOnTrampoline;
        private bool _jumpedFromTrampoline;

        public PlayerPresenter(PlayerView view, ISubject<float> actionActivated, GamePlayView gamePlayView)
        {
            _view = view;
            _view.IsGrounded = SetGrounded;
            _actionActivated = actionActivated;
            _gamePlayView = gamePlayView;
            _gamePlayView.MovePlayer = Move;
            _actionActivated.Subscribe(ActivateAction);
        }

        private void Move(float amount)
        {
            if(!_isOnTrampoline)
                _view.Move(amount);
        }

        private  void ActivateAction(float power)
        {
            if (_isOnTrampoline) _jumpedFromTrampoline = true;
            else if(_isGrounded) _view.Jump(power * 0.8f, power * 0.2f);
            
            
        }

        private void SetGrounded(bool isGrounded)
        {
            _isGrounded = isGrounded;
        }

        public void Dismiss()
        {
            _view.Reset();
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
                _view.Jump(1 * 0.8f, 1 * 0.5f);
            }
        }
    }
}