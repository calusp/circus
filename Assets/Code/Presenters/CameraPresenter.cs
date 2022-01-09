using Code.Views;

namespace Code.Presenters
{
    public class CameraPresenter
    {
        private PlayerView playerView;
        private CameraView cameraView;
        private GamePlayView gamePlayView;

        public CameraPresenter(PlayerView playerView, CameraView cameraView, GamePlayView gamePlayView)
        {
            this.playerView = playerView;
            this.cameraView = cameraView;
            this.gamePlayView = gamePlayView;
        }
    }
}