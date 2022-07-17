using Code.Views;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Code.Presenters
{
    public class EndGameHubPresenter
    {
        private readonly EndGameHudView view;
        private readonly ISubject<Unit> gameStarted;
        private readonly ISubject<Unit> gameFinished;
        private readonly Subject<Unit> backToMainMenu;
        private readonly BestGamePoints bestGamePoints;

        public EndGameHubPresenter(EndGameHudView view, ISubject<Unit> gameStarted, ISubject<Unit> gameFinished, Subject<Unit> backToMainMenu, BestGamePoints bestGamePoints)
        {
            this.view = view;
            this.gameStarted = gameStarted;
            this.gameFinished = gameFinished;
            this.backToMainMenu = backToMainMenu;
            this.bestGamePoints = bestGamePoints;
            view.Restart = Restart;
            view.Back = Back;
        }

        private void Back()
        {
            view.Hide();
            backToMainMenu.OnNext(Unit.Default);
        }

        public void Setup()
        {
           view.Setup();
           gameFinished.Subscribe(_=> {
               bestGamePoints.SaveBestGame();
               view.Show();
               });
        }

        public void Restart()
        {
            view.Hide();
            gameStarted.OnNext(Unit.Default);
        }
    }
}