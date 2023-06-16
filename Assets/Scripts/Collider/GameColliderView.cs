using GameEngine;
using UnityEditor;
using UnityEngine;

public class GameColliderView : MonoBehaviour
{
    // 暂时不考虑运行时通过该组件更新碰撞大小

    /// <summary>
    /// 碰撞盒大小
    /// </summary>
    public Vector2 size;

    /// <summary>
    /// 碰撞盒偏移
    /// </summary>
    public Vector2 offset;


    //Just test for collider
    /**
    public bool isUpdate = false;
    private GameCollider2D _collider2D;

    private void Start()
    {
        var colliderData = new GameColliderData2D(size,offset);
        _collider2D = new GameCollider2D(colliderData,this,new Vector2(transform.position.x,transform.position.z),30);
        GameColliderManager.Ins.RegisterGameCollider(_collider2D);

    }

    private void Update()
    {
        if (isUpdate)
        {
            GameColliderManager.Ins.UpdateGameCollidersInMap(_collider2D, new Vector2(transform.position.x, transform.position.z),transform.rotation.eulerAngles.y);
        }
    }


    public void HandleCollideTo(ICollideProcessor tgtColliderProcessor)
    {
        //throw new System.NotImplementedException();
        Debug.Log("发生了碰撞");

        TryGetComponent<HeroView>(out HeroView heroView);
        if (heroView != null)
        {
            //transform.position = new Vector3(-5,0,0);
        }
        else
        {
            gameObject.SetActive(false);
            GameColliderManager.Ins.UnRegisterGameCollider(_collider2D);
        }
        
    }

    public Entity GetProcessorEntity()
    {
        //throw new System.NotImplementedException();
        return null;
    }
    */





    /// <summary>
    /// 编辑器下的辅助功能
    /// </summary>
    #region Editor
#if UNITY_EDITOR
    public bool DrawDizmos;

    public Color color = Color.red;

    private void OnDrawGizmos()
    {
        if (!MapDef.GlobalDrawMapDizmos || !DrawDizmos)
            return;

        Gizmos.color = color;
        

        Vector3 groundPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 rotation = this.transform.rotation.eulerAngles;

        float halfWidth = size.x / 2f;
        float halfHeight = size.y / 2f;

        // 自身有旋转，计算四个顶点的本地坐标
        Vector3 lb_offset = MapHelper.RotateByYAxis(new Vector3((-halfWidth + offset.x) * this.transform.lossyScale.x, 0, (-halfHeight + offset.y) * this.transform.lossyScale.z), -rotation.y);
        Vector3 rb_offset = MapHelper.RotateByYAxis(new Vector3((halfWidth + offset.x) * this.transform.lossyScale.x, 0, (-halfHeight + offset.y) * this.transform.lossyScale.z), -rotation.y);
        Vector3 rt_offset = MapHelper.RotateByYAxis(new Vector3((halfWidth + offset.x) * this.transform.lossyScale.x, 0, (halfHeight + offset.y) * this.transform.lossyScale.z), -rotation.y);
        Vector3 lt_offset = MapHelper.RotateByYAxis(new Vector3((-halfWidth + offset.x) * this.transform.lossyScale.x, 0, (halfHeight + offset.y) * this.transform.lossyScale.z), -rotation.y);

        // 计算四个定点的世界坐标
        Vector3 lb = groundPos + lb_offset;
        Vector3 rb = groundPos + rb_offset;
        Vector3 rt = groundPos + rt_offset;
        Vector3 lt = groundPos + lt_offset;

        float lineThickness = 2f;
        //Gizmos.DrawLine(lb, rb);
        Handles.DrawLine(lb, rb, lineThickness);
        //Gizmos.DrawLine(rb, rt);
        Handles.DrawLine(rb, rt, lineThickness);
        //Gizmos.DrawLine(rt, lt);
        Handles.DrawLine(rt, lt, lineThickness);
        //Gizmos.DrawLine(lt, lb);
        Handles.DrawLine(lt, lb, lineThickness);
    }

    
#endif
    #endregion
}
