using System;
using System.Collections.Generic;
using Code.Views;
using UniRx;

namespace Code.Presenters
{
    public class GamePlay
    {
        private readonly GamePlayView _view;
        private readonly ISubject<Unit> _gameStarted;
        private readonly ISubject<Unit> _gameFinished;
        private PlayerPresenter _playerPresenter;
        private PlayerInputPresenter _playerInputPresenter;

        public GamePlay(GamePlayView view, ISubject<Unit> gameStarted, ISubject<Unit> gameFinished)
        {
            _view = view;
            _gameStarted = gameStarted;
            _gameStarted.Subscribe(_ => _view.Initialize());
            _gameFinished = gameFinished;
            _view.GamePlayStart = GamePlayStart;
            _view.GamePlayFinish = GamePlayFinish;
            _view.AttachTo = AttachTo;
        }

        private void AttachTo(Actionable actionable)
        {
            actionable.Attach(_playerPresenter);
        }

        private void GamePlayFinish()
        {
            _playerPresenter.Dismiss();
            _playerInputPresenter.Dismiss();
            _gameFinished.OnNext(Unit.Default);
        }

        private void GamePlayStart(PlayerInput playerInput, PlayerView playerView)
        {
            var actionActivated = new Subject<float>();
            _playerInputPresenter = new PlayerInputPresenter(playerInput, actionActivated);
            _playerPresenter = new PlayerPresenter(playerView, actionActivated, _view);
            _view.CreateChunks();
        }
    }
}