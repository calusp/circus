using System.Collections;
using UnityEngine;

namespace Assets.Code.Utils
{
    public class StructureSize : MonoBehaviour
    {
        [SerializeField] private Vector2 size;
        [SerializeField] private Vector2 position;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position,new Vector2(5,4.4f));
        }
    }
}