using UnityEngine;
using UnityEditor;
using UnityEditor.Formats.Fbx.Exporter;
using System.IO;

public class MeshToFbxExporter
{
    [MenuItem("Tools/Export Selected Meshes to FBX")]
    private static void ExportSelectedMeshes()
    {
        string exportFolder = "Assets/Exported";
        if (!AssetDatabase.IsValidFolder(exportFolder))
        {
            AssetDatabase.CreateFolder("Assets", "Exported");
        }

        foreach (GameObject obj in Selection.gameObjects)
        {
            string localPath = exportFolder + "/" + obj.name + ".fbx";

            var prefab = PrefabUtility.SaveAsPrefabAsset(obj, "Assets/__temp.prefab");
            if (prefab == null)
            {
                Debug.LogError("Failed to create prefab for export: " + obj.name);
                continue;
            }

            ModelExporter.ExportObject(localPath, prefab);
            AssetDatabase.DeleteAsset("Assets/__temp.prefab");
        }

        AssetDatabase.Refresh();
    }
}
