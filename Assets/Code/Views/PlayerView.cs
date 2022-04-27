using Assets.Code.Views.TargetSystem;
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
        private readonly int DyingKnifed = Animator.StringToHash("dyingKnifed");
        private readonly int Stopped = Animator.StringToHash("stopped");

        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Animator animator;
        [SerializeField] private Vector2 jumpForce;
        [SerializeField] private Vector3 startPosition;
        [SerializeField] AnimationClip burnt;
        [SerializeField] AnimationClip smashed;
        [SerializeField] GameConfiguration gameConfiguration;
        [SerializeField] SharedGameState sharedGameState;
        [SerializeField] DisplayableData distance;
        [SerializeField] private float _moveSpeed;

        private readonly LayerMask layerMask = 1 << 9;
        private float _previousVelocityOnY;
       

        public Action<bool> IsGrounded { get; set; }
        public Action DieFromSmash { get; set; }
        public bool StopMovement { get; set; }
        public Action<KnifeView> DieFromKnife { get; set; }

        public void Awake()
        {
            Init();
        }

        public IObservable<Unit> DieKnifed(KnifeView knifeView)
        {
            StopMovement = true;
            knifeView.gameObject.SetActive(false);
            sharedGameState.JustDied = true;
            animator.ResetTrigger(WalkTrigger);
            animator.SetTrigger(DyingKnifed);
            return MoveWithKnife(knifeView).ToObservable();
        }

        private IEnumerator MoveWithKnife(KnifeView knifeView)
        {
            yield return new WaitForSeconds(2);
        }

        public void Stop()
        {
            _moveSpeed = _moveSpeed  == 0 && !sharedGameState.JustDied ? (gameConfiguration.CalculateIncrement(distance.Content) + gameConfiguration.CameraSpeed )* -1 : 0;
            if(_moveSpeed == 0)
            {
                animator.ResetTrigger(WalkTrigger);
                animator.SetTrigger(DyingKnifed);
            }
            else
            {
                animator.SetTrigger(WalkTrigger);
            }
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
            _moveSpeed = 0;
            body.simulated = true;
            body.velocity = Vector2.zero;
            body.AddForce(new Vector2(jumpForce.x * powerX, jumpForce.y * (1 + powerY)),
                ForceMode2D.Impulse);

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
            transform.position = startPosition;
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

        public void RestartMovement()
        {
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


        private bool ColliderHasActionable(IEnumerable<RaycastHit2D> hits) =>
            hits.Any(hit => hit.collider && hit.collider.GetComponent<Actionable>() != null);

        private void Update()
        {
            List<RaycastHit2D> hits = CreateRays(transform.position);
            transform.position = new Vector3(transform.position.x + _moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            bool isGrounded = CheckGrounded(hits);
            IsGrounded(isGrounded);

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