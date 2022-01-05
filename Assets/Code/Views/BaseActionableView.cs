using System;
using Code.Presenters;
using UnityEngine;

namespace Code.Views
{
    public abstract class BaseActionableView : MonoBehaviour, Actionable
    {
        private PlayerPresenter _playerPresenter;

        public virtual void Execute()
        {
            _playerPresenter.TrampolineJump();
        }

        public virtual void Attach(PlayerPresenter playerPresenter)
        {
            _playerPresenter = playerPresenter;
        }
    }

    public interface Actionable
    {
        void Execute();
        void Attach(PlayerPresenter playerPresenter);
    }
}
