using Code.Views;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace Assets.Code.Editor
{
    public class CreateChunk : ScriptableObject
    {
        [MenuItem("Tools/Chunks/Create Chunk")]
        static void CreatePrefab()
        {
            GameObject chunk = CreateChunkPrefab();

            AddFloor(chunk);

            string localPath = GetLocalPath(chunk);

            CreateAndLoad(chunk, localPath);
            
        }

        private static GameObject CreateChunkPrefab()
        {
            var chunk = new GameObject("Chunk");
            chunk.AddComponent<ChunkView>();
            return chunk;
        }

        private static void AddFloor(GameObject chunk)
        {
            var floor = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Chunks/GameChunks/Floor 1.prefab");
            var floorPrefab = PrefabUtility.InstantiatePrefab(floor, chunk.transform);
        }

        private static string GetLocalPath(GameObject chunk)
        {
            if (!Directory.Exists("Assets/Prefabs"))
                AssetDatabase.CreateFolder("Assets", "Prefabs");
            string localPath = "Assets/Prefabs/Chunks/GameChunks/" + chunk.name + ".prefab";

            // Make sure the file name is unique, in case an existing Prefab has the same name.
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            return localPath;
        }

        private static void CreateAndLoad(GameObject chunk, string localPath)
        {
            // Create the new Prefab and log whether Prefab was saved successfully.
            bool prefabSuccess;
            PrefabUtility.SaveAsPrefabAssetAndConnect(chunk, localPath, InteractionMode.UserAction, out prefabSuccess);
            if (prefabSuccess == true)
            {
                AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<GameObject>(localPath));
            }
            else
                Debug.Log("Prefab failed to save" + prefabSuccess);
        }
    }
}