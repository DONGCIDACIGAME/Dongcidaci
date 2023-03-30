using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace GameEngine
{
    public abstract partial class UIEntity
    {
        protected T BindControl<T>(string uiPath, Dictionary<string, object> openArgs = null)
            where T : UIControl, new()
        {
            if (mUIRoot == null) // 没有根节点
            {
                Log.Error(LogLevel.Critical, "BindControl failed,ui root node is null!");
                return null;
            }

            Transform rootNode = CommanHelper.FindChildNode(mUIRoot.transform, uiPath);
            if (rootNode == null)// 找不到要绑定的节点
            {
                Log.Error(LogLevel.Critical, "BindControl failed,does not has target node,node={0}", rootNode);
                return null;
            }
            T ctl = UIManager.Ins.BindControl<T>(this, rootNode.gameObject, openArgs);
            return ctl;
        }

        protected GameObject BindNode(string node)
        {
            if (mUIRoot == null) // 没有根节点
            {
                Log.Error(LogLevel.Critical, "BindNode failed,ui root node is null!");
                return null;
            }

            Transform targetNode = CommanHelper.FindChildNode(mUIRoot.transform, node);
            if (targetNode == null)// 找不到要绑定的节点
            {
                Log.Error(LogLevel.Critical, "BindNode failed,does not has target node,node={0},UIEntity is:{1}", node, this.GetType());
                return null;
            }
            GameObject go = targetNode.gameObject; // 绑定成功
            return go;
        }

        protected Image BindImageNode(string node,Sprite defaultSprite = null)
        {
            GameObject go = BindNode(node);
            if (go== null) // 节点绑定失败
            {
                Log.Error(LogLevel.Critical, "BindImageNode failed,fail to find node:{0}", node);
                return null;
            }

            Image img = go.GetComponent<Image>();
            if (img == null) //没有Image组件
            {
                Log.Error(LogLevel.Critical, "BindImageNode failed,Image component is Required! node={0}", node);
                return null;
            }

            if (defaultSprite != null)
                img.sprite = defaultSprite;

            return img;
        }


        protected Button BindButtonNode(string node, UnityAction call)
        {
            GameObject go = BindNode(node);
            if (go == null) // 节点绑定失败
            {
                Log.Error(LogLevel.Critical, "BindButtonNode failed,fail to find node:{0}", node);
                return null;
            }

            Button btn = go.GetComponent<Button>();
            if (btn == null) //没有Button组件
            {
                Log.Error(LogLevel.Critical, "BindButtonNode failed,Button component is Required! node={0}", node);
                return null;
            }

            if (call != null) //没有绑定回调（可以不绑）
            {
                btn.onClick.AddListener(call);

                // 关闭前要移除按钮的点击事件监听
                RegisterBeforeCloseAction(() =>
                {
                    btn.onClick.RemoveListener(call);
                });
            }

            return btn;
        }

        protected TMP_Text BindTextNode(string node, string defaultText = null)
        {
            GameObject go = BindNode(node);
            if (go == null) // 节点绑定失败
            {
                Log.Error(LogLevel.Critical, "BindTextNode failed,fail to find node:{0}", node);
                return null;
            }

            TMP_Text tmp_text = go.GetComponent<TMP_Text>();
            if (tmp_text == null) //没有TMP_Text组件
            {
                Log.Error(LogLevel.Critical, "BindTextNode failed,TMP_Text component is Required! node={0}", node);
                return null;
            }

            if (defaultText != null)
                tmp_text.text = defaultText;

            return tmp_text;
        }

        protected TMP_InputField BindInputFieldNode(string node,  UnityAction<string> endEditAction, string defaultText = null)
        {
            GameObject go = BindNode(node);
            if (go == null) // 节点绑定失败
            {
                Log.Error(LogLevel.Critical, "BindInputFieldNode failed,fail to find node:{0}", node);
                return null;
            }

            TMP_InputField tmp_field = go.GetComponent<TMP_InputField>();
            if (tmp_field == null) //没有TMP_InputField组件
            {
                Log.Error(LogLevel.Critical, "BindInputFieldNode failed,TMP_InputField component is Required! node={0}", node);
                return null;
            }

            if (defaultText != null)
            {
                TMP_Text tt = tmp_field.placeholder as TMP_Text;
                if(tt != null)
                {
                    tt.text = defaultText;
                }
            }

            if(endEditAction != null)
            {
                tmp_field.onEndEdit.AddListener(endEditAction);
                // 关闭前要移除输入框的输入完成事件监听
                RegisterBeforeCloseAction(() =>
                {
                    tmp_field.onEndEdit.RemoveListener(endEditAction);
                });
            }

            return tmp_field;
        }

        protected TMP_Dropdown BindDropDownNode(string node, List<TMP_Dropdown.OptionData> options, UnityAction<int> call)
        {
            GameObject go = BindNode(node);
            if (go == null) // 节点绑定失败
            {
                Log.Error(LogLevel.Critical, "BindDropDownNode failed,fail to find node:{0}", node);
                return null;
            }

            TMP_Dropdown comp = go.GetComponent<TMP_Dropdown>();
            if (comp == null) //没有Dropdown组件
            {
                Log.Error(LogLevel.Critical, "BindDropDownNode failed,Dropdown component is Required! node={0}", node);
                return null;
            }

            comp.AddOptions(options);
            comp.onValueChanged.AddListener(call);
            return comp;
        }
    }
}