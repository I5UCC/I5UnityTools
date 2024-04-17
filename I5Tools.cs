#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using VRC.SDK3.Dynamics.PhysBone.Components;
using VRC.Dynamics;
using VRC.SDKBase.Editor;

namespace I5Tools
{
    public class Tools : MonoBehaviour
    {
        [MenuItem("Tools/I5Tools/Optimize Texture Formats", false, 1000)]
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
                TextureImporterPlatformSettings standalone = importer.GetPlatformTextureSettings("Standalone");
                if (standalone == null)
                {
                    Debug.LogWarning("Failed to get Standalone TextureImporterPlatformSettings for asset: " + assetPath);
                    continue;
                }

                switch (importer.textureType)
                {
                    case TextureImporterType.Default:
                        if (!importer.mipmapEnabled || !importer.streamingMipmaps)
                        {
                            Debug.Log("Enabling mipmap streaming: " + assetPath);
                            importer.mipmapEnabled = true;
                            importer.streamingMipmaps = true;
                            changesMade = true;
                        }
                        if (importer.crunchedCompression)
                        {
                            Debug.Log("Uncrunching texture: " + assetPath);
                            importer.crunchedCompression = false;
                            changesMade = true;
                        }
                        if (importer.textureCompression == TextureImporterCompression.Uncompressed)
                        {
                            Debug.Log("Compressing texture: " + assetPath);
                            importer.textureCompression = TextureImporterCompression.Compressed;
                            changesMade = true;
                        }
                        if (importer.textureCompression != TextureImporterCompression.CompressedHQ && importer.DoesSourceTextureHaveAlpha() && importer.alphaSource != TextureImporterAlphaSource.None && importer.sRGBTexture)
                        {
                            Debug.Log("Setting texture compression to BC7: " + assetPath);
                            importer.textureCompression = TextureImporterCompression.CompressedHQ;
                            importer.alphaIsTransparency = true;
                            changesMade = true;
                        }
                        break;
                    case TextureImporterType.NormalMap:
                        if (standalone.format != TextureImporterFormat.BC5 || !standalone.overridden)
                        {
                            Debug.Log("Normal map not in BC5 format: " + assetPath);
                            standalone.overridden = true;
                            standalone.maxTextureSize = importer.maxTextureSize;
                            standalone.format = TextureImporterFormat.BC5;
                            importer.SetPlatformTextureSettings(standalone);
                            changesMade = true;
                        }
                        break;
                    default:
                        break;
                }
            }

            if (changesMade)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        [MenuItem("Tools/I5Tools/Make All Bones Immovable in World Space", false, 1001)]
        public static void MakeAllBonesImmovable()
        {
            VRCPhysBone[] physBones = FindObjectsOfType<VRCPhysBone>();
            foreach (VRCPhysBone physBone in physBones)
            {
                physBone.immobileType = VRCPhysBoneBase.ImmobileType.World;
                physBone.immobile = 1.0f;
            }
        }

        [MenuItem("Tools/I5Tools/Delete All OSC Config Files", false, 1002)]
        public static void DeleteAllOscConfigFiles()
        {
            string oscfolderpath = VRC_SdkBuilder.GetLocalLowPath() + "/VRChat/VRChat/OSC/";
            Debug.Log("Deleting all usr_* folders in " + oscfolderpath);
            string[] oscfolders = System.IO.Directory.GetDirectories(oscfolderpath);
            oscfolders = System.Array.FindAll(oscfolders, s => s.Contains("usr_"));
            if (oscfolders.Length == 0)
            {
                Debug.Log("No OSC folders found");
                return;
            }
            foreach (string oscfolder in oscfolders)
            {
                Debug.Log("Deleting " + oscfolder);
                System.IO.Directory.Delete(oscfolder, true);
            }
        }

        [MenuItem("Tools/I5Tools/Force Scene View in Play Mode", false, 1003)]
        public static void CreateDebugManager()
        {
            GameObject debugManager = new GameObject("ForceSceneView");
            debugManager.AddComponent<ForceSceneView>();
        }
    }
}
#endif