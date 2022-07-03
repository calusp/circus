using System;
using UniRx;

namespace Code.Views
{
    public class ClownBoxPresenter
    {
        private ClownBoxView clownBoxView;
        private Subject<Unit> gameStarted;

        public ClownBoxPresenter(ClownBoxView clownBoxView, Subject<Unit> gameStarted)
        {
            this.clownBoxView = clownBoxView;
            this.gameStarted = gameStarted;
            gameStarted.Subscribe(_ => Show());
        }

        private void Show()
        {
            this.clownBoxView.Show();
        }
    }
}