using GameEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AI行为
/// </summary>
public class ControlAIAction : UIControl
{
    private Button Btn_BehaviourNode;
    private Button Btn_AddCondition;
    private GameObject Node_SubNodesContain;

    private static int singleNodeWidth = 150;
    private static int singleNodeHeight = 100; 
    private static int nodeOffsetX = 20; 
    private static int nodeOffsetY = 20;

    private BTNode mNode;
    //private List<ControlAIAction> mChildCtlAIActions;

    protected override void BindUINodes()
    {
        Btn_BehaviourNode = BindButtonNode("Button_BehaviourNode", OnBtnBehaviourNodeClick);
        Btn_AddCondition = BindButtonNode("Button_AddCondition", OnBtnAddConditionClick);
        Node_SubNodesContain = BindNode("Node_SubNodesContain");
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        //mChildCtlAIActions = new List<ControlAIAction>();
        mNode = openArgs["BTNode"] as BTNode;
    }

    private static int GetWidth(BTNode node)
    {
        if (node == null)
            return singleNodeWidth;

        if(node is BTLeafNode)
        {
            return singleNodeWidth;
        }
        else if(node is BTTree)
        {
            BTTree tree = node as BTTree;
            return GetWidth(tree.GetChildNode());
        }
        else if(node is BTDecorNode)
        {
            BTDecorNode decor = node as BTDecorNode;
            return GetWidth(decor.GetChildNode());
        }
        else if (node is BTCompositeNode)
        {
            BTCompositeNode composite = node as BTCompositeNode;
            List<BTNode> nodes = composite.GetChildNodes();
            int width = 0;
            foreach(BTNode _childNode in nodes)
            {
                width += GetWidth(_childNode) + nodeOffsetX;
            }

            if(nodes.Count > 0)
            {
                width -= nodeOffsetX;
            }

            return width;
        }

        return singleNodeWidth;
    }

    private static int GetHeight(BTNode node)
    {
        if (node == null)
            return 0;

        int height = 0;
        while(node.GetChildeNodeNum() > 0)
        {
            if (node is BTLeafNode)
            {
                height += singleNodeHeight;
            }
            else if (node is BTTree)
            {
                BTTree tree = node as BTTree;
                BTNode childNode = tree.GetChildNode();
                if(childNode != null)
                {
                    height += GetHeight(childNode);
                }
            }
            else if (node is BTDecorNode)
            {
                BTTree tree = node as BTTree;
                BTNode childNode = tree.GetChildNode();
                if (childNode != null)
                {
                    height += GetHeight(childNode);
                }
            }
            else if (node is BTCompositeNode)
            {
                BTCompositeNode composite = node as BTCompositeNode;
                List<BTNode> nodes = composite.GetChildNodes();
                int maxHeight = 0;
                foreach (BTNode _childNode in nodes)
                {
                    int _height = GetHeight(_childNode);
                    if(_height > maxHeight)
                    {
                        maxHeight = _height;
                    }
                }
                height += maxHeight;
            }

            height += nodeOffsetY;
        }


        return height;
    }

    private void OnBtnBehaviourNodeClick()
    {

    }

    private void OnBtnAddConditionClick()
    {

    }


    protected override void OnClose()
    {
        
    }


}
