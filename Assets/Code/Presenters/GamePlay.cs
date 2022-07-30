using System;
using System.Collections.Generic;
using Assets.Code.Presenters;
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
        private readonly SharedGameState sharedGameState;
        private readonly ISubject<Unit> backToMenu;
        private readonly InGameMenuView gameMenuView;
        private PlayerPresenter _playerPresenter;
        private CameraPresenter _cameraPresenter;
        private InGameMenuPresenter _inGameMenuPresenter;
        private PlayerInputPresenter _playerInputPresenter;

        public GamePlay(GamePlayView view, ISubject<Unit> gameStarted, ISubject<Unit> gameFinished, GameConfiguration gameCofiguration, SharedGameState sharedGameState, ISubject<Unit> backToMenu)
        {
            _gamePlayView = view;
            _gameStarted = gameStarted;
            InitializeView();
            _gameFinished = gameFinished;
            this.gameCofiguration = gameCofiguration;
            this.sharedGameState = sharedGameState;
            this.backToMenu = backToMenu;
            _gamePlayView.GamePlayStart = GamePlayStart;
            _gamePlayView.GamePlayFinish = GamePlayFinish;
            _gamePlayView.AttachToActionable = AttachTo;
            _gamePlayView.AttachToHazard = AttachTo;
        }

        private void InitializeView()
        {
            _gameStarted.Subscribe(_ => _gamePlayView.Initialize());
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
            sharedGameState.Initialize();
        }

        private void GamePlayStart(
            PlayerInput playerInput,
            PlayerView playerView,
            CameraView cameraView,
            AudioCenter audioCenter,
            InGameMenuView gameMenuView
            )
        {
            var actionActivated = new Subject<float>();
            var moved = new Subject<float>();
            _playerInputPresenter = new PlayerInputPresenter(playerInput, actionActivated, moved);
            _playerPresenter = new PlayerPresenter(playerView, actionActivated, _gamePlayView, gameCofiguration, moved, sharedGameState, audioCenter);
            _cameraPresenter = new CameraPresenter(playerView, cameraView, _gamePlayView);
            _inGameMenuPresenter = new InGameMenuPresenter(gameMenuView, audioCenter, backToMenu);
            backToMenu.Subscribe(_ => {
                _gamePlayView.ClearState();
                _playerPresenter.Dismiss();
                sharedGameState.Initialize();
            } );
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