using Code.Views;
using System;
using UniRx;
using UnityEngine;

namespace Code.Presenters
{
    public class StartGameHubPresenter
    {
        private readonly StartGameHudView _view;
        private readonly ISubject<Unit> _gameStarted;
        private readonly Subject<Unit> _backToMainMenu;
        private readonly AudioCenter audioCenter;

        public StartGameHubPresenter(StartGameHudView view, ISubject<Unit> gameStarted, Subject<Unit> backToMainMenu, AudioCenter audioCenter)
        {
            _view = view;
            _view.StartGame = StartGame;
            _view.SwithSoundOnOff = SwitchSound;
            _view.PlayClip = PlayClip;
            _gameStarted = gameStarted;
            _backToMainMenu = backToMainMenu;
            this.audioCenter = audioCenter;
        }

        private void PlayClip(AudioClip clip)
        {
            audioCenter.PlaySoundFx(clip);     
        }


        public void Initialize()
        {
            _view.Initialize();
            audioCenter.ChangeBackgroundMusic(_view.MenuMusic);
            _backToMainMenu.Do(_ => _view.Show()).Subscribe();
        }

        private void StartGame()
        {
            _view.PlayStartButtonSound();
            _gameStarted.OnNext(Unit.Default);
            _view.Hide();
        }

        private void SwitchSound()
        {
            audioCenter.SwithSoundOnOff();
            SetVolumeSprite();
        }

        private void SetVolumeSprite()
        {
            _view.SwithSoundButton(audioCenter.IsBackgroundMuted());
        }
    }
}