using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Assets.Code.Editor
{
    public class Scenes : ScriptableObject
    {
        [MenuItem("Scenes/Open Game Scene")]
        static void OpenGameScene()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/GameScene.unity");
        }

        [MenuItem("Scenes/Open Test Scene")]
        static void OpenTestScene()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/TestScene.unity");
        }
    }
}