using Code.Views;
using System;
using UniRx;
using UnityEngine;

namespace Code.Presenters
{
    public class PlayerInputPresenter
    {
        private readonly PlayerInput _view;
        private readonly ISubject<float> _actionActivated;
        private readonly ISubject<Unit> stopped;
        private readonly ISubject<float> moved;

        public PlayerInputPresenter(PlayerInput view, ISubject<float> actionActivated, ISubject<float> moved)
        {
            _view = view;
            _actionActivated = actionActivated;
            this.moved = moved;
            _view.Action = Activate;
            _view.Move = Move;
        }

        private void Move(float speed)
        {
            moved.OnNext(speed);
        }

        private void Activate(float power)
        {
            _actionActivated.OnNext(power);
        }

        public void Dismiss()
        {
            
        }
    }
}