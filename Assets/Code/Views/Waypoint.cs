using System.Collections;
using UnityEngine;

namespace Assets.Code.Views
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] private float radius = 0.1f;
        [SerializeField] private Color color = Color.blue;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, radius);
        }
    }
}