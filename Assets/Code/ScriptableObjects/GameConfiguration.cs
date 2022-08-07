using System;
using System.Collections;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Game Configuration")]
    public class GameConfiguration : ScriptableObject
    {
        [SerializeField, Range(0, 50)] float playerSpeed = 3;
        [SerializeField, Range(0, 50)] float cameraSpeed = 3;
        [SerializeField, Range(0, 50)] float incrementalRatio = 3;
        [SerializeField, Range(0, 50)] float distanceCap = 3;
        [SerializeField, Range(0, 50)] float cannonSpeed;
        [SerializeField, Range(0, 1)] float trapeceSpeed;

        [Header("Power Values, keep it below 2")]
        [SerializeField] Vector2 jumpForce;
        [SerializeField] Vector2 trampoolineForce;
        [SerializeField] Vector2 cannonForce;

        internal void SetPlayerSpeed(float speed)
        {
            playerSpeed = speed;
        }

        [SerializeField] SeesawBallConfiguration seesawBallConfiguration;
        [SerializeField] private Vector2 trapeceForce;
        [SerializeField] private float _bagDropDistance;
        [SerializeField] private AnimationCurve incrementCurve;
        [SerializeField] private DisplayableData distanceData;
        public float PlayerSpeed => playerSpeed;

        public Vector2 JumpForce => jumpForce;
        public Vector2 TrampolineForce => trampoolineForce;

        public float CannonSpeed => cannonSpeed;
        public Vector2 CannonForce => cannonForce;

        public float TrapeceSpeed => trapeceSpeed;
        public Vector2 TrapeceForce => trapeceForce;

        internal void SetChunkSpeed(float speed)
        {
            cameraSpeed = speed;
        }

        public SeesawBallConfiguration SeesawBallConfiguration => seesawBallConfiguration;

        public float BagDropDistance => _bagDropDistance;

        public float CalculateIncrement(float distanceTravelled) =>
            incrementCurve.Evaluate(incrementalRatio * distanceTravelled / distanceCap);
        public float Increment => CalculateIncrement(distanceData.Content);

        internal void SetIncrementRatio(float ratio)
        {
            incrementalRatio = ratio;
        }

        public float ChunkSpeed => CameraSpeed + Increment;
        public float CameraSpeed => cameraSpeed;

        public float DistanceCap => distanceCap;

        internal void SetDistanceCap(float distance)
        {
            distanceCap = distance;
        }

        internal void SetJumpForceYValue(float v)
        {
            jumpForce = new Vector2(jumpForce.x, v);
        }

        internal void SetJumpForceXValue(float v)
        {
            jumpForce = new Vector2(v, jumpForce.y);
        }
    }
}