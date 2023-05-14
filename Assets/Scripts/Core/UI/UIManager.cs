using System.Collections.Generic;
using UnityEngine;

namespace GameEngine
{
    public class UIManager : ModuleManager<UIManager>
    {
        /// <summary>
        /// 所有要打开的面板，都先加到队列中
        /// </summary>
        private SimpleQueue<UILoadingPanel> mLoadingPanels;
        
        /// <summary>
        /// 所有UI挂载的layer root
        /// </summary>
        private  Dictionary<string, GameObject> mLayers;

        /// <summary>
        /// 所有已经打开的面板 
        /// 理论上一个面板同一时间应该只有一份
        /// 多个的可以考虑设计为UIControl
        /// </summary>
        private Dictionary<System.Type, UIPanel> mAllOpenPanels;

        /// <summary>
        /// UI实体缓存池
        /// </summary>
        private Dictionary<System.Type, GamePool<UIEntity>> mUIEntityPools;

        /// <summary>
        /// UI GameObject 缓存池
        /// </summary>
        private Dictionary<string, GameObjectPool> mUIObjectPool;

        /// <summary>
        ///  UI GameObject 缓存池在场景中的挂点
        /// </summary>
        private GameObject mUIPoolNode;

        private int entityIndexer;
        public int PopUIEntityId()
        {
            return entityIndexer++;
        }

        /// <summary>
        /// 注册UI面板的挂载层
        /// </summary>
        /// <param name="layerPath"></param>
        private void RegisterLayer(GameObject uiRoot, string layerPath)
        {
            if (string.IsNullOrEmpty(layerPath))
                return;

            Transform layer = uiRoot.transform.Find(layerPath);
            if (layer != null)
            {
                mLayers.Add(layerPath, layer.gameObject);
            }
        }

        /// <summary>
        ///  获取UI面板的挂在层
        /// </summary>
        /// <param name="layerPath"></param>
        /// <returns></returns>
        private GameObject GetLayer(string layerPath)
        {
            if (mLayers.TryGetValue(layerPath, out GameObject layer))
            {
                return layer;
            }

            return null;
        }

        public override void Initialize()
        {
            mLoadingPanels = new SimpleQueue<UILoadingPanel>();
            mLayers = new Dictionary<string, GameObject>();
            mAllOpenPanels = new Dictionary<System.Type, UIPanel>();
            mUIEntityPools = new Dictionary<System.Type, GamePool<UIEntity>>();
            mUIObjectPool = new Dictionary<string, GameObjectPool>();

            GameObject UIRoot = GameObject.Find(UIPathDef.UI_ROOT);
            if (UIRoot == null)
            {
                Log.Error(LogLevel.Fatal, "UI Root Not Find");
                return;
            }

            mUIPoolNode = UIRoot.transform.Find(UIPathDef.UI_POOL_NODE).gameObject;

            // 绑定所有UI层级
            foreach (string layer in UIPathDef.ALL_UI_LAYER)
            {
                RegisterLayer(UIRoot, layer);
            }
        }

        #region common funcs

        /// <summary>
        /// 获取指定类型的entity实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T GetUIEntity<T>() where T : UIEntity, new()
        {
            T entity = null;

            GamePool<UIEntity> pool;
            if (mUIEntityPools.TryGetValue(typeof(T), out pool))
            {
                if (pool != null)
                {
                    entity = pool.PopObj() as T;
                }
            }

            if (entity == null)
            {
                entity = new T();
            }

            return entity;
        }

        /// <summary>
        /// 回收entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        private void PushUIEntity<T>(T entity) where T : UIEntity
        {
            GamePool<UIEntity> pool;

            System.Type type = entity.GetType();
            if (mUIEntityPools.TryGetValue(type, out pool))
            {
                if (pool == null)
                {
                    pool = new GamePool<UIEntity>();
                }
            }
            else
            {
                pool = new GamePool<UIEntity>();
                mUIEntityPools.Add(type, pool);
            }

            pool.PushObj(entity);
        }

        /// <summary>
        /// go 放入缓存池
        /// </summary>
        /// <param name="resPath">资源路径</param>
        /// <param name="uiObj">go</param>
        private void PushUIObjectToPool(string resPath, GameObject uiObj)
        {
            // 如果资源路径为空，则不能复用
            // 例如某个直接bind的control，文件夹中并没有control的资源路径，这种的就不能复用
            if (string.IsNullOrEmpty(resPath))
            {
                return;
            }

            if (uiObj == null)
            {
                Log.Error(LogLevel.Normal, "PushUIObject Failed,push null game object");
                return;
            }

            GameObjectPool pool;

            if (!mUIObjectPool.TryGetValue(resPath, out pool))
            {
                pool = new GameObjectPool(mUIPoolNode);
                mUIObjectPool.Add(resPath, pool);
            }

            pool.PushObj(uiObj);
            uiObj.transform.SetParent(mUIPoolNode.transform, false);
        }

        /// <summary>
        /// 从缓存池中去除对应资源路径的go
        /// </summary>
        /// <param name="resPath"></param>
        /// <returns></returns>
        private GameObject GetUIObjectFromPool(string resPath)
        {
            GameObjectPool pool;
            if (!mUIObjectPool.TryGetValue(resPath, out pool))
            {
                return null;
            }

            return pool.PopObj();
        }

        private static GameObject InstantiateAndSetName(GameObject template)
        {
            GameObject uiObj = GameObject.Instantiate(template);
            uiObj.name = template.name;
            return uiObj;
        }

        #endregion

        #region 打开panel
        /// <summary>
        /// 检查面板是否打开
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool CheckPanelOpen<T>() 
            where T : UIPanel
        {
            // 在待打开面板的里
            foreach (UILoadingPanel loadingPanel in mLoadingPanels)
            {
                if (loadingPanel.panel.GetType().Equals(typeof(T)))
                {
                    return true;
                }
            }

            // 在已打开面板里
            if (mAllOpenPanels.ContainsKey(typeof(T)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 打开面板
        /// 面板的打开策略：全异步
        /// </summary>
        /// <typeparam name="T">面板类型</typeparam>
        /// <param name="panelPath">面板prefab的资源路径</param>
        /// <param name="openArgs">面板的打开参数</param>
        public void OpenPanel<T>(string panelPath, Dictionary<string, object> openArgs = null) where T : UIPanel, new()
        {
            //1. 检查面板是否被打开
            if (CheckPanelOpen<T>())
            {
                Log.Error(LogLevel.Info, "Reopen panel {0}", typeof(T));
                return;
            }

            // 2. 检查面板路径是否为空
            if (string.IsNullOrEmpty(panelPath))
            {
                Log.Error(LogLevel.Critical, "OpenPanel {0} Failed, empty panel path!", typeof(T));
                return;
            }

            // 3. 获取面板entity
            T panel = GetUIEntity<T>();
            if (!panel.CustomCheckBeforeOpen(openArgs))
            {
                Log.Error(LogLevel.Critical, "OpenPanel {0} Failed, Check Can Open Failed!", typeof(T));
                return;
            }

            // 3. 添加到待加载面板队列中，在update中，每帧根据加载最大数量加载面板
            UILoadingPanel loadingPanel = new UILoadingPanel(panel, panelPath, openArgs);
            mLoadingPanels.Enqueue(loadingPanel);
        }


        /// <summary>
        /// 目前仅做每帧加载上限限制，后面根据需求来处理（如还有面板在关闭动画过程中，需要等待等情况）
        /// </summary>
        private void _OpenPanels()
        {
            // 限制每帧加载的面板数量上限
            int cnt = Mathf.Min(UIDefine.Panel_Load_Per_Frame, mLoadingPanels.Count);
            for (int i = 0; i < cnt; i++)
            {
                UILoadingPanel loadingPanel = mLoadingPanels.Dequeue();
                LoadPanel(loadingPanel);
            }
        }

        /// <summary>
        /// 加载面板
        /// </summary>
        /// <param name="loadingPanel"></param>
        private void LoadPanel(UILoadingPanel loadingPanel)
        {
            if (loadingPanel.panel == null)
            {
                Log.Error(LogLevel.Normal, "ExcutePanelLoadAction Error,Load null UIPanel!");
            }

            // 获取面板放置的Layer
            string UILayerPath = loadingPanel.panel.GetPanelLayerPath();
            GameObject layer = GetLayer(UILayerPath);
            if (layer == null)
            {
                Log.Error(LogLevel.Critical, "OpenPanel Failed, panel layer not find,UILayerPath:{0}", UILayerPath);
                return;
            }

            // 从缓存池中取obj
            GameObject uiObj = GetUIObjectFromPool(loadingPanel.resPath);


            if (uiObj == null) // 没有的话就异步加载panel
            {
                ResourceMgr.Ins.AsyncLoadRes<GameObject>(loadingPanel.resPath, "Load UI Panel", (Object obj) =>
                {
                    GameObject template = obj as GameObject;
                    if (template == null)
                        return;

                    uiObj = InstantiateAndSetName(template);
                    OnUIPanelLoadFinish(loadingPanel.panel, loadingPanel.openArgs, uiObj, loadingPanel.resPath, layer);
                });
            }
            else
            {
                uiObj.transform.parent = layer.transform;
                OnUIPanelLoadFinish(loadingPanel.panel, loadingPanel.openArgs, uiObj, loadingPanel.resPath, layer);
            }
        }

        /// <summary>
        /// panel加载完成
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="openArgs">panel打开参数</param>
        /// <param name="uiObj">panel绑定的go</param>
        /// <param name="resPath">panel的资源路径</param>
        /// <param name="layerGo">panel所在的layer</param>
        private void OnUIPanelLoadFinish(UIPanel panel, Dictionary<string, object> openArgs, GameObject uiObj, string resPath, GameObject layerGo)
        {
            // 1. 初始化
            panel.Initialize(resPath, uiObj, layerGo, null);

            // 2.Open
            panel.Open(openArgs);

            // 3.添加已打开面板字典
            System.Type type = panel.GetType();
            if (!mAllOpenPanels.ContainsKey(type))
            {
                mAllOpenPanels.Add(type, panel);
            }
        }

        #endregion

        #region 关闭panel

        /// <summary>
        /// 面板是否需要关闭
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        private bool CheckPanelCanClosed<T>()
        {
            // 不在已打开面板里，不需要关闭
            if (!mAllOpenPanels.ContainsKey(typeof(T)))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 关闭面板
        /// 面板关闭策略：接受到关闭指令就直接关闭
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="panel"></param>
        public void ClosePanel<T>()
            where T:UIPanel
        {
            if(!CheckPanelCanClosed<T>())
            {
                return;
            }

            // 待关闭的面板加入队列，让在本帧结束后关闭
            if (mAllOpenPanels.TryGetValue(typeof(T), out UIPanel panel))
            {
                //mToClosePanels.Enqueue(panel);
                if (panel != null)
                {
                    mAllOpenPanels.Remove(typeof(T));
                    panel.UIEntityOnClose();
                }
            }
        }
        #endregion

        #region 添加control

        /// <summary>
        /// 添加control
        /// 策略：同步加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="holder">父UIEntity</param>
        /// <param name="resPath">资源路径</param>
        /// <param name="parent">加载完成后放置在哪个节点下</param>
        /// <param name="openArgs">打开参数</param>
        /// <returns></returns>
        public T AddControl<T>(UIEntity holder, string resPath, GameObject parent, Dictionary<string, object> openArgs = null)
                where T : UIControl, new()
        {
            // 1.Control不能单独存在，必须有上级Entity，上级entity可以是panel，也可以是control
            if (holder == null)
            {
                Log.Error(LogLevel.Critical, "AddControl Failed,UI Control must has a holder!");
                return null;
            }

            // 2.资源路径检查，不能为空
            if (string.IsNullOrEmpty(resPath))
            {
                Log.Error(LogLevel.Critical, "AddControl Failed, load {0} with empty path! ", typeof(T));
                return null;
            }

            // 3.必须有一个父物体
            if (parent == null)
            {
                Log.Error(LogLevel.Critical, "AddControl Failed, load {0} with null parent", typeof(T));
                return null;
            }

            // 4. 获取entity
            T control = GetUIEntity<T>();

            // 5. 自定义检查
            if (!control.CustomCheckBeforeOpen(openArgs))
            {
                Log.Error(LogLevel.Critical, "AddControl {0} Failed, Check Can Open Failed!", typeof(T));
                return null;
            }

            // 尝试从缓存池中获取obj
            GameObject uiObj = GetUIObjectFromPool(resPath);
            if(uiObj == null)
            {
                // 没有的话直接同步加载资源
                GameObject template = ResourceMgr.Ins.Load<GameObject>(resPath, "Load UI Control");
                if (template == null)
                    return null;
                // 再实例化
                uiObj = InstantiateAndSetName(template);
            }

            OnUIControlLoadFinish(holder, control, openArgs, uiObj, resPath, parent);
            return control;
        }

        /// <summary>
        /// control加载完成后
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="entity"></param>
        /// <param name="openArgs"></param>
        /// <param name="uiObj"></param>
        /// <param name="resPath"></param>
        /// <param name="parent"></param>
        private void OnUIControlLoadFinish(UIEntity holder, UIEntity entity, Dictionary<string, object> openArgs, GameObject uiObj, string resPath, GameObject parent)
        {
            // 1.initialize
            entity.Initialize(resPath, uiObj, parent, holder);

            // 2.Open
            entity.Open(openArgs);
        }

        #endregion

        #region 绑定control
        /// <summary>
        /// 直接绑定control
        /// 非加载式，直接绑定
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="holder">持有者</param>
        /// <param name="ctlRootNode">control的根节点</param>
        /// <returns></returns>
        public T BindControl<T>(UIEntity holder, GameObject ctlRootNode, Dictionary<string, object> openArgs = null) 
            where T : UIControl, new()
        {
            if (holder == null)
            {
                Log.Error(LogLevel.Critical, "BindControl Failed,holder is null!");
                return null;
            }

            if (ctlRootNode == null)
            {
                Log.Error(LogLevel.Critical, "BindControl Failed,rootNode is null!");
                return null;
            }

            var ctl = GetUIEntity<T>();

            OnUIControlLoadFinish(holder, ctl, openArgs, ctlRootNode, null, null);
            return ctl;
        }
        #endregion

        #region 移除control
        /// <summary>
        /// 移除Control
        /// </summary>
        /// <param name="holder">control的持有者</param>
        /// <param name="toRemove">被移除的control</param>
        public void RemoveControl(UIControl toRemove)
        {
            if (toRemove == null)
            {
                Log.Error(LogLevel.Critical, "RemoveControl Failed,ctl is null!");
                return;
            }

            UIEntity holder = toRemove.GetHolder();

            if (holder == null)
            {
                Log.Error(LogLevel.Critical, "RemoveControl Failed,holder is null!");
                return;
            }


            if (holder.GetHashCode() == toRemove.GetHashCode())
            {
                Log.Error(LogLevel.Fatal, "RemoveControl Fatal Error, {0} holder is self! ", toRemove.GetType());
                return;
            }
            
            if(holder.HasChildEntity(toRemove))
            {
                toRemove.UIEntityOnClose();
                holder.RemoveChildUIEntity(toRemove);
            }
        }
        #endregion

        /// <summary>
        /// 所有打开的面板Update
        /// </summary>
        /// <param name="deltaTime"></param>
        private void UpdateAllOpenPanels(float deltaTime)
        {
            foreach (var kv in mAllOpenPanels)
            {
                UIPanel panel = kv.Value;
                if (panel != null)
                {
                    panel.Update(deltaTime);
                }
            }
        }

        /// <summary>
        ///  UI Root Update...
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void OnUpdate(float deltaTime)
        {
            _OpenPanels();       // open panels

            UpdateAllOpenPanels(deltaTime);  // common panel update
        }

        public override void OnLateUpdate(float deltaTime)
        {

        }

        /// <summary>
        /// 删除UIEntity 和 UI GameObject
        /// 移除Entity    （根据复用策略，可以进缓存池）
        /// 移除UI GameObject （根据复用策略，可以选择进缓存池）
        /// </summary>
        /// <param name="entity"></param>
        public void DestroyUIEntity(UIEntity entity)
        {
            if (entity == null)
                return;

            if (entity.CheckRecycleUIEntity()) // recycle ui entity
            {
                PushUIEntity(entity);
            }

            if (entity.CheckRecycleUIGameObject()) // recycle ui gameobject
            {
                PushUIObjectToPool(entity.GetUIResPath(), entity.GetRootObj());
            }
            else if (entity.GetRootObj() != null) // destroy ui gameobject,
            {
                GameObject.Destroy(entity.GetRootObj());
            }
        }

        public override void Dispose()
        {
            mLoadingPanels = null;
            mLayers = null;
            mAllOpenPanels = null;
            mUIEntityPools = null;
            mUIObjectPool = null;
            mUIPoolNode = null;
        }
    }
}