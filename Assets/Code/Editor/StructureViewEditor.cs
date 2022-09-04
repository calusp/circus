using Assets.Code.Editor;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(StructureView))]
public class StructureViewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Create Prefab"))
        {
            StructureView view = (StructureView) target;
            CreateStructure.Create(view.gameObject);  
        }
    }
}

