using UnityEngine;

public class GameCollider
{
    private Vector2 mPos;
    private Rect mArea;

    public GameCollider(Vector2 pos, Rect area)
    {
        this.mPos = pos;
        this.mArea = area;
    }

    public bool CheckPosInCollider(Vector2 pos)
    {
        return pos.x >= mPos.x + mArea.x - mArea.width / 2 && pos.x <= mPos.x + mArea.x + mArea.width / 2
            && pos.y >= mPos.y + mArea.y - mArea.height / 2 && pos.y <= mPos.y + mArea.y + mArea.height / 2;
    }

    public bool CheckCollapse(GameCollider other)
    {
        float offsetX = Mathf.Abs(mPos.x - other.mPos.x);
        float offsetY = Mathf.Abs(mPos.y - other.mPos.y);

        return offsetX <= mArea.width / 2 + other.mArea.width / 2
            && offsetY <= mArea.height / 2 + other.mArea.height / 2;
    }

    public bool CheckCollapse(Rect target)
    {
        float offsetX = Mathf.Abs(mPos.x - target.x);
        float offsetY = Mathf.Abs(mPos.y - target.y);

        return offsetX <= mArea.width / 2 + target.width / 2
            && offsetY <= mArea.height / 2 + target.height / 2;
    }
}
