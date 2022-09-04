using Code.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StructureView : MonoBehaviour
{
    public enum Side
    {
        Left,
        Right
    }

    [SerializeField] private Side side;

    public Side ChunkSide { get; set; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector2(5, 4.4f));
    }
}
