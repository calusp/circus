using Code.Views;
using UniRx;

namespace Code.Presenters
{
    public class StartGameHubPresenter
    {
        private readonly StartGameHudView _view;
        private readonly ISubject<Unit> _gameStarted;
        private readonly Subject<Unit> _gameFinished;

        public StartGameHubPresenter(StartGameHudView view, ISubject<Unit> gameStarted, Subject<Unit> gameFinished)
        {
            _view = view;
            _view.StartGame = StartGame;
            _gameStarted = gameStarted;
            _gameFinished = gameFinished;
        }

        public void Initialize()
        {
            _view.Initialize();
            _gameFinished.Do(_ => _view.Show()).Subscribe();
        }

        private void StartGame()
        {
            _gameStarted.OnNext(Unit.Default);
            _view.Hide();
        }
    }
}