using LitJson;

public static class BTTreeHelper
{
    public static BTNodeData LoadBTTreeData(string filePath)
    {
        if(string.IsNullOrEmpty(filePath))
        {
            Log.Error(LogLevel.Normal, "LoadTree Failed, filePath is null or empty!");
            return null;
        }

        string jsonStr = FileHelper.ReadText(filePath, System.Text.Encoding.UTF8);
        BTNodeData treeData = JsonMapper.ToObject<BTNodeData>(jsonStr);
        return treeData;
    }

    public static void SaveBTTreeData(string filePath, BTNodeData data)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            Log.Error(LogLevel.Normal, "SaveBTTreeData Failed, filePath is null or empty!");
            return;
        }

        string jsonStr= JsonMapper.ToJson(data);

        FileHelper.WriteStr(filePath, jsonStr, System.Text.Encoding.UTF8);
    }
}
