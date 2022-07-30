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
        [SerializeField] private EndGameHudView endGameHudView;
        [SerializeField] private SharedGameState sharedGameState;
        [SerializeField] private BestGamePoints bestGamePoints;
        [SerializeField] private AudioCenter audioCenter;
        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            sharedGameState.Initialize();
            var gameStarted = new Subject<Unit>();
            var gameFinished = new Subject<Unit>();
            var backToMainMenu = new Subject<Unit>();
            var startGameHudPresenter = new StartGameHubPresenter(startGameHudView, gameStarted, backToMainMenu, audioCenter);
            var endGameHudPresenter = new EndGameHubPresenter(endGameHudView, gameStarted, gameFinished, backToMainMenu, bestGamePoints);
            startGameHudPresenter.Initialize();
            endGameHudPresenter.Setup();
            var gamePlay = new GamePlay(gamePlayView, gameStarted, gameFinished, gameConfiguration, sharedGameState, backToMainMenu);
        }
    }
}