using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InGameMenuView : MonoBehaviour
{

    [SerializeField] private GameObject menuBackground;
    [SerializeField] private Button button;
    [SerializeField] private Button volumeButton;
    [SerializeField] private Button returnToMenuButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioCenter audioCenter;
    private bool isClosed;

    // Start is called before the first frame update
    void Start()
    {
        isClosed = true;
        button.onClick.AddListener(SwitchOpenClose);
        volumeButton.onClick.AddListener(SwithSoundOnOff);
    }

    private void SwithSoundOnOff()
    {
        audioCenter.SwithSoundOnOff();
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
