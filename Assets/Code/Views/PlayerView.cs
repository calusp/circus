using Assets.Code.ScriptableObjects;
using Assets.Code.Views;
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
        private readonly int JumpTrigger = Animator.StringToHash("jumping");
        private readonly int EnterTrapeceTrigger = Animator.StringToHash("enteringTrapece");
        private readonly int DyingSmashed = Animator.StringToHash("dyingSmashed");
        private readonly int DyingBottle = Animator.StringToHash("dyingBottle");
        private readonly int DyingBanana = Animator.StringToHash("dyingBanana");
        private readonly int DyingBurnt = Animator.StringToHash("dyingBurned");
        private readonly int DyingKnifed = Animator.StringToHash("dyingKnifed");
        private readonly int StopTrigger = Animator.StringToHash("stopped");

        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Animator animator;
        [SerializeField] private Vector2 jumpForce;
        [SerializeField] private Vector3 startPosition;
        [SerializeField] AnimationClip burnt;
        [SerializeField] AnimationClip smashed;
        [SerializeField] AnimationClip fallBanana;
        [SerializeField] private AnimationClip bottlePinched;
        [SerializeField] SharedGameState sharedGameState;
        [SerializeField] private PlayerBounds playerBounds;
        [SerializeField] PlayerSounds playerSounds;
        AudioCenter audioCenter;
    

        private readonly LayerMask layerMask = 1 << 9;
        private float _previousVelocityOnY;
        private float updatedPositionOnX;
     

        public Action<bool> IsGrounded { get; set; }
        public Action DieFromSmash { get; set; }
        public bool StopMovement { get; set; }
        public Action<KnifeView> DieFromKnife { get; set; }
        public Action DieFromBanana { get;  set; }
        public Action DieFromBottle { get; set; }
        public Action DieBurnt { get; set; }
        public float PositionX => transform.position.x;

       public void Setup(AudioCenter audioCenter)
        {
            this.audioCenter = audioCenter;
        }

        public void Awake()
        {
            Init();
        }

        private void Update()
        {

        }

        private void FixedUpdate()
        {
            if (StopMovement) return;

            List<RaycastHit2D> hits = CreateRays(transform.position);
            bool isGrounded = CheckGrounded(hits);
            IsGrounded(isGrounded);

            CheckCollisionWithActivable(hits);

            if (transform.position.y < _previousVelocityOnY)
                transform.rotation = Quaternion.identity;

            _previousVelocityOnY = transform.position.y;
            var distanceOffset = (transform.position.x + updatedPositionOnX) - playerBounds.RightBound;

            if (distanceOffset > 0)
            {
                //transform.position = new Vector2(transform.position.x - distanceOffset, transform.position.y);
                transform.position = new Vector2(transform.position.x, transform.position.y);
                sharedGameState.PlayerDistanceFromBox.OnNext(distanceOffset);
            }
            else
                transform.position = new Vector2(transform.position.x + updatedPositionOnX, transform.position.y);
        }

        public void Move(float speed)
        {
            updatedPositionOnX = speed * Time.deltaTime;
        }

        public void SetWalking()
        {
            animator.SetTrigger(WalkTrigger);
        }

        public void SetStopping()
        {
            animator.SetTrigger(StopTrigger);
        }

        public IObservable<Unit> DieKnifed(KnifeView knifeView)
        {
            StopMovement = true;
            knifeView.gameObject.SetActive(false);
            sharedGameState.JustDied = true;
            animator.SetTrigger(DyingKnifed);
            audioCenter.PlaySoundFx(playerSounds.DieStabbed);
            return MoveWithKnife(knifeView).ToObservable();
        }

        private IEnumerator MoveWithKnife(KnifeView knifeView)
        {
            yield return new WaitForSeconds(2);
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
            animator.SetTrigger(EnterCannonTrigger);
        }

        public void Jump(float powerX, float powerY)
        {
            audioCenter.PlaySoundFx(playerSounds.Jump);
            animator.SetTrigger(JumpTrigger);
            body.simulated = true;
            body.velocity = Vector2.zero;
            body.AddForce(new Vector2(jumpForce.x * powerX, jumpForce.y * (1 + powerY)),
                ForceMode2D.Impulse);

        }

        public IObservable<Unit> DieFromFireRing()
        {
            audioCenter.PlaySoundFx(playerSounds.DieBurnt);
            animator.ResetTrigger(EnterTrapeceTrigger);
            animator.SetTrigger(DyingBurnt);
            return Die(burnt.length).ToObservable();
        }
        public IObservable<Unit> DieFallBanana()
        {
            audioCenter.PlaySoundFx(playerSounds.DieFellDown);
            animator.SetTrigger(DyingBanana);
            return Die(fallBanana.length).ToObservable();
        }

        public IObservable<Unit> DieBottle()
        {
            audioCenter.PlaySoundFx(playerSounds.DieStabbed);
            animator.SetTrigger(DyingBottle);
            return Die(bottlePinched.length).ToObservable();
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

        public IObservable<Unit> DieSmashed()
        {
            audioCenter.PlaySoundFx(playerSounds.DieSmashed);
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
            Physics2D.Raycast(new Vector2(position.x - 0.25f, position.y), Vector2.down, 0.55f, layerMask),
            Physics2D.Raycast(new Vector2(position.x, position.y), Vector2.down,  0.51f,layerMask),
            Physics2D.Raycast(new Vector2(position.x + 0.25f, position.y), Vector2.down,  0.55f, layerMask)
        };


        private bool ColliderHasActionable(IEnumerable<RaycastHit2D> hits) =>
            hits.Any(hit => hit.collider && hit.collider.GetComponent<Actionable>() != null);


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