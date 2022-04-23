using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Views
{
    public class EndGameHudView : MonoBehaviour
    {
        public Action Restart { get; set; }
        public Action Back { get; set; }
        [SerializeField] Button _reStartButton;
        [SerializeField] Button _backToMenu;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Setup()
        {
            _reStartButton.onClick.RemoveAllListeners();
            _reStartButton.onClick.AddListener(() =>
            {
                Restart();
            }
            );

            _backToMenu.onClick.RemoveAllListeners();
            _backToMenu.onClick.AddListener(() =>
            {
                Back();
            });
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}
