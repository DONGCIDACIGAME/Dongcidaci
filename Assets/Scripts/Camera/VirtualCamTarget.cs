using UnityEngine;

public class VirtualCamTarget
{
    private Vector3 mVirtualPos;
    public void BindPosition(Vector3 pos)
    {
        mVirtualPos = pos;
    }

    public void Move(Vector3 offset)
    {
        mVirtualPos += offset;
    }

    public Vector3 GetPosition()
    {
        return mVirtualPos;
    }
}
