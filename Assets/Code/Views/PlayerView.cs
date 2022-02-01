using Code.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UniRx;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Code.Views
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D body;

        internal void GetPlayerInCannon(Vector2 cannon)
        {
            throw new NotImplementedException();
        }

        [SerializeField] private Vector2 jumpForce;
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private DisplayableData displyableData;
        private float _newPlayerPositionXAxis;

        public void Awake()
        {
            Init();
        }

        public Action<bool> IsGrounded { get; set; }
        public float DistanceTravelled { get; private set; }

        public void Jump(float powerX, float powerY)
        {
            body.velocity = Vector2.zero;
            body.AddForce(new Vector2(jumpForce.x * powerX, jumpForce.y * (1 + powerY)),
                ForceMode2D.Impulse);
        }

        private void CheckGrounded(Vector3 position)
        {
            List<RaycastHit2D> hits = CreateRays(position);
            hits.ForEach(hit => Debug.DrawRay(hit.centroid, Vector3.down));
            var isSteppedOnSomething = hits.Any(hit => hit.collider && hit.distance < 0.52f);
            IsGrounded(isSteppedOnSomething);
            if (!isSteppedOnSomething) return;
            if (ColliderHasActionable(hits))
                hits.First(hit => hit.collider && hit.collider.GetComponent<Actionable>() != null).collider
                    .GetComponent<Actionable>()
                    .Execute();

        }

        public IObservable<Unit> DieSmashed()
        {
            return Die().ToObservable();
        }

        IEnumerator Die()
        {
            Debug.Log("Player died");
            yield return null;
        }

        private static List<RaycastHit2D> CreateRays(Vector3 position) =>new List<RaycastHit2D>()
        {
                Physics2D.Raycast(new Vector2(position.x - 0.5f, position.y), Vector2.down, 1, 1 << 9),
                Physics2D.Raycast(new Vector2(position.x, position.y), Vector2.down, 1, 1 << 9),
                Physics2D.Raycast(new Vector2(position.x + 0.5f, position.y), Vector2.down, 1, 1 << 9)
          };

        private  bool ColliderHasActionable(IEnumerable<RaycastHit2D> hits) => 
            hits.Any(hit => hit.collider && hit.collider.GetComponent<Actionable>() != null);

        private void Update()
        {
            CheckGrounded(transform.position);
        }

        private void FixedUpdate()
        {
            transform.position = new Vector2(_newPlayerPositionXAxis, transform.position.y);
            displyableData.DistanceTravelled = Mathf.Abs( transform.position.x - startPosition.x);
        }

        public void Init()
        {
            displyableData.DistanceTravelled = 0;
            _newPlayerPositionXAxis = startPosition.x;
            transform.position = startPosition;
        }

        public void Move(float amount)
        {
            _newPlayerPositionXAxis = transform.position.x + amount;

        }
    }
}