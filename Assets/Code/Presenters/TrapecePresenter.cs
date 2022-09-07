using Code.ScriptableObjects;
using Code.Views;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Presenters
{
    public class TrapecePresenter
    {
        private SharedGameState _sharedGameState;

        public TrapecePresenter(TrapeceView view, SharedGameState sharedGameState)
        {
            _sharedGameState = sharedGameState;
            view.EnterTrapece = EnterTrapece;
            view.TrapecePositionUpdated = UpdateTrapecePosition;
        }

        private void UpdateTrapecePosition(Vector2 position)
        {
            _sharedGameState.UpdateTrapecePosition.OnNext(position);
        }

        public void EnterTrapece(Vector2 position)
        {
            _sharedGameState.EnterTrapece.OnNext(position);
        }

        public void ExecuteAction(Vector2 direction)
        {
            _sharedGameState.ExecuteTrapeceAction.OnNext(direction);
        }
    }
}