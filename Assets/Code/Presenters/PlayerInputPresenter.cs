using Code.Views;
using UniRx;
using UnityEngine;

namespace Code.Presenters
{
    public class PlayerInputPresenter
    {
        private readonly PlayerInput _view;
        private readonly ISubject<float> _actionActivated;
        private readonly ISubject<Unit> stopped;

        public PlayerInputPresenter(PlayerInput view, ISubject<float> actionActivated, ISubject<Unit> stopped)
        {
            _view = view;
            _actionActivated = actionActivated;
            this.stopped = stopped;
            _view.Action = Activate;
            _view.Stop = Stop;
        }

        private void Activate(float power)
        {
            _actionActivated.OnNext(power);
        }

        private void Stop()
        {
            stopped.OnNext(Unit.Default);
        }

        public void Dismiss()
        {
            
        }
    }
}