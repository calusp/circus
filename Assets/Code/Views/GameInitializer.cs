using System;
using Code.Presenters;
using UniRx;
using UnityEngine;

namespace Code.Views
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private StartGameHudView startGameHudView;
        [SerializeField] private GamePlayView gamePlayView;
        private void Start()
        {
            var gameStarted = new Subject<Unit>();
            var gameFinished = new Subject<Unit>();
            var startGameHudPresenter = new StartGameHubPresenter(startGameHudView, gameStarted,gameFinished);
            startGameHudPresenter.Initialize();
            var gamePlay = new GamePlay(gamePlayView, gameStarted, gameFinished);
        }
    }
}