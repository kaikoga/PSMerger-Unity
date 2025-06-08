using System.IO;
using ClusterVR.CreatorKit.Item.Implements;
using UnityEditor;
using UnityEngine;

namespace Silksprite.PSMerger
{
    public static class PSMergerUtil
    {
        public static JavaScriptAsset CreateJavaScriptAsset(string assetPath)
        {
            var actualPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);
            File.WriteAllText(actualPath, "");
            AssetDatabase.ImportAsset(actualPath);
            var javaScriptAsset = AssetDatabase.LoadAssetAtPath<JavaScriptAsset>(actualPath);
            return javaScriptAsset;
        }
    }
}
