using Code.Presenters;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Views
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Smash : BaseHazardView
    {
        private Collider2D _collider;
        private Rigidbody2D _rigidBody;
        private PlayerPresenter _playerPresenter;

        // Use this for initialization
        void Start()
        {
            _collider = GetComponent<Collider2D>();
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            CheckHits(transform.position);
        }

        private void CheckHits(Vector3 position)
        {
            HitSomethingVertically(position);
            HitSomethingRight(position);
        }

        private void HitSomethingVertically(Vector3 position)
        {
            List<RaycastHit2D> verticalHits = CreateVerticalRays(position);
            verticalHits.ForEach(hit => Debug.DrawRay(hit.centroid, Vector3.down));
            var hitSomethingVertically = verticalHits.Any(hit => hit.collider && hit.distance < _collider.bounds.size.y * 0.5f);
            if (hitSomethingVertically)
            {
                Debug.Log("Player Hit from top.");
                enabled = false;
                _playerPresenter.DieSmashed();
            }
        }
        private void HitSomethingRight(Vector3 position)
        {
            List<RaycastHit2D> hits = CreateRightHorizontalRays(position);
            hits.ForEach(hit => Debug.DrawLine(hit.centroid, hit.centroid+ Vector2.right, Color.red,1.0f));
            var hitSomething = hits.Any(hit => hit.collider && hit.distance < _collider.bounds.size.x * 0.5f);
            if (hitSomething && _rigidBody.velocity.x > 0)
            {
                Debug.Log("Player Hit from the back.");
                enabled = false;
                _playerPresenter.DieSmashed();
            }
        }

        private List<RaycastHit2D> CreateVerticalRays(Vector3 position) => new List<RaycastHit2D>()
        {
            Physics2D.Raycast(new Vector2(position.x - _collider.bounds.size.x*0.33f, position.y), Vector2.down, 1, 1 << 8),
            Physics2D.Raycast(new Vector2(position.x, position.y), Vector2.down, 1, 1 << 8),
            Physics2D.Raycast(new Vector2(position.x + _collider.bounds.size.x*0.33f, position.y), Vector2.down, 1, 1 << 8)
        };

        private List<RaycastHit2D> CreateRightHorizontalRays(Vector3 position) => new List<RaycastHit2D>()
        {
            Physics2D.Raycast(new Vector2(position.x , position.y- _collider.bounds.size.y*0.33f), Vector2.right, 1, 1 << 8),
            Physics2D.Raycast(new Vector2(position.x, position.y), Vector2.right, 1, 1 << 8),
            Physics2D.Raycast(new Vector2(position.x , position.y+ _collider.bounds.size.y*0.33f), Vector2.right, 1, 1 << 8)
        };

        private List<RaycastHit2D> CreateLeftHorizontalRays(Vector3 position) => new List<RaycastHit2D>()
        {
            Physics2D.Raycast(new Vector2(position.x , position.y- _collider.bounds.size.y*0.33f), Vector2.left, 1, 1 << 8),
            Physics2D.Raycast(new Vector2(position.x, position.y), Vector2.left, 1, 1 << 8),
            Physics2D.Raycast(new Vector2(position.x , position.y+ _collider.bounds.size.y*0.33f), Vector2.left, 1, 1 << 8)
        };

        public override void Attach(PlayerPresenter playerPresenter)
        {
            _playerPresenter = playerPresenter;
        }
    }
}