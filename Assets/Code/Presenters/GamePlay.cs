using System;
using System.Collections.Generic;
using Code.Views;
using UniRx;

namespace Code.Presenters
{
    public class GamePlay
    {
        private readonly GamePlayView _gamePlayView;
        private readonly ISubject<Unit> _gameStarted;
        private readonly ISubject<Unit> _gameFinished;
        private PlayerPresenter _playerPresenter;
        private CameraPresenter _cameraPresenter;
        private PlayerInputPresenter _playerInputPresenter;

        public GamePlay(GamePlayView view, ISubject<Unit> gameStarted, ISubject<Unit> gameFinished)
        {
            _gamePlayView = view;
            _gameStarted = gameStarted;
            _gameStarted.Subscribe(_ => _gamePlayView.Initialize());
            _gameFinished = gameFinished;
            _gamePlayView.GamePlayStart = GamePlayStart;
            _gamePlayView.GamePlayFinish = GamePlayFinish;
            _gamePlayView.AttachToActionable = AttachTo;
            _gamePlayView.AttachToHazard = AttachTo;
        }

        private void AttachTo(Actionable actionable)
        {
            actionable.Attach(_playerPresenter);
        }
        
        private void AttachTo(Hazard hazard)
        {
            hazard.Attach(_playerPresenter);
        }

        private void GamePlayFinish()
        {
            _playerPresenter.Dismiss();
            _playerInputPresenter.Dismiss();
            _gameFinished.OnNext(Unit.Default);
        }

        private void GamePlayStart(PlayerInput playerInput, PlayerView playerView, CameraView cameraView)
        {
            var actionActivated = new Subject<float>();
            _playerInputPresenter = new PlayerInputPresenter(playerInput, actionActivated);
            _playerPresenter = new PlayerPresenter(playerView, actionActivated, _gamePlayView);
            _cameraPresenter = new CameraPresenter(playerView, cameraView, _gamePlayView);
            _gamePlayView.CreateChunks();
        }
    }

    public interface Hazard
    {
        void Execute();
        void Attach(PlayerPresenter playerPresenter);
    }
}