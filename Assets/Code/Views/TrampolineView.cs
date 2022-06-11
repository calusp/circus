using System;
using Code.Presenters;
using UnityEngine;

namespace Code.Views
{
    public class TrampolineView : MonoBehaviour
    {
        private PlayerPresenter _playerPresenter;

        public  void Execute()
        {
            _playerPresenter.TrampolineJump();
        } 
    }
    
}
