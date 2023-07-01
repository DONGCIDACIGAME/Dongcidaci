using GameEngine;


public enum AIEditorCacheType
{
    Copy = 1,
    Cut
}

public class AIEditorClipboard : IGameDisposable
{
    private BTNode mClidpboard;
    private AIEditorCacheType mCacheType;

    public void Dispose()
    {
        mClidpboard = null;
    }

    public void Copy(BTNode node)
    {
        mClidpboard = node;
        mCacheType = AIEditorCacheType.Copy;
    }

    public void Cut(BTNode node)
    {
        mClidpboard = node;
        mCacheType = AIEditorCacheType.Cut;
    }

    public BTNode GetNode()
    {
        return mClidpboard;
    }

    public AIEditorCacheType GetCacheType()
    {
        return mCacheType;
    }
}
