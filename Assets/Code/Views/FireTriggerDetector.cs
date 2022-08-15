using Code.Views;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Views
{
    public class FireTriggerDetector : MonoBehaviour
    {
        [SerializeField] BoxCollider2D collider2D;
        // Use this for initialization
        void Start()
        {
            collider2D.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var target = collision.GetComponent<PlayerView>();
            target?.DieBurnt();

        }
    }
}