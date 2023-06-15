using GameEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AI行为
/// </summary>
public class ControlAIActionNode : UIControl
{
    private Button Btn_BehaviourNode;
    private Button Btn_AddChildNode;
    private TMP_Text Text_BehaviourNode;
    private GameObject Node_SubNodesContain;
    private GameObject Node_ConnectLineContain;
    private GameObject Node_ConnectLineHorizontal;
    private GameObject Template_Line;

    private float singleNodeWidth;
    private float singleNodeHeight;

    private BTNode mNode;
    private static Quaternion UpLineRot = Quaternion.Euler(0, 0, 90);
    private static Quaternion DownLineRot = Quaternion.Euler(0, 0, -90);

    protected override void BindUINodes()
    {
        Btn_BehaviourNode = BindButtonNode("Button_BehaviourNode", OnBtnBehaviourNodeClick);
        Btn_AddChildNode = BindButtonNode("Button_AddChildNode", OnBtnAddChildeNodeClick);
        Text_BehaviourNode = BindTextNode("Text_BehaviourNode","AINode");
        Node_SubNodesContain = BindNode("Node_SubNodesContain");
        Node_ConnectLineContain = BindNode("Node_ConnectLineContain");
        Node_ConnectLineHorizontal = BindNode("Node_ConnectLineHorizontal");
        Template_Line = BindNode("Template_Line");
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        //mChildCtlAIActions = new List<ControlAIAction>();
        mNode = openArgs["BTNode"] as BTNode;

        singleNodeWidth = (GetRootObj().transform as RectTransform).sizeDelta.x;
        singleNodeHeight = (GetRootObj().transform as RectTransform).sizeDelta.y;

        Text_BehaviourNode.text = mNode.NodeName;
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
            BTTree tree = mNode as BTTree;
            Btn_AddChildNode.gameObject.SetActive(tree.GetChildNode() == null);
        }
        else if(mNode is BTCompositeNode)
        {
            Btn_AddChildNode.gameObject.SetActive(true);
        }
    }

    private void AddChildNode(BTNode childNode, Vector2 pos)
    {
        ControlAIActionNode node = UIManager.Ins.AddControl<ControlAIActionNode>(
            this,
            "Prefabs/UI/AgentAIEditor/Ctl_AIAction",
            Node_SubNodesContain,
            new Dictionary<string, object>()
            {
                { "BTNode", childNode }
            });

        node.GetRootObj().transform.localPosition = pos;

        GameObject line_up = GameObject.Instantiate(Template_Line);
        line_up.transform.SetParent(Node_ConnectLineContain.transform);
        line_up.transform.rotation = UpLineRot;
        line_up.transform.localPosition = Vector2.zero;
        line_up.transform.localScale = Vector3.one;

        GameObject line_down = GameObject.Instantiate(Template_Line);
        line_down.transform.SetParent(Node_ConnectLineContain.transform);
        line_down.transform.rotation = DownLineRot;
        line_down.transform.localPosition = pos;
        line_down.transform.localScale = Vector3.one;
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
            }
        }
        else if(mNode is BTDecorNode)
        {
            BTDecorNode decor = mNode as BTDecorNode;
            BTNode childNode = decor.GetChildNode();
            if (childNode != null)
            {
                AddChildNode(childNode, Vector2.zero);
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

    private void OnBtnBehaviourNodeClick()
    {

    }

    private void OnBtnAddChildeNodeClick()
    {
        GameEventSystem.Ins.Fire("OnClickAddChildNode", mNode);
    }

    protected override void OnClose()
    {
        
    }
}
