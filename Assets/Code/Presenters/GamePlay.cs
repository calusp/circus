using System;
using System.Collections.Generic;
using Code.ScriptableObjects;
using Code.Views;
using UniRx;

namespace Code.Presenters
{
    public class GamePlay
    {
        private readonly GamePlayView _gamePlayView;
        private readonly ISubject<Unit> _gameStarted;
        private readonly ISubject<Unit> _gameFinished;
        private readonly GameConfiguration gameCofiguration;
        private PlayerPresenter _playerPresenter;
        private CameraPresenter _cameraPresenter;
        private PlayerInputPresenter _playerInputPresenter;

        public GamePlay(GamePlayView view, ISubject<Unit> gameStarted, ISubject<Unit> gameFinished, GameConfiguration gameCofiguration)
        {
            _gamePlayView = view;
            _gameStarted = gameStarted;
            InitializeView(gameCofiguration);
            _gameFinished = gameFinished;
            this.gameCofiguration = gameCofiguration;
            _gamePlayView.GamePlayStart = GamePlayStart;
            _gamePlayView.GamePlayFinish = GamePlayFinish;
            _gamePlayView.AttachToActionable = AttachTo;
            _gamePlayView.AttachToHazard = AttachTo;
        }

        private void InitializeView(GameConfiguration gameCofiguration)
        {
            _gameStarted.Subscribe(_ => _gamePlayView.Initialize(gameCofiguration));
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
            //_gameFinished.OnNext(Unit.Default);
        }

        private void GamePlayStart(PlayerInput playerInput, PlayerView playerView, CameraView cameraView)
        {
            var actionActivated = new Subject<float>();
            _playerInputPresenter = new PlayerInputPresenter(playerInput, actionActivated);
            _playerPresenter = new PlayerPresenter(playerView, actionActivated, _gamePlayView, gameCofiguration);
            _cameraPresenter = new CameraPresenter(playerView, cameraView, _gamePlayView);
            _playerPresenter.Initialize();
            _cameraPresenter.Initialize();
            _gamePlayView.StartGamePlay();
        }
    }

    public interface Hazard
    {
        void Execute();
        void Attach(PlayerPresenter playerPresenter);
    }
}