using System;
using Code.Presenters;
using Code.ScriptableObjects;
using UniRx;
using UnityEngine;

namespace Code.Views
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private StartGameHudView startGameHudView;
        [SerializeField] private GamePlayView gamePlayView;
        [SerializeField] private GameConfiguration gameConfiguration;
        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            var gameStarted = new Subject<Unit>();
            var gameFinished = new Subject<Unit>();
            var startGameHudPresenter = new StartGameHubPresenter(startGameHudView, gameStarted, gameFinished);
            startGameHudPresenter.Initialize();
            var gamePlay = new GamePlay(gamePlayView, gameStarted, gameFinished, gameConfiguration);
        }
    }
}