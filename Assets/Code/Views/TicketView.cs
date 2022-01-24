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

        private void Start()
        {
            _trigger = GetComponent<BoxCollider2D>();
            _trigger.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.name.Contains("Player"))
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}

