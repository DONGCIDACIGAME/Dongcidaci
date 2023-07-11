using GameEngine;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// 行为树管理器
/// </summary>
public class BehaviourTreeCenter
{
    /// <summary>
    /// 已经加载过的行为树数据都缓存在这里面
    /// 下次创建相同行为树时直接从缓存数据创建
    /// </summary>
    private Dictionary<string, BTNodeData> mLoadedBTNodeDatas;

    public void Initialize()
    {
        mLoadedBTNodeDatas = new Dictionary<string, BTNodeData>();
        string dirPath = null;
#if UNITY_EDITOR
        dirPath = PathDefine.EDITOR_DATA_DIR_PATH + "/BehaviourTree";
#else
        dirPath = PathDefine.RELEASE_DATA_DIR_PATH + "/BehaviourTree";
#endif
        LoadAllBehaviourTree(dirPath);
    }

    private void LoadAllBehaviourTree(string dirPath)
    {
        if (string.IsNullOrEmpty(dirPath))
            return;

        DirectoryInfo di = new DirectoryInfo(dirPath);
        foreach (DirectoryInfo subDi in di.EnumerateDirectories())
        {
            LoadAllBehaviourTree(subDi.FullName);
        }

        foreach (FileInfo fi in di.EnumerateFiles("*.tree"))
        {
            LoadTreeWithFileFullPath(fi.FullName, false);
        }
    }

    /// <summary>
    /// 加载行为树
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public BTTreeEntry LoadTreeWithFileFullPath(string filePath, bool forceReload = false)
    {
        BTNodeData data = LoadTreeData(filePath, forceReload);
        if (data == null)
            return null;
        BTTreeEntry tree = CreateBTNode(data) as BTTreeEntry;
        tree.LoadFromBTNodeData(data);
        return tree;
    }

    /// <summary>
    /// 加载行为树
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public BTTreeEntry LoadTreeWithTreeName(string treeName, bool forceReload = false)
    {
        string fileFullPath = BehaviourTreeHelper.TreeNameToFileFullPath(treeName);
        return LoadTreeWithFileFullPath(fileFullPath, forceReload);
    }


    /// <summary>
    /// 加载行为树数据
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public BTNodeData LoadTreeData(string filePath, bool forceReload = false)
    {
        // 查询缓存数据
        BTNodeData data = null;
        if (forceReload || !mLoadedBTNodeDatas.TryGetValue(filePath, out data))
        {
            data = BehaviourTreeHelper.LoadBTNodeData(filePath);
        }

        if (data == null)
            return null;
        if (!mLoadedBTNodeDatas.ContainsKey(filePath))
        {
            mLoadedBTNodeDatas.Add(filePath, data);
        }

        return data;
    }

    /// <summary>
    /// 保存行为树
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="tree"></param>
    public void SaveTree(string filePath, BTTree tree)
    {
        if(tree == null)
        {
            Log.Error(LogLevel.Normal, "SaveTree Failed, tree is null!");
            return;
        }

        BTNodeData data = tree.ToBTNodeData();
        BehaviourTreeHelper.SaveBTNodeData(filePath, data);
    }



    public void Dispose()
    {
        mLoadedBTNodeDatas = null;
    }
}
