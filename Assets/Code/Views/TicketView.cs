using Code.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Views
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TicketView : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _trigger;
        [SerializeField] DisplayableData tickets;
        [SerializeField] AudioClip audioClip;

        AudioCenter audioCenter;
        private void Start()
        {
            _trigger = GetComponent<BoxCollider2D>();
            _trigger.isTrigger = true;
            audioCenter = FindObjectOfType<AudioCenter>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.name.Contains("Player"))
            {
                audioCenter.PlaySoundFx(audioClip);
                gameObject.SetActive(false);
                Destroy(gameObject);
                tickets.Content++;
            }
        }
    }
}

