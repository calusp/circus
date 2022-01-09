using System;
using Code.Presenters;
using UnityEngine;

namespace Code.Views
{
    public abstract class BaseActionableView : MonoBehaviour, Actionable
    {
        public virtual void Execute()
        {
        }

        public virtual void Attach(PlayerPresenter playerPresenter)
        {
        }
    }

    public interface Actionable
    {
        void Execute();
        void Attach(PlayerPresenter playerPresenter);
    }
}
