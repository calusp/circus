using System;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Presenters
{
    public class InGameMenuPresenter
    {
        private InGameMenuView gameMenuView;
        private readonly AudioCenter audioCenter;

        public InGameMenuPresenter(InGameMenuView gameMenuView, AudioCenter audioCenter)
        {
            this.gameMenuView = gameMenuView;
            this.audioCenter = audioCenter;
            gameMenuView.SwithSoundOnOff = SwitchSound;
            gameMenuView.ReturnToMenu = ReturnToMenu;
            SetVolumeSprite();
        }

        private void ReturnToMenu()
        {
           
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