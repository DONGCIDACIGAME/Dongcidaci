using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

public class AssetBundlePacker : Editor
{
#if UNITY_EDITOR
    #region ���
    public static string[] textureExtensions = new[] { ".png" };


    [MenuItem("BuildAssetBundle/ScanSpriteAtlas")]
    public static void ScanSpriteAtlas()
    {
        var path = Path.Combine(Application.dataPath, "AssetBundles");
        var dirInfo = new DirectoryInfo(path);
        ScanSprites(dirInfo);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static void ScanSprites(DirectoryInfo dirInfo)
    {
        var dirArr = dirInfo.GetDirectories();
        if (dirArr.Length > 0)
        {
            for (int i = 0; i < dirArr.Length; i++)
            {
                ScanSprites(dirArr[i]);
            }
        }
        var fileArr = dirInfo.GetFiles();
        if (fileArr.Length > 0)
        {
            var dirPath = dirInfo.FullName.Replace("\\", "/");
            var subDirPath = dirPath.Replace(Application.dataPath, "Assets");
            for (int i = 0; i < fileArr.Length; i++)
            {
                var label = "";
                var variant = "";
                if (textureExtensions.Contains(fileArr[i].Extension))
                {
                    var texImporter = AssetImporter.GetAtPath(Path.Combine(subDirPath, fileArr[i].Name)) as TextureImporter;
                    if (texImporter.textureType == TextureImporterType.Sprite)
                    {
                        GetSpriteAtlas(dirInfo);
                        break;
                    }
                }
            }
        }
    }

    public static SpriteAtlas GetSpriteAtlas(DirectoryInfo dirInfo)
    {
        //�ļ���·��
        var assetDataPath = dirInfo.FullName.Replace("\\", "/").Replace(Application.dataPath, "Assets");
        //ͼ������
        var atlasName = assetDataPath.Replace("Assets/AssetBundles/", "").Replace("/", "-");
        //ͼ��·��
        var assetPath = $"{assetDataPath}/{atlasName}.spriteatlas";
        //����ͼ��
        var sa = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath);
        if (sa == null)
        {
            Debug.Log($"Creat SpriteAtlas at path : {assetDataPath}");
            sa = new SpriteAtlas();
            AssetDatabase.CreateAsset(sa, assetPath);
            UnityEngine.Object texture = AssetDatabase.LoadMainAssetAtPath(assetDataPath);
            SpriteAtlasPackingSettings packset = new SpriteAtlasPackingSettings()
            {
                blockOffset = 1,
                enableRotation = false,
                enableTightPacking = false,
                padding = 4
            };
            sa.SetPackingSettings(packset);
            SpriteAtlasTextureSettings texSet = new SpriteAtlasTextureSettings()
            {
                readable = true,
                filterMode = FilterMode.Bilinear,
                sRGB = true,
                generateMipMaps = true
            };
            sa.SetTextureSettings(texSet);
            sa.SetIncludeInBuild(false);
            sa.SetIsVariant(false);
            sa.Add(new UnityEngine.Object[] { texture });
        }
        return sa;
    }


    public static void AutoSetBundleName(bool clear)
    {
        string path = Path.Combine(Application.dataPath, "AssetBundles");
        DirectoryInfo info = new DirectoryInfo(path);
        ScanDir(info, clear);
    }

    public static string[] IgnoreExtensions = new[] { ".meta", ".DS_Store" };
    public static string[] VideoExtensions = new[] { ".mp4" };
    public static List<FileInfo> videoFileList = new List<FileInfo>();
    public static void ScanDir(DirectoryInfo info, bool clear, bool smartSet = true)
    {
        DirectoryInfo[] dirArr = info.GetDirectories();
        //�ݹ���������ļ���
        for (int i = 0; i < dirArr.Length; i++)
        {
            ScanDir(dirArr[i], clear);
            //��ʾ������
            EditorUtility.DisplayProgressBar("ɨ����Դ�ļ���", $"��Դ�ļ�������{dirArr[i].Name}", i * 1.0f / dirArr.Length);
        }
        FileInfo[] fileArr = info.GetFiles();
        if (fileArr.Length > 0)
        {
            string dirPath = info.FullName.Replace("\\", "/");
            string subDirPath = dirPath.Replace(Application.dataPath, "Assets");
            string assetBundleLabel = subDirPath.Replace("Assets/AssetBundles/", "").ToLower();
            //�����ļ�����ӱ�ǩ���Զ����
            for (int i = 0; i < fileArr.Length; i++)
            {
                if (IgnoreExtensions.Contains(fileArr[i].Extension))
                {
                    continue;
                }
                string label = "None";
                string variant = "None";
                if (!clear)
                {
                    if (textureExtensions.Contains(fileArr[i].Extension))
                    {
                        //����·����ȡָ����Դ
                        var texImporter = AssetImporter.GetAtPath(Path.Combine(subDirPath, fileArr[i].Name)) as TextureImporter;
                        if (texImporter.textureType == TextureImporterType.Sprite)
                        {
                            continue;
                        }
                    }
                    label = assetBundleLabel;
                    variant = "variant";
                }
                string assetPath = $"{subDirPath}/{fileArr[i].Name}";
                AssetImporter asset = AssetImporter.GetAtPath(assetPath);
                Debug.Log($"FileName:{fileArr[i].Name},Extension:{fileArr[i].Extension}");
                if (asset.assetBundleName != label || !smartSet)
                {
                    try
                    {
                        asset.SetAssetBundleNameAndVariant(label, variant);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                        EditorUtility.ClearProgressBar();
                    }
                }
                Debug.Log($"����AB���� ������{label}");
                EditorUtility.DisplayProgressBar("����AB����", $"������{label}", i * 1.0f / fileArr.Length);
            }
        }
        EditorUtility.ClearProgressBar();
    }

    [MenuItem("BuildAssetBundle/BuildStreamingAssets", false, 0)]
    public static void BuildWindowsBundles()
    {
        //AutoSetBundleName(false);
        BuildAllAssetBundleToPersistent(BuildAssetBundleOptions.ForceRebuildAssetBundle);
    }

    private static  void CreateDirectory(string dirPath)
    {
        if (Directory.Exists(dirPath))
            return;

        Directory.CreateDirectory(dirPath);
    }

    public static void BuildAllAssetBundleToPersistent(BuildAssetBundleOptions bundleOptions)
    {
        string packPath = "Assets/StreamingAssets";
        CreateDirectory(packPath);
        if (packPath.Length <= 0)
            return;
        Debug.Log($"OutPath:{packPath}");
        BuildPipeline.BuildAssetBundles(packPath, bundleOptions, GetCurrBuildTarget());
        AssetDatabase.Refresh();
    }

    private static BuildTarget GetCurrBuildTarget()
    {
        return BuildTarget.StandaloneWindows64;
    }



    #endregion
#endif
}
