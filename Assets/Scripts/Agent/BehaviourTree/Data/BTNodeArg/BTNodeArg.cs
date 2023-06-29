[System.Serializable]
public class BTNodeArg 
{
    /// <summary>
    /// 参数名
    /// </summary>
    public string ArgName;

    /// <summary>
    /// 参数类型
    /// </summary>
    public string ArgType;

    /// <summary>
    /// 参数数据内容
    /// 以字符串形式存储，加载时需要根据类型进行解析
    /// </summary>
    public string ArgContent;
}
