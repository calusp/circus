using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TriggerActivator : MonoBehaviour
{

    [SerializeField] private GameObject activable;

    private BoxCollider2D trigger;

    private void Awake()
    {
        activable.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        trigger = GetComponent<BoxCollider2D>();
        trigger.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name.Contains("Player"))
            activable.SetActive(true);
    }
}
