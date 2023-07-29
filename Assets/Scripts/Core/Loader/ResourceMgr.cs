

using UnityEngine;
using UnityEngine.U2D;
using System.Collections.Generic;
using UnityEditor;

namespace GameEngine
{

#if UNITY_EDITOR
    public partial class ResourceMgr
    {
        private static Dictionary<System.Type, string[]> mEditorAssetTypeExt = new Dictionary<System.Type, string[]>();

        private static string[] mEditorAssetExts = new string[]
        {
        ".prefab",
        ".spriteatlas",
        ".jpg",
        ".png",
        ".ttf",
        ".otf",
        ".mp3",
        ".ogg",
        ".wav",
        ".mat",
        ".controller",
        ".bytes",
        ".asset",
        ".rendertexture",
        ".tga",
        ".tif",
        ".shadervariants",
        };

        public static string[] EditorGetAssetExt<T>()
        {
            string[] exts;
            System.Type k = typeof(T);
            if (!mEditorAssetTypeExt.TryGetValue(k, out exts))
            {
                exts = mEditorAssetExts;
            }

            return exts;
        }

        static private bool IsValidSpriteEditorPath(string assetPath)
        {
            TextureImporter imported = (TextureImporter)TextureImporter.GetAtPath(assetPath);
            if (imported != null)
            {
                if (imported.spritePackingTag.Length > 0)
                {
                    return true;
                }
            }

            return false;
        }

        static public bool EditorCheckExists(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            foreach (string ext in mEditorAssetExts)
            {
                string path = string.Format("{0}{1}{2}", PathDefine.ASSETBUNDLES_ROOT_DIR, url, ext);
                if (System.IO.File.Exists(path))
                {
                    return true;
                }
            }

            return false;
        }

        static private T EditorLoad<T>(string url, string reason) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(url))
            {
                Log.Error(LogLevel.Fatal, "Load Empty Res! reason:{0}", reason);
                return null;
            }

            string[] exts = EditorGetAssetExt<T>();
            foreach (string ext in exts)
            {
                string path = string.Format("{0}{1}{2}", PathDefine.ASSETBUNDLES_ROOT_DIR, url, ext);
                T obj = AssetDatabase.LoadAssetAtPath<T>(path);
                if (obj)
                {
                    if (typeof(T) == typeof(Sprite)) // 对于Sprite类型需要做特殊检查
                    {
                        if (!IsValidSpriteEditorPath(path))
                        {
                            Log.Warning("SpritePackingTag Empty: {0}", path);
                            return null;
                        }
                    }

                    return obj;
                }
            }

            return null;
        }

        static private void EditorInitResMgr()
        {
            mEditorAssetTypeExt.Clear();
            mEditorAssetTypeExt.Add(typeof(GameObject), new string[] { ".prefab" });
            mEditorAssetTypeExt.Add(typeof(SpriteAtlas), new string[] { ".spriteatlas" });
            mEditorAssetTypeExt.Add(typeof(Texture2D), new string[] { ".png", ".jpg" });
            mEditorAssetTypeExt.Add(typeof(Sprite), new string[] { ".png", ".jpg" });
            mEditorAssetTypeExt.Add(typeof(Material), new string[] { ".mat" });
            mEditorAssetTypeExt.Add(typeof(AudioClip), new string[] { ".wav", ".mp3", ".ogg" });
            mEditorAssetTypeExt.Add(typeof(Texture), new string[] { ".png", ".jpg", ".rendertexture" });
            mEditorAssetTypeExt.Add(typeof(Font), new string[] { ".ttf", ".otf" });
            mEditorAssetTypeExt.Add(typeof(Animator), new string[] { ".controller" });
            mEditorAssetTypeExt.Add(typeof(TextAsset), new string[] { ".bytes", ".txt" });
        }

        static ResourceMgr()
        {
            EditorInitResMgr();
        }
    }

#endif


    // assetbundle资源patch包管理器
    public partial class ResourceMgr : ModuleManager<ResourceMgr>
    {
        // 清理所有的资源引用
        private void Clear()
        {
            //if (BuildSetting.ReleaseMode)
            //{
            //    AssetBundleManager.Ins.Clear();
            //}
        }

        public void UnloadAllPackage()
        {
            Clear();
        }

        public override void Initialize()
        {

        }

        public override void Dispose()
        {

        }

        public override void OnGameUpdate(float deltaTime)
        {

        }

        public override void OnLateUpdate(float deltaTime)
        {

        }

        public Object Load(string url, string reason)
        {
            return LoadRes<UnityEngine.Object>(url, true, reason);
        }

        public T Load<T>(string url, string reason) where T : UnityEngine.Object
        {
            return LoadRes<T>(url, true, reason);
        }

        public T InstantiateResource<T>(string url, string reason) where T : UnityEngine.Object
        {
            var res = LoadRes<T>(url, true, reason);

            if (res == null) return null;

            return UnityEngine.Object.Instantiate<T>(res) as T;
        }

        public T ResourcesLoad<T>(string url) where T : UnityEngine.Object
        {
            return Resources.Load<T>(url);
        }

        public Sprite LoadSprite(string url, string reason)
        {
#if UNITY_EDITOR
            if (mResLoadHandler != null)
            {
                mResLoadHandler.OnResLoad(url, reason);
            }
#endif
            if (string.IsNullOrEmpty(url))
            {
                Log.Error(LogLevel.Critical, "Load Empty sprite! reason:{0}", reason);
                return null;
            }

            Sprite sprite = LoadRes<Sprite>(url, false, reason);
            if (sprite != null)
            {
                return sprite;
            }

            Log.Error(LogLevel.Critical, "Load Sprite Fail:{0},load reason:{1}", url, reason);
            return null;
        }

        /// <summary>
        /// 如果Sprite加载后为null，则不报错。
        /// 用于读取不到Sprite就读取相同url的其他资源的方法。
        /// </summary>
        /// <param name="url"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public Sprite LoadSpriteWithoutNullSpriteError(string url, string reason)
        {
            if (string.IsNullOrEmpty(url))
            {
                Log.Error(LogLevel.Critical, "Load Empty sprite! reason:{0}", reason);
                return null;
            }
            Sprite sprite = LoadRes<Sprite>(url, false, reason);
            if (sprite != null)
            {
                return sprite;
            }
            return null;
        }

        public Texture LoadTexture(string url, string reason)
        {
#if UNITY_EDITOR
            if (mResLoadHandler != null)
            {
                mResLoadHandler.OnResLoad(url, reason);
            }
#endif
            if (string.IsNullOrEmpty(url))
            {
                Log.Error(LogLevel.Critical, "Load Empty Texture! reason:{0}", reason);
                return null;
            }

#if UNITY_EDITOR
            Sprite sprite = LoadRes<Sprite>(url, false, reason);
            if (sprite != null)
            {
                Log.Error(LogLevel.Critical, "Load Texture fail, Type is Sprite: {0}, reason:{1}", url, reason);
                return null;
            }
#endif

            Texture tex = LoadRes<Texture>(url, false, reason);
            if (tex != null)
            {
                return tex;
            }

            Log.Error(LogLevel.Critical, "Load Texture Fail:{0},load reason:{1}", url, reason);
            return null;
        }

        public T LoadRes<T>(string url, bool logError, string reason) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            if (mResLoadHandler != null)
            {
                mResLoadHandler.OnResLoad(url, reason);
            }
#endif

            if (string.IsNullOrEmpty(url))
            {
                if (logError)
                {
                    Log.Warning("Load Empty Res!,reason:{0}", reason);
                }
                return null;
            }

#if UNITY_EDITOR
            if (AssetBundleManager.Ins == null) // 在自动构建模式下，会触发ReleaseMode为True，而AssetBundleManager为空的情况
            {
                Log.Warning("Skip load in [BuildPipeline], url={0}", url);
                return null;
            }   

            var obj = EditorLoad<T>(url, reason);
            if (null != obj)
            {
                return obj;
            }

            if (logError)
            {
                Log.Error(LogLevel.Critical, string.Format("[Editor]Load Fail[{0}],load reason:{1}", url, reason));
            }
#else
            T obj;
            var ret = AssetBundleManager.Ins.LoadAsset<T>(url, out obj);
            if (ret == 0)
            {
                return obj;
            }

            if (logError)
            {
                Log.Error(LogLevel.Critical,string.Format("[AB]LoadFail[{0}], reason={1}, ret={2}", url, reason, ret));
            }
#endif
            return null;
        }

        public T LoadPreloadRes<T>(string url, bool logError, string reason) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            var obj = EditorLoad<T>(url, reason);
            if (null != obj)
            {
                return obj;
            }

            if (logError)
            {
                Log.Error(LogLevel.Critical, string.Format("[EditPAB]LoadFail[{0}],load reason:{1}", url, reason));
            }
#else
            var (ret, obj) = AssetBundleManager.Ins.LoadPreloadAsset<T>(url);
            if (ret == 0)
            {
                return obj;
            }

            if (logError)
            {
                Log.Error(LogLevel.Critical, string.Format("[PAB]LoadFail[{0}], reason={1}, ret={2}", url, reason, ret));
            }
#endif
            return null;
        }

        public bool AsyncLoadRes<T>(string url, string reason, FuncAsyncLoadTask callback) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            if (mResLoadHandler != null)
            {
                mResLoadHandler.OnResLoad(url, reason);
            }
#endif
            if (string.IsNullOrEmpty(url))
            {
                Log.Error(LogLevel.Critical, "Load Empty Res!,reason:{0}", reason);
                return false;
            }
#if UNITY_EDITOR
            var obj = EditorLoad<T>(url, reason);
            if (null != obj)
            {
                callback.Invoke(obj);
                return true;
            }
            else
            {
                Log.Error(LogLevel.Critical, string.Format("Load Fail[{0}],load reason:{1}", url, reason));
            }
#else
        return AssetBundleManager.Ins.AsyncLoadAsset(url, callback);
#endif
            return false;
        }

#if UNITY_EDITOR
        private IResLoadHandler mResLoadHandler;

        public void InstallResLoadHandler(IResLoadHandler handler)
        {
            mResLoadHandler = handler;
        }
#endif
    }


    public interface IResLoadHandler
    {
        void OnResLoad(string url, string reason);
    }
}