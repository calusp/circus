using Code.Presenters;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Views
{
    [RequireComponent(typeof(Collider2D))]
    public class Smash : BaseHazardView
    {
        private Collider2D _collider;
        private PlayerPresenter _playerPresenter;

        // Use this for initialization
        void Start()
        {
            _collider = GetComponent<Collider2D>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            CheckHit(transform.position);
        }

        private void CheckHit(Vector3 position)
        {
            List<RaycastHit2D> hits = CreateRays(position);
            hits.ForEach(hit => Debug.DrawRay(hit.centroid, Vector3.down));
            var isSteppedOnSomething = hits.Any(hit => hit.collider && hit.distance < _collider.bounds.size.y*0.5f);
            if (isSteppedOnSomething)
            {
                enabled = false;
                _playerPresenter.DieSmashed();
            }
        }

        private List<RaycastHit2D> CreateRays(Vector3 position) => new List<RaycastHit2D>()
        {
            Physics2D.Raycast(new Vector2(position.x - _collider.bounds.size.x*0.33f, position.y), Vector2.down, 1, 1 << 8),
            Physics2D.Raycast(new Vector2(position.x, position.y), Vector2.down, 1, 1 << 8),
            Physics2D.Raycast(new Vector2(position.x + _collider.bounds.size.x*0.33f, position.y), Vector2.down, 1, 1 << 8)
        };

        public override void Attach(PlayerPresenter playerPresenter)
        {
            _playerPresenter = playerPresenter;
        }
    }
}