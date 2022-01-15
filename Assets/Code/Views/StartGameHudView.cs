using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Views
{
    public class StartGameHudView : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private GameObject startGameHud;
        [SerializeField] private Camera camera;
        public Action StartGame { get; set; }

        public void Initialize()
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(StartGame.Invoke);
        }

        public void Hide()
        {
            startGameHud.SetActive(false);
            camera.enabled = false;
        }

        public void Show()
        {
            startGameHud.SetActive(true);
            camera.enabled = true;
        }
    }
}
