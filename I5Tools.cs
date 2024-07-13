#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using VRC.SDK3.Dynamics.PhysBone.Components;
using VRC.Dynamics;
using VRC.SDKBase.Editor;
using VRC.Core;
using UnityEditor.SceneManagement;

namespace I5Tools
{
    public class Tools : MonoBehaviour
    {
        [MenuItem("Tools/I5Tools/Optimize Texture Formats", false, 1000)]
        public static void OptimizeTextureFormats()
        {
            string[] guids = AssetDatabase.FindAssets("t:Texture2D");

            foreach (string guid in guids)
            {
                bool changesMade = false;
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
                if (changesMade)
                {
                    importer.SaveAndReimport();
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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

        [MenuItem("Tools/I5Tools/Set Anchor/Head", false, 1004)]
        public static void SetAnchorHead() => SetAnchor("Armature/Hips/Spine/Chest/Neck/Head");

        [MenuItem("Tools/I5Tools/Set Anchor/Chest", false, 1005)]
        public static void SetAnchorChest() => SetAnchor("Armature/Hips/Spine/Chest");

        [MenuItem("Tools/I5Tools/Set Anchor/Spine", false, 1006)]
        public static void SetAnchorSpine() => SetAnchor("Armature/Hips/Spine");
        
        private static void SetAnchor(string path)
        {
            PipelineManager[] avatars = FindObjectsOfType<PipelineManager>();
            int avatarCount = avatars.Length;

            for (int i = 0; i < avatarCount; i++)
            {
                GameObject avatarObject = avatars[i].gameObject;
                // find head bone
                Transform anchorBone = avatarObject.transform.Find(path);
                Debug.Log("Found bone: " + anchorBone.name);
                // get All Meshes
                SkinnedMeshRenderer[] skinnedMeshRenderers = avatarObject.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
                {
                    Debug.Log("Setting probeAnchor for " + skinnedMeshRenderer.name + " to " + anchorBone.name + " on " + avatarObject.name);
                    skinnedMeshRenderer.probeAnchor = anchorBone;
                }
                MeshRenderer[] meshRenderers = avatarObject.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer meshRenderer in meshRenderers)
                {
                    Debug.Log("Setting probeAnchor for " + meshRenderer.name + " to " + anchorBone.name + " on " + avatarObject.name);
                    meshRenderer.probeAnchor = anchorBone;
                }
            }

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        
        [MenuItem("Tools/I5Tools/Set Biggest Bounds", false, 1004)]
        public static void SetBiggestBounds()
        {
            PipelineManager[] avatars = FindObjectsOfType<PipelineManager>();
            int avatarCount = avatars.Length;

            for (int i = 0; i < avatarCount; i++)
            {
                GameObject avatarObject = avatars[i].gameObject;
                // get All Meshes
                SkinnedMeshRenderer[] skinnedMeshRenderers = avatarObject.GetComponentsInChildren<SkinnedMeshRenderer>();
                Bounds biggestBounds = new Bounds(Vector3.zero, Vector3.zero);
                foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
                {
                    if (skinnedMeshRenderer.bounds.size.sqrMagnitude > biggestBounds.size.sqrMagnitude)
                    {
                        biggestBounds = skinnedMeshRenderer.localBounds;
                    }
                }
                foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers) 
                {
                    Debug.Log("Setting localBounds for " + skinnedMeshRenderer.name + " to " + biggestBounds.size + " on " + avatarObject.name);
                    skinnedMeshRenderer.localBounds = biggestBounds;
                }
            }

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
#endif