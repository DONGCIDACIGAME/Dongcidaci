using System;
using UnityEngine;
using System.Collections.Generic;

public delegate void FuncAsyncLoadTask(UnityEngine.Object obj);

public class UwAsyncLoadTask
{
    public UwAsyncLoadTask(string assetPath, AssetBundleRequest request, FuncAsyncLoadTask completeCallback)
    {
        mAssetPath = assetPath;
        mRequest = request;
        //mCache = iCache;
        AppendCompleteCallback(completeCallback);
    }

    private AssetBundleRequest mRequest;
    private FuncAsyncLoadTask mCompleteCallback;
    //private IResourceCache mCache;
    private string mAssetPath;
    public string AssetPath
    {
        get
        {
            return mAssetPath;
        }
    }

    public void AppendCompleteCallback(FuncAsyncLoadTask completeCallback)
    {
        if (completeCallback != null)
        {
            mCompleteCallback += completeCallback;
        }
    }

    public bool UpdateTask()
    {
        if (mRequest.isDone)
        {
            try
            {
                //mCache.Push(mAssetPath, mRequest.asset);
                mCompleteCallback.Invoke(mRequest.asset);
            }
            catch(System.Exception e)
            {
                Log.Error(LogLevel.Fatal, e.ToString());
            }
            return true;
        }
        return false;
    }
}

public class UwAsyncLoadTaskMgr
{
    private List<UwAsyncLoadTask> mTasks = new List<UwAsyncLoadTask>();
    private HashSet<string> mNames = new HashSet<string>();

    public bool IsAsyncLoading()
    {
        return mTasks.Count > 0;
    }

    public void Clear()
    {
        mTasks.Clear();
        mNames.Clear();
    }

    private UwAsyncLoadTask FindTask(string assetPath)
    {
        for(int i=0; i<mTasks.Count; ++i)
        {
            if (mTasks[i].AssetPath == assetPath)
            {
                return mTasks[i];
            }
        }
        return null;
    }

    public void UpdateMgr()
    {
        int len = mTasks.Count;
        for(int index=len-1; index>=0; --index)
        {
            if (mTasks[index].UpdateTask())
            {
                var currName = mTasks[index].AssetPath;
                mTasks.RemoveAt(index);
                mNames.Remove(currName);
            }
        }
    }

    public void Load(string assetPath, AssetBundle assetBundle, FuncAsyncLoadTask completeCallback)
    {
        if (mNames.Contains(assetPath))
        {
            var task = FindTask(assetPath);
            if (task != null)
            {
                task.AppendCompleteCallback(completeCallback);
                return;
            }
            else
            {
                Log.Error(LogLevel.Fatal,"Alt-Internal");
            }
        }

        var request = assetBundle.LoadAssetAsync(assetPath);
        if (request != null)
        {
            mTasks.Add(new UwAsyncLoadTask(assetPath, request, completeCallback));
        }
        else
        {
            Log.Error(LogLevel.Fatal, "LoadFail:{0}", assetPath);
        }
    }
}