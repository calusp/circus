using Code.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Code.Views
{
    public class PlayerView : MonoBehaviour
    {
        private readonly int EnterCannonTrigger = Animator.StringToHash("enteringCannon");
        private readonly int WalkTrigger = Animator.StringToHash("walking");
        private readonly int EnterTrapeceTrigger = Animator.StringToHash("enteringTrapece");
        private readonly int DyingSmashed = Animator.StringToHash("dyingSmashed");
        private readonly int DyingBurnt = Animator.StringToHash("dyingBurned");
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Animator animator;
        [SerializeField] private Vector2 jumpForce;
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private DisplayableData displyableData;
        [SerializeField] AnimationClip burnt;
        [SerializeField] AnimationClip smashed;
        private float _newPlayerPositionXAxis;
        private readonly LayerMask layerMask = 1 << 9;
        public Vector2 offset;
        private bool _jumped;
        private float _previousVelocityOnY;

        public Action<bool> IsGrounded { get; set; }
        public Action DieFromSmash { get; set; }
        public float DistanceTravelled { get; private set; }
        public bool IsInsideCannon { get; set; }

        public void Awake()
        {
            Init();
        }

        public void GetPlayerInCannon(Vector2 cannon, Quaternion rotation)
        {
            transform.position = cannon;
            transform.rotation = rotation;
            body.simulated = false;
            SetEnteringCannon();
        }

        public void SetEnteringCannon()
        {
            animator.ResetTrigger(WalkTrigger);
            animator.SetTrigger(EnterCannonTrigger);
        }

        public void Jump(float powerX, float powerY)
        {
            body.simulated = true;
            body.velocity = Vector2.zero;
            body.AddForce(new Vector2(jumpForce.x * powerX, jumpForce.y * (1 + powerY)),
                ForceMode2D.Impulse);
            _jumped = true;

        }

        public IObservable<Unit> DieBurnt()
        {
            animator.ResetTrigger(EnterTrapeceTrigger);
            animator.ResetTrigger(WalkTrigger);
            animator.SetTrigger(DyingBurnt);
            return Die(burnt.length).ToObservable();
        }

        public void Init()
        {
            displyableData.Content = 0;
            _newPlayerPositionXAxis = startPosition.x;
            transform.position = startPosition;
        }

        public void Move(float amount)
        {
            _newPlayerPositionXAxis = transform.position.x + amount;
        }

        public void GetInTrapece(Vector3 trapeceSpot)
        {
            transform.position = trapeceSpot;
            body.simulated = false;
            SetEnteringTrapece();
        }

        private void SetEnteringTrapece()
        {
            animator.ResetTrigger(WalkTrigger);
            animator.SetTrigger(EnterTrapeceTrigger);
        }

        public bool StopMovement { get; set; }
        

        public void RestartMovement()
        {
            _newPlayerPositionXAxis = transform.position.x;
            StopMovement = false;
        }

        public void UpdatePlayerInCannon(Vector3 position, Quaternion rotation)
        {
            transform.rotation = rotation;
            transform.position = position;
        }

        public void SetWalking()
        {
            animator.SetTrigger(WalkTrigger);
        }

        public IObservable<Unit> DieSmashed()
        {
            animator.ResetTrigger(WalkTrigger);
            animator.SetTrigger(DyingSmashed);
            body.simulated = false;
            return Die(smashed.length).ToObservable();
        }


        IEnumerator Die(float lenght)
        {
            yield return new WaitForSeconds(lenght);
        }

        private List<RaycastHit2D> CreateRays(Vector3 position) => new List<RaycastHit2D>()
        {
            Physics2D.Raycast(new Vector2(position.x - 0.5f, position.y), Vector2.down, 0.51f, layerMask),
            Physics2D.Raycast(new Vector2(position.x, position.y), Vector2.down,  0.51f,layerMask),
            Physics2D.Raycast(new Vector2(position.x + 0.5f, position.y), Vector2.down,  0.51f, layerMask)
        };

        internal void Jump(object p1, object p2)
        {
            throw new NotImplementedException();
        }

        private bool ColliderHasActionable(IEnumerable<RaycastHit2D> hits) =>
            hits.Any(hit => hit.collider && hit.collider.GetComponent<Actionable>() != null);

        private void Update()
        {
            List<RaycastHit2D> hits = CreateRays(transform.position);

            bool isGrounded = CheckGrounded(hits);
            IsGrounded(isGrounded);
            if (isGrounded && !StopMovement)
                SetWalking();

            CheckCollisionWithActivable(hits);
        }

        private void FixedUpdate()
        {
            if (StopMovement) return;

            if (transform.position.y < _previousVelocityOnY)
                transform.rotation = Quaternion.identity;

            _previousVelocityOnY = transform.position.y;
        }

        private bool CheckGrounded(List<RaycastHit2D> hits)
        {
            hits.ForEach(hit => Debug.DrawRay(hit.centroid, Vector3.down));
            var isSteppedOnSomething = hits.Any(hit => hit.collider && hit.distance < 0.35f);
            return isSteppedOnSomething;
        }

        private void CheckCollisionWithActivable(List<RaycastHit2D> hits)
        {
            if (ColliderHasActionable(hits))
                hits.First(hit => hit.collider && hit.collider.GetComponent<Actionable>() != null).collider
                    .GetComponent<Actionable>()
                    .Execute();
        }
    }
}