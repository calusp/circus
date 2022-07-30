using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InGameMenuView : MonoBehaviour
{
    public Action SwithSoundOnOff { get; set; }
    public Action ReturnToMenu { get; set; }

    [SerializeField] private GameObject menuBackground;
    [SerializeField] private Button button;
    [SerializeField] private Button volumeButton;
    [SerializeField] private Button returnToMenuButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Image background;
    [SerializeField] private Animator animator;
    [SerializeField] private Sprite volumeOnSprite;
    [SerializeField] private Sprite volumeOffSprite;
    private bool isClosed;
   
    // Start is called before the first frame update
    public void Initialize()
    {
        isClosed = true;

        volumeButton.image.enabled = false;
        returnToMenuButton.image.enabled = false;
        continueButton.image.enabled = false;
        background.fillAmount = 0;

        button.onClick.RemoveAllListeners();
        volumeButton.onClick.RemoveAllListeners();
        continueButton.onClick.RemoveAllListeners();
        returnToMenuButton.onClick.RemoveAllListeners();

        button.onClick.AddListener(SwitchOpenClose);
        volumeButton.onClick.AddListener(() => SwithSoundOnOff());
        continueButton.onClick.AddListener(() => SwitchOpenClose());
        returnToMenuButton.onClick.AddListener(() => {
            ReturnToMenu(); 
        });
    }

    public void SwithSoundButton(bool isMuted)
    {
        if (isMuted)
            volumeButton.image.sprite = volumeOffSprite;
        else
            volumeButton.image.sprite = volumeOnSprite;
    }

    private void SwitchOpenClose()
    {
        if(isClosed)
            animator.SetTrigger("openning");
        else
            animator.SetTrigger("closing");

        isClosed = !isClosed;
    }
}
