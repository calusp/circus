using Code.Views;
using System;

namespace Code.Presenters
{
    public class CameraPresenter
    {
        private PlayerView _playerView;
        private CameraView _cameraView;
        private GamePlayView _gamePlayView;

        public CameraPresenter(PlayerView playerView, CameraView cameraView, GamePlayView gamePlayView)
        {
            _playerView = playerView;
            _cameraView = cameraView;
            _gamePlayView = gamePlayView;

            _gamePlayView.MoveCamera = Move;
            _cameraView.SetUpdatedCameraBounds = KeepPlayerIntoBounds;
        }

        public void Initialize()
        {
            _cameraView.Reset();
        }

        private void Move(float moveAmount)
        {
            _cameraView.Move(moveAmount);
        }

        public void KeepPlayerIntoBounds(float maxPosX)
        {
            var distance = _playerView.transform.position.x - maxPosX;
            _cameraView.ExtraPlayerAmount = (distance > 0 ? distance : 0);
        }
    }
}