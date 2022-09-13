using Assets.Code.Editor;
using Code.Views;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ChunkView))]
public class ChunkViewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Remove Structures"))
        {
            ChunkView view = (ChunkView) target;
            view.RemoveStructures();
        }

        if (GUILayout.Button("Add Prefabs as structures"))
        {
            ChunkView view = (ChunkView)target;
            view.AddStructuresToList();
        }
    }
}

