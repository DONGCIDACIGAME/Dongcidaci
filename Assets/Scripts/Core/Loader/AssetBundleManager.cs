
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// AB包资产管理器
/// </summary>
public class AssetBundleManager: MonoBehaviour
{
    public static AssetBundleManager Ins { get; private set; }

    /// <summary>
    /// AB包
    /// </summary>
    /// <typeparam name="string"></typeparam>
    /// <typeparam name="AssetBundle"></typeparam>
    /// <returns></returns>
    private Dictionary<string, AssetBundle> mName2Bundle = new Dictionary<string, AssetBundle>();

    // 预加载的AB包（比如字体资源，永远不卸载，不更新）
    private Dictionary<string, AssetBundle> mPreloadBundle = new Dictionary<string, AssetBundle>();

    /// <summary>
    /// 异步加载器
    /// </summary>
    /// <returns></returns>
    private UwAsyncLoadTaskMgr mAsyncLoader = new UwAsyncLoadTaskMgr();

    private List<Shader> mShaders = new List<Shader>();


    private AssetBundleRequest mRequest;

    private void Awake()
    {
        if (Ins != null)
        {
            Log.Error(LogLevel.Fatal, "AssetBundleManager Recreate!!");
        }

        Ins = this;
    }

    public void SetReleaseABEnable(bool enable)
    {
        //mDebugNoReleaseAb = enable;
    }

    public void Clear()
    {
        foreach (var pair in mName2Bundle)
        {
            var assetBundle = pair.Value;
            assetBundle.Unload(true);
        }
        mName2Bundle.Clear();
        mAsyncLoader.Clear();
    }

    public void ClearAllRes()
    {
        this.Clear();
        foreach (var pair in mPreloadBundle)
        {
            var assetBundle = pair.Value;
            assetBundle.Unload(true);
        }
        mPreloadBundle.Clear();
    }
    private bool AbHasLoad(string assetBundleName)
    {
        return mName2Bundle.ContainsKey(assetBundleName);
    }

    // 加载基础AB包
    public int LoadInternalAssetBundle(string assetBundleName)
    {
        AssetBundle assetBundle = null;

        int ret = 0;
        if (mName2Bundle.ContainsKey(assetBundleName))
        {
            ret = 2;
            return ret;
        }


        // 再从内部包目录加载
        //string internalUrl = Path.Combine(PathDefine.ASSET_DIR, assetBundleName);
        string internalUrl = PathDefine.STREAMINGASSET_DIR + "/" + assetBundleName + ".variant";
        assetBundle = AssetBundle.LoadFromFile(internalUrl);
        ret = 0;

        if (assetBundle == null)
        {
            ret = -404;
        }
        else
        {
            mName2Bundle.Add(assetBundleName, assetBundle);
        }

        return ret;
    }

    /// <summary>
    /// 异步加载资产
    /// </summary>
    /// <param name="assetPathSource">资产路径</param>
    /// <param name="completeCallback">加载回调</param>
    /// <returns>是否加载成功</returns>
    public bool AsyncLoadAsset(string assetPathSource, FuncAsyncLoadTask completeCallback)
    {
        if (string.IsNullOrEmpty(assetPathSource))
        {
            return false;
        }
        string assetPath = assetPathSource.ToLower().Replace("\\", "/");
        GetBundlePath(assetPath, out string bundleName, out string assetName);
        // 1. 根据资源名判断资源是否是动态资源，如果是动态资源，则载入对应的AB。
        LoadDynamicAssetBundle(bundleName);

        // 2. 继续加载
        if (mName2Bundle.TryGetValue(bundleName, out AssetBundle ab))
        {
            mAsyncLoader.Load(assetName, ab, (UnityEngine.Object obj) => {
                completeCallback(obj); 
            });

            return true;
        }

        return false;
    }

    public void UnLoadAssetBundle(string bundleName, bool enable = true)
    {
        if (mName2Bundle.TryGetValue(bundleName, out AssetBundle ab))
        {
            Log.Logic(LogLevel.Critical,"unload AssetBundle = {0}", bundleName);
            ab.Unload(enable);
            mName2Bundle.Remove(bundleName);
        }
    }

    private void LoadDynamicAssetBundle(string bundleName)
    {
        if (AbHasLoad(bundleName))
        {
            return;
        }

        var ret = LoadInternalAssetBundle(bundleName);
        Log.Logic(LogLevel.Critical,"Load Dynamic bundleName = {0}, ret={1}", bundleName, ret);
    }
    public (int ret, T obj) LoadPreloadAsset<T>(string assetPathSource) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(assetPathSource))
        {
            return (403, null);
        }

        string assetPath = assetPathSource.ToLower().Replace("\\", "/");
        GetBundlePath(assetPath, out string bundleName, out string assetName);
        if (mPreloadBundle.TryGetValue(bundleName, out AssetBundle ab))
        {
            var obj = ab.LoadAsset(assetName);
            var ret = obj as T;
            return (ret == null ? -1: 0, ret);
        }

        return (404, null);
    }

    private void GetBundlePath(string assetPath,out string bundleName, out string assetName)
    {
        bundleName = string.Empty;
        assetName = string.Empty;
        if (string.IsNullOrEmpty(assetPath))
            return;

        int index = assetPath.LastIndexOf('/');
        bundleName = assetPath.Substring(0, index);
        assetName = assetPath.Substring(index+1, assetPath.Length-index-1);
    }

    /// <summary>
    /// 同步加载资产
    /// </summary>
    /// <param name="assetPathSource">资源路径</param>
    /// <param name="ret">返回值加载的对象</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>是否加载成功</returns>
    public int LoadAsset<T>(string assetPathSource, out T ret) where T : UnityEngine.Object
    {       
        if (string.IsNullOrEmpty(assetPathSource))
        {
            ret = null;
            return 403;
        }

        string assetPath = assetPathSource.ToLower().Replace("\\", "/");
        
        GetBundlePath(assetPath,out string bundleName,out string assetName);

        // 1. 根据资源名判断资源是否是动态资源，如果是动态资源，则载入对应的AB。
        LoadDynamicAssetBundle(bundleName);

        // 3. 如果没有，再从磁盘载入
        if (mName2Bundle.TryGetValue(bundleName, out AssetBundle ab))
        {
            var obj = ab.LoadAsset(assetName);

            ret = obj as T;
            return (ret == null) ? -1 : 0;
        }
        else
        {
            ret = null;
            return 404;
        }
    }

    void Update()
    {
        mAsyncLoader.UpdateMgr();

        if (mRequest != null)
        {
            if (!mRequest.isDone) return;

            mShaders.Clear();
            foreach (var asset in mRequest.allAssets)
            {
                var shader = asset as Shader;
                if (shader != null)
                {
                    mShaders.Add(shader);
                }
            }
            mRequest = null;
        }
    }
}
