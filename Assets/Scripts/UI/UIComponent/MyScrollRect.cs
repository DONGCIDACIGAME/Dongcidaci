using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectWithHandle : ScrollRect
{
    private UnityAction<PointerEventData> mScrollHandle;
    private UnityAction<PointerEventData> mOnDragHandle;
    private UnityAction<PointerEventData> mBeginDragHandle;
    private UnityAction<PointerEventData> mEndDragHandle;

    public void SetScrollHandle(UnityAction<PointerEventData> handle)
    {
        mScrollHandle = handle;
    }

    public void SetOnDragHandle(UnityAction<PointerEventData> handle)
    {
        mOnDragHandle = handle;
    }

    public void SetBeginDragHandle(UnityAction<PointerEventData> handle)
    {
        mBeginDragHandle = handle;
    }

    public void SetEndDragHandle(UnityAction<PointerEventData> handle)
    {
        mEndDragHandle = handle;
    }

    public override void OnScroll(PointerEventData data)
    {
        base.OnScroll(data);
        if(mScrollHandle != null)
        {
            mScrollHandle(data);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        if (mOnDragHandle != null)
        {
            mOnDragHandle(eventData);
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if (mBeginDragHandle != null)
        {
            mBeginDragHandle(eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (mEndDragHandle != null)
        {
            mEndDragHandle(eventData);
        }
    }
}
