// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using UnityEngine;
using UnityEditor;
using System.IO;

namespace ObjectPlacerJobSystem
{
    /// <summary>
    /// http://wiki.unity3d.com/index.php/CreateScriptableObjectAsset
    /// </summary>
    public static class ScriptableObjectUtility
    {
        /// <summary>
        /// This makes it easy to create, name and place unique new ScriptableObject asset files.
        /// </summary>
        public static bool TryCreateJobAsset<T>(string pathToResources, out T asset) where T : ScriptableObject
        {
            asset = null;

            if (string.IsNullOrEmpty(pathToResources))
            {
                Debug.LogWarning($"Given path is empty. Job data gonna be saved under path : {pathToResources}.");
                pathToResources = "Assets/Resources/";
            }
            else if (!pathToResources.Contains("Resources") || Path.GetExtension(pathToResources) != "")
            {
                Debug.LogError($"Path: {pathToResources} to save job data is invalid.");
                return false;
            }

            asset = ScriptableObject.CreateInstance<T>();
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(pathToResources + "/New " + typeof(T) + ".asset");
            AssetDatabase.CreateAsset(asset, assetPathAndName);

            return asset != null;
        }

        public static void SaveAsset(ScriptableObject asset)
        {
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}