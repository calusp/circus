using Code.Presenters;
using UnityEngine;

namespace Code.Views
{
    public abstract class BaseHazardView :MonoBehaviour, Hazard
    {
        public virtual void Execute()
        {
        }

        public virtual void Attach(PlayerPresenter playerPresenter)
        {
        }
    }
}