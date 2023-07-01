using GameEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AI节点
/// </summary>
public class ControlAINode : UIControl
{
    private Button Btn_AINode;
    private Image Image_SelectBackground;
    private Button Btn_AddChildNode;
    private TMP_Text Text_NodeName;
    private GameObject Node_SubNodesContain;
    private GameObject Node_ConnectLineContain;
    private GameObject Node_ConnectLineHorizontal;

    private float singleNodeWidth;
    private float singleNodeHeight;

    private BTNode mNode;
    private static Quaternion UpLineRot = Quaternion.Euler(0, 0, 90);
    private static Quaternion DownLineRot = Quaternion.Euler(0, 0, -90);

    protected override void BindUINodes()
    {
        Btn_AINode = BindButtonNode("Btn_AINode", OnBtnAINodeClick);
        Image_SelectBackground = Btn_AINode.GetComponent<Image>();
        Btn_AddChildNode = BindButtonNode("Button_AddChildNode", OnBtnAddChildeNodeClick);
        Text_NodeName = BindTextNode("Text_NodeName", "AINode");
        Node_SubNodesContain = BindNode("Node_SubNodesContain");
        Node_ConnectLineContain = BindNode("Node_ConnectLineContain");
        Node_ConnectLineHorizontal = BindNode("Node_ConnectLineHorizontal");
    }

    public BTNode GetBTNode()
    {
        return mNode;
    }

    protected override void BindEvents()
    {
        base.BindEvents();

        mEventListener.Listen<BTNode>("SelectNode", OnOperationSelectNode);
        mEventListener.Listen<BTNode>("UpdateNodeDisplayName", OnUpdateNodeDisplayName);
        mEventListener.Listen<BTNode, BTNode>("UpdateNode", OnUpdateNode);
    }

    private void OnUpdateNode(BTNode oldNode, BTNode newNode)
    {
        if(mNode.Equals(oldNode))
        {
            mNode = newNode;
        }
    }

    private void OnUpdateNodeDisplayName(BTNode node)
    {
        if (!node.Equals(mNode))
            return;

        Text_NodeName.text = node.NodeName;
    }


    private void OnOperationSelectNode(BTNode node)
    {
        if(mNode.Equals(node))
        {
            OnBtnAINodeClick();
        }
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        mNode = openArgs["BTNode"] as BTNode;

        singleNodeWidth = (GetRootObj().transform as RectTransform).sizeDelta.x;
        singleNodeHeight = (GetRootObj().transform as RectTransform).sizeDelta.y;
        Draw();
        Deselect();
    }

    public override bool CheckRecycleUIEntity()
    {
        return true;
    }

    public override bool CheckRecycleUIGameObject()
    {
        return true;
    }

    public void Draw()
    {
        Text_NodeName.text = mNode.NodeName;
        UpdateAddChildBtnVisible();
        AddAllChildNodes();
    }

    private void UpdateAddChildBtnVisible()
    {
        if (mNode is BTLeafNode)
        {
            Btn_AddChildNode.gameObject.SetActive(false);
        }
        else if (mNode is BTDecorNode)
        {
            BTDecorNode decor = mNode as BTDecorNode;
            Btn_AddChildNode.gameObject.SetActive(decor.GetChildNode() == null);
        }
        else if (mNode is BTTree)
        {
            if(mNode is BTTreeEntry)
            {
                BTTreeEntry tree = mNode as BTTreeEntry;
                Btn_AddChildNode.gameObject.SetActive(tree.GetChildNode() == null);
            }
            else if(mNode is BTChildTree)
            {
                Btn_AddChildNode.gameObject.SetActive(false);
            }
        }
        else if(mNode is BTCompositeNode)
        {
            Btn_AddChildNode.gameObject.SetActive(true);
        }
    }

    private void AddChildNode(BTNode childNode, Vector2 pos)
    {
        ControlAINode node = UIManager.Ins.AddControl<ControlAINode>(
            this,
            "Prefabs/UI/AgentAIEditor/Ctl_AINode",
            Node_SubNodesContain,
            new Dictionary<string, object>()
            {
                { "BTNode", childNode }
            });

        node.GetRootObj().transform.localPosition = pos;

        AddLine(pos, DownLineRot);
    }

    private void AddLine(Vector2 pos, Quaternion rotation)
    {
        ControlAINodeLine line_up = UIManager.Ins.AddControl<ControlAINodeLine>(
            this,
            "Prefabs/UI/AgentAIEditor/Ctl_AINodeLine",
            Node_ConnectLineContain);

        line_up.GetRootObj().transform.rotation = rotation;
        line_up.GetRootObj().transform.localPosition = pos;
        line_up.GetRootObj().transform.localScale = Vector3.one;
    }

    public  float GetWidth()
    {
        return AIEditorHelper.GetWidth(mNode, singleNodeWidth);
    }

    public float GetHeight()
    {
        return AIEditorHelper.GetHeight(mNode, singleNodeHeight);
    }

    private void AddAllChildNodes()
    {
        Node_ConnectLineHorizontal.SetActive(false);

        if (mNode == null)
            return;

        if (mNode is BTTree)
        {
            BTTree tree = mNode as BTTree;
            BTNode childNode = tree.GetChildNode();
            if(childNode != null)
            {
                AddChildNode(childNode, Vector2.zero);
                AddLine(Vector2.zero, UpLineRot);
            }
        }
        else if(mNode is BTDecorNode)
        {
            BTDecorNode decor = mNode as BTDecorNode;
            BTNode childNode = decor.GetChildNode();
            if (childNode != null)
            {
                AddChildNode(childNode, Vector2.zero);
                AddLine(Vector2.zero, UpLineRot);
            }
        }
        else if(mNode is BTLeafNode)
        {
            
        }
        else if(mNode is BTCompositeNode)
        {
            BTCompositeNode composite = mNode as BTCompositeNode;
            List<BTNode> childNodes = composite.GetChildNodes();
            float totalWidth = AIEditorHelper.GetWidth(composite,singleNodeWidth);
            float start = -totalWidth/2;
            float totalHorizontalLineWidth = 0;
            float startNodeX = 0;
            float endNodeX = 0;

            if(childNodes.Count > 0)
            {
                AddLine(Vector2.zero, UpLineRot);
            }
            

            for (int i = 0; i < childNodes.Count; i++)
            {
                BTNode childNode = childNodes[i];
                float nodeWidth = AIEditorHelper.GetWidth(childNode, singleNodeWidth);
                Vector2 pos = new Vector2(start + nodeWidth / 2, 0);
                AddChildNode(childNode, pos);
                start += nodeWidth;

                if (i == 0)
                {
                    totalHorizontalLineWidth += nodeWidth / 2;
                    startNodeX = pos.x;
                }
                else if(i == childNodes.Count - 1)
                {
                    totalHorizontalLineWidth += nodeWidth / 2;
                    endNodeX = pos.x;
                }
                else
                {
                    totalHorizontalLineWidth += nodeWidth;
                }
            }

            Node_ConnectLineHorizontal.SetActive(childNodes.Count > 1);
            (Node_ConnectLineHorizontal.transform as RectTransform).sizeDelta = new Vector2(totalHorizontalLineWidth, 2);
            Node_ConnectLineHorizontal.transform.localPosition = new Vector2((startNodeX + endNodeX) / 2, 0);
        }
    }

    private void OnBtnAINodeClick()
    {
        GameEventSystem.Ins.Fire("ClickAINodeCtl", this);
    }

    public void Select()
    {
        Image_SelectBackground.color = Color.green;
    }

    public void Deselect()
    {
        Image_SelectBackground.color = Color.white;
    }

    private void OnBtnAddChildeNodeClick()
    {
        GameEventSystem.Ins.Fire("OnClickAddChildNode", this);
    }

    

    protected override void OnClose()
    {
        mNode = null;
    }
}
