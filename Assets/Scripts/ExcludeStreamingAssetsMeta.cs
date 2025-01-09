#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class ExcludeStreamingAssetsMeta : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (var importedAsset in importedAssets)
        {
            if (importedAsset.Contains("Assets/StreamingAssets"))
            {
                //Debug.Log(importedAsset);
                // Remove generated meta file
                string metaFilePath = $"{importedAsset}.meta";
                AssetDatabase.MoveAssetToTrash(metaFilePath);
            }
        }
    }
}
#endif