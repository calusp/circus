using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Views
{
    public class StartGameHudView : MonoBehaviour
    {
        public Action StartGame { get; set; }
        public Action SwithSoundOnOff { get; set; }
        public Action<AudioClip> PlayClip { get; set; }
        public AudioClip MenuMusic => menuAudioClip;

        [SerializeField] private Button startButton;
        [SerializeField] private GameObject startGameHud;
        [SerializeField] private Camera camera;
        [SerializeField] private AudioClip menuAudioClip;
        [SerializeField] private AudioClip _startSound;
        [SerializeField] private AudioClip _commonButtonSound;
        [SerializeField] private Sprite volumeOnSprite;
        [SerializeField] private Sprite volumeOffSprite;
        [SerializeField] private Button volumeButton;
      
        public void Initialize()
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(StartGame.Invoke);
            volumeButton.onClick.RemoveAllListeners();
            volumeButton.onClick.AddListener(SwithSoundOnOff.Invoke);
        }

        public void Hide()
        {
            startGameHud.SetActive(false);
            camera.enabled = false;
        }

        public void SwithSoundButton(bool isMuted)
        {
            if (isMuted)
                volumeButton.image.sprite = volumeOffSprite;
            else
                volumeButton.image.sprite = volumeOnSprite;
        }

        public void Show()
        {
            startGameHud.SetActive(true);
            camera.enabled = true;
        }

        private void OnEnable()
        {
           
        }

        public void PlayStartButtonSound()
        {
            PlayClip(_startSound);
        }

        public void PlayCommonButtonSound()
        {
            PlayClip(_commonButtonSound);
        }


    }
}
