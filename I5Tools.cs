#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using VRC.SDK3.Dynamics.PhysBone.Components;
using VRC.Dynamics;

namespace I5Tools
{
    public class Tools : MonoBehaviour
    {
        [MenuItem("Tools/I5Tools/Make All Bones Immovable in World Space")]
        public static void MakeAllBonesImmovable()
        {
            VRCPhysBone[] physBones = FindObjectsOfType<VRCPhysBone>();
            foreach (VRCPhysBone physBone in physBones)
            {
                physBone.immobileType = VRCPhysBoneBase.ImmobileType.World;
                physBone.immobile = 1.0f;
            }
        }

        [MenuItem("Tools/I5Tools/Optimize Texture Formats")]
        public static void OptimizeTextureFormats()
        {
            string[] guids = AssetDatabase.FindAssets("t:Texture2D");
            bool changesMade = false;

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (assetPath.Contains("com.unity")) continue;

                TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (importer == null)
                {
                    Debug.LogWarning("Failed to get TextureImporter for asset: " + assetPath);
                    continue;
                }
                else if (importer.textureType != TextureImporterType.NormalMap && importer.textureType != TextureImporterType.Default)
                {
                    continue;
                }
                TextureImporterPlatformSettings standalone = importer.GetPlatformTextureSettings("Standalone");

                if (importer.textureType == TextureImporterType.NormalMap && (standalone.format != TextureImporterFormat.BC5 || !standalone.overridden))
                {
                    Debug.Log("Normal map not in BC5 format: " + assetPath);
                    standalone.overridden = true;
                    standalone.maxTextureSize = importer.maxTextureSize;
                    standalone.format = TextureImporterFormat.BC5;
                    importer.SetPlatformTextureSettings(standalone);
                    changesMade = true;
                }
                else if (importer.textureType == TextureImporterType.Default && importer.textureCompression != TextureImporterCompression.CompressedHQ && importer.DoesSourceTextureHaveAlpha() && importer.alphaSource != TextureImporterAlphaSource.None && importer.sRGBTexture)
                {
                    Debug.Log("Texture with alpha not in BC7 format: " + assetPath);
                    importer.textureCompression = TextureImporterCompression.CompressedHQ;
                    importer.alphaIsTransparency = true;
                    changesMade = true;
                }
            }

            if (changesMade)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        [MenuItem("Tools/I5Tools/Force Scene View in Play Mode")]
        public static void CreateDebugManager()
        {
            GameObject debugManager = new GameObject("ForceSceneView");
            debugManager.AddComponent<ForceSceneView>();
        }
    }
}
#endif