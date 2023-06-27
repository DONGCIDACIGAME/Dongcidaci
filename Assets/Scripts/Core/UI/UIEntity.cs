using System.Collections.Generic;
using UnityEngine;

namespace GameEngine
{
    public delegate void UIEntityAction();

    public abstract partial class UIEntity
    {
        /// <summary>
        /// 关闭前执行
        /// </summary>
        protected List<UIEntityAction> mBeforeCloseActions = new List<UIEntityAction>();

        /// <summary>
        /// 所有的子Entity
        /// </summary>
        private List<UIEntity> mChildUIEntitys = new List<UIEntity>();

        /// <summary>
        /// UI（panel、control）的根节点
        /// </summary>
        protected GameObject mUIRoot;

        /// <summary>
        /// control 持有者,可以是一个panel，也可以是另一个control，但不能是自己
        /// </summary>
        protected UIEntity mHolder;

        /// <summary>
        /// UIEntity的上下文容器
        /// </summary>
        protected Dictionary<string, object> Context = new Dictionary<string, object>();

        /// <summary>
        /// 资源路径
        /// </summary>
        private string mResPath;

        protected GameEventListener mEventListener;

        private int mUIEntityID;

        // 构造函数
        protected UIEntity() 
        {
            mUIEntityID = UIManager.Ins.PopUIEntityId();
        } 

        public void Initialize(string resPath, GameObject rootGo, GameObject parent, UIEntity holder) 
        {
            // 1. 资源路径
            // 如果路径为空，则不能复用资源
            this.mResPath = resPath;
            // 2.根节点
            this.mUIRoot = rootGo;
            // 3.父节点
            SetParent(parent);
            // 4.持有者
            SetHolder(holder);
            // 5.初始化事件监听器
            mEventListener = GamePoolCenter.Ins.GameEventLIstenerPool.Pop();
            // 6.绑定所有子节点
            BindAllUINodes();
        }

        /// <summary>
        /// 绑定UI
        /// </summary>
        protected abstract void BindUINodes();

        protected virtual void BindEvents() { }

        public void Open(Dictionary<string,object> openArgs)
        {
            OnOpen(openArgs);
            BindEvents();
        }

        /// <summary>
        /// panel 打开时调用
        /// </summary>
        protected abstract void OnOpen(Dictionary<string,object> openArgs);

        /// <summary>
        /// 检查传入的参数是否正确，如果传入参数有问题就不打开UI
        /// </summary>
        /// <param name="openArgs"></param>
        /// <returns></returns>
        public virtual bool CustomCheckBeforeOpen(Dictionary<string, object> openArgs)
        {
            return true;
        }

        private void RegisterBeforeCloseAction(UIEntityAction action)
        {
            if (action != null && mBeforeCloseActions != null)
                mBeforeCloseActions.Add(action);
        }

        /// <summary>
        /// panel关闭前调用
        /// </summary>
        protected abstract void OnClose();

        public string GetUIResPath()
        {
            return mResPath;
        }

        /// <summary>
        /// 绑定持有者
        /// </summary>
        /// <param name="holder"></param>
        public void SetHolder(UIEntity holder)
        {
            if (holder == null)

                return;

            UIEntity temp = holder;
            // holder不能是自己且自己不能是holder的上级-防止递归
            while(temp != null)
            {
                if (temp.GetUIEntityID() == this.GetUIEntityID())
                    return;
                temp = temp.GetHolder();
            }

            holder.AddChildUIEntity(this);

            mHolder = holder;
        }

        private void AddChildUIEntity(UIEntity uiEntity)
        {
            if (!mChildUIEntitys.Contains(uiEntity))
            {
                mChildUIEntitys.Add(uiEntity);
            }
        }

        public object GetContextData(string key)
        {
            if(Context.TryGetValue(key,out object data))
            {
                return data;
            }

            return null;
        }

        public void AddContextData(string key, object value)
        {
            if(!Context.ContainsKey(key))
            {
                Context.Add(key, value);
            }
            else
            {
                Context[key] = value;
            }
        }

        public UIEntity GetHolder()
        {
            return mHolder;
        }

        public GameObject GetRootObj()
        {
            return mUIRoot;
        }

        public void UIEntityOnClose()
        {
            DestroyAllChildUIEntity();
            OnClose();
            ClearAll();
            DestroyUIEntity();
        }

        public void SetParent(GameObject parent)
        {
            if (mUIRoot == null)
                return;

            if (parent == null)
                return;

            mUIRoot.transform.SetParent(parent.transform, false);
        }

        public void DestroyAllChildUIEntity()
        {
            foreach (UIEntity uiEntity in mChildUIEntitys)
            {
                uiEntity.UIEntityOnClose();
            }
        }

        public bool HasChildEntity(UIEntity entity)
        {
            return mChildUIEntitys.Contains(entity);
        }

        public void RemoveChildUIEntity(UIEntity uiEntity)
        {
            if (mChildUIEntitys.Contains(uiEntity))
            {
                mChildUIEntitys.Remove(uiEntity);
            }
        }

        public void ClearAll()
        {
            if (mBeforeCloseActions != null)
            {
                foreach (UIEntityAction action in mBeforeCloseActions)
                {
                    action();
                }

                mBeforeCloseActions.Clear();
            }

            if (mEventListener != null)
            {
                mEventListener.ClearAllEventListen();
            }

            if(mChildUIEntitys != null)
            {
                mChildUIEntitys.Clear();
            }
            Context.Clear();
            mHolder = null;
        }

        /// <summary>
        /// 是否复用Entity
        /// </summary>
        /// <returns></returns>
        public virtual  bool CheckRecycleUIEntity()
        {
            return false;
        }

        /// <summary>
        /// 是否复用GameObject
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckRecycleUIGameObject()
        {
            return false;
        }

        public void DestroyUIEntity()
        {
            UIManager.Ins.DestroyUIEntity(this);
        }

        private void BindAllUINodes()
        {
            if (mUIRoot != null)
            {
                BindUINodes();
            }
            else
            {
                Log.Error(LogLevel.Critical, "BindUIObjectNodes Error, root is null !!!");
            }
        }

        public void SetVisible(bool visible)
        {
            if(mUIRoot != null)
                mUIRoot.SetActive(visible);
        }

        protected virtual void OnUpdate(float deltaTime) { }

        public void Update(float deltaTime)
        {
            OnUpdate(deltaTime);

            // 子Entity的update由父Entity驱动
            foreach (UIEntity childEntity in mChildUIEntitys)
            {
                if (childEntity != null)
                {
                    childEntity.Update(deltaTime);
                }
            }
        }

        public void Dispose()
        {
            ClearAll();
            mBeforeCloseActions = null;
            mEventListener = null;
            Context = null;
            mChildUIEntitys = null;
            mHolder = null;
        }

        public int GetUIEntityID()
        {
            return mUIEntityID;
        }
    }
}