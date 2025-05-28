using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectorWindow : EditorWindow
{
    [MenuItem("Tools/Scene Selector")]
    public static void ShowWindow()
    {
        GetWindow<SceneSelectorWindow>("Scene Selector");
    }

    private void OnGUI()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if (GUILayout.Button($"{i}.{sceneName}")) // Нумерация сцен в порядке Build Settings
            {
                EditorSceneManager.OpenScene(scenePath);
            }
        }
    }
}
