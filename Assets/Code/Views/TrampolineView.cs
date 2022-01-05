using System;
using Code.Presenters;
using UnityEngine;

namespace Code.Views
{
    public class TrampolineView : BaseActionableView
    {
        private PlayerPresenter _playerPresenter;

        public override void Execute()
        {
            _playerPresenter.TrampolineJump();
        }

        public override void Attach(PlayerPresenter playerPresenter)
        {
            _playerPresenter = playerPresenter;
        }
        
    }
    
}
