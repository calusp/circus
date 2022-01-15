﻿using System.Collections;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Game Configuration")]
    public class GameConfiguration : ScriptableObject
    {
        [SerializeField, Range(0,50)] float playerSpeed = 3;
        [SerializeField, Range(0, 50)] float cameraSpeed = 3;
        [SerializeField, Range(0, 50)] float incrementalRatio = 3;
        [SerializeField, Range(0, 50)] float distanceCap = 3;

        [Header("Power Values, keep it below 2")]
        [SerializeField] Vector2 jumpForce;
        [SerializeField] Vector2 trampoolineForce;

        public float PlayerSpeed => playerSpeed;
        public float CameraSpeed => cameraSpeed;
        public float IncrementalRatio => incrementalRatio;
        public float DistanceCap => distanceCap;
        public Vector2 JumpForce => jumpForce;
        public Vector2 TrampolineForce => trampoolineForce;

    }
}