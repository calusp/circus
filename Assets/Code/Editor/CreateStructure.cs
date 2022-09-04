using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Windows;

namespace Assets.Code.Editor
{
    public class CreateStructure : ScriptableObject
    {
        [MenuItem("Tools/Chunks/Create Structure/Left")]
        static void CreateStructureLeft()
        {
            var structure = new GameObject("Structure Left");
            StageUtility.PlaceGameObjectInCurrentStage(structure);
            var view = structure.AddComponent<StructureView>();
            view.transform.position = new Vector2(-2.5f, -1.85f);
            view.ChunkSide = StructureView.Side.Left;
        }

        [MenuItem("Tools/Chunks/Create Structure/Rigth")]
        static void CreateStructureRight()
        {
            var structure = new GameObject("Structure Right");
            StageUtility.PlaceGameObjectInCurrentStage(structure);
            var view =  structure.AddComponent<StructureView>();
            view.transform.position = new Vector2(2.5f, -1.85f);
            view.ChunkSide = StructureView.Side.Right;

        }
        public static void Create(GameObject structure)
        {
            if (!Directory.Exists("Assets/Prefabs"))
                AssetDatabase.CreateFolder("Assets", "Prefabs");
            string localPath = "Assets/Prefabs/Chunks/StructuresPrefabs/" + structure.name + ".prefab";

            // Make sure the file name is unique, in case an existing Prefab has the same name.
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            // Create the new Prefab and log whether Prefab was saved successfully.
            bool prefabSuccess;
            PrefabUtility.SaveAsPrefabAssetAndConnect(structure, localPath, InteractionMode.UserAction, out prefabSuccess);
            if (prefabSuccess == true)
                Debug.Log("Prefab was saved successfully");
            else
                Debug.Log("Prefab failed to save" + prefabSuccess);
        }
    }
}