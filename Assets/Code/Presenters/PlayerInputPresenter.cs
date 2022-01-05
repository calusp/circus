using Code.Views;
using UniRx;

namespace Code.Presenters
{
    public class PlayerInputPresenter
    {
        private readonly PlayerInput _view;
        private readonly ISubject<float> _actionActivated;

        public PlayerInputPresenter(PlayerInput view, ISubject<float> actionActivated)
        {
            _view = view;
            _actionActivated = actionActivated;
            _view.Action = Activate;
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