using System.Collections.Generic;

public static class AIEditorHelper
{
    public static float GetWidth(BTNode node, float singleNodeWidth)
    {
        if (node == null)
            return singleNodeWidth;

        float width = 0;
        if (node is BTLeafNode)
        {
            width = singleNodeWidth;
        }
        else if (node is BTTree)
        {
            BTTree tree = node as BTTree;
            width = GetWidth(tree.GetChildNode(), singleNodeWidth);
        }
        else if (node is BTDecorNode)
        {
            BTDecorNode decor = node as BTDecorNode;
            width = GetWidth(decor.GetChildNode(), singleNodeWidth);
        }
        else if (node is BTCompositeNode)
        {
            BTCompositeNode composite = node as BTCompositeNode;
            List<BTNode> nodes = composite.GetChildNodes();
            if(nodes.Count == 0)
            {
                width = singleNodeWidth;
            }
            else
            {
                foreach (BTNode _childNode in nodes)
                {
                    width += GetWidth(_childNode, singleNodeWidth);
                }
            }
        }

        return width;
    }

    public static float GetHeight(BTNode node, float singleNodeHeight)
    {
        if (node == null)
            return 0;

        float height = singleNodeHeight;
         if (node is BTTree)
        {
            BTTree tree = node as BTTree;
            height += GetHeight(tree.GetChildNode(), singleNodeHeight);
        }
        else if (node is BTDecorNode)
        {
            BTDecorNode decor = node as BTDecorNode;
            height += GetHeight(decor.GetChildNode(), singleNodeHeight);
        }
        else if (node is BTCompositeNode)
        {
            BTCompositeNode composite = node as BTCompositeNode;
            List<BTNode> nodes = composite.GetChildNodes();
            float maxHeight = 0;
            foreach (BTNode _childNode in nodes)
            {
                float _height = GetHeight(_childNode, singleNodeHeight);
                if (_height > maxHeight)
                {
                    maxHeight = _height;
                }
            }

            height += maxHeight;
        }
        return height;
    }
}
