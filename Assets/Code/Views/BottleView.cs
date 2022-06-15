using Code.Views;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Views
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BottleView : MonoBehaviour
    {
        private BoxCollider2D collider2D;
        // Use this for initialization
        void Start()
        {
            collider2D = GetComponent<BoxCollider2D>();
            collider2D.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var target = collision.GetComponent<PlayerView>();
            target?.DieFromBottle();
        }
    }
}