using System.Collections;
using UnityEngine;

namespace Assets.Code.Views
{
    public class PlayerBounds : MonoBehaviour
    {
        public Vector3 size = Vector3.one;

        public float RightBound => transform.position.x + size.x;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, size);
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}