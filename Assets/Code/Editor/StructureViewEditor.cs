using Assets.Code.Editor;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(StructureView))]
public class StructureViewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}

