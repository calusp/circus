using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Assets.Code.Presenters
{
    public class InGameMenuPresenter
    {
        private InGameMenuView gameMenuView;
        private readonly AudioCenter audioCenter;
        private readonly ISubject<Unit> backToMenu;

        public InGameMenuPresenter(InGameMenuView gameMenuView, AudioCenter audioCenter, ISubject<Unit> backToMenu)
        {
            this.gameMenuView = gameMenuView;
            this.audioCenter = audioCenter;
            this.backToMenu = backToMenu;
            gameMenuView.SwithSoundOnOff = SwitchSound;
            gameMenuView.ReturnToMenu = ReturnToMenu;
            SetVolumeSprite();
            gameMenuView.Initialize();
        }

        private void ReturnToMenu()
        {
           backToMenu.OnNext(Unit.Default);
        }

        private void SwitchSound()
        {
            audioCenter.SwithSoundOnOff();
            SetVolumeSprite();
        }

        private void SetVolumeSprite()
        {
            gameMenuView.SwithSoundButton(audioCenter.IsBackgroundMuted());
        }
    }
}