using System.Collections;
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
        [SerializeField, Range(0, 50)] float cannonSpeed;
        [SerializeField, Range(0, 1)] float trapeceSpeed;

        [Header("Power Values, keep it below 2")]
        [SerializeField] Vector2 jumpForce;
        [SerializeField] Vector2 trampoolineForce;
        [SerializeField] Vector2 cannonForce;
        [SerializeField] SeesawBallConfiguration seesawBallConfiguration;
        [SerializeField] private Vector2 trapeceForce;
        [SerializeField] private float _bagDropDistance;
        [SerializeField] private AnimationCurve incrementCurve;

        public float PlayerSpeed => playerSpeed;
        public float CameraSpeed => cameraSpeed;
        public Vector2 JumpForce => jumpForce;
        public Vector2 TrampolineForce => trampoolineForce;

        public float CannonSpeed => cannonSpeed;
        public Vector2 CannonForce => cannonForce;

        public float TrapeceSpeed => trapeceSpeed;
        public Vector2 TrapeceForce => trapeceForce;
        public SeesawBallConfiguration SeesawBallConfiguration => seesawBallConfiguration;

        public float BagDropDistance => _bagDropDistance;

        public float CalculateIncrement(float distanceTravelled) =>
            //incrementCurve.Evaluate(incrementalRatio * distanceTravelled / distanceCap);
            0;
    }
}