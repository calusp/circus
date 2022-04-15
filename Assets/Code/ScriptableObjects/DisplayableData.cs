using System;
using System.Collections;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DisplayableData")]
    public class DisplayableData : ScriptableObject
    {
        [SerializeField] private bool isInterger;
        public float Content; //{ get; set; }

        public string DisplayContent() =>
            isInterger ? Content.ToString("0") : Content.ToString("0.00");
    }
}