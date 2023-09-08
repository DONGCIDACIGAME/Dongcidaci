using UnityEngine;
using UnityEditor;

public class MapGroundView : MapEntityView
{
    /// <summary>
    /// 这个地板占据的地图块索引信息
    /// </summary>
    [SerializeField] private int[] _occupyMapIndexs;
    public int[] OccupyMapIndexs => _occupyMapIndexs;

    /// <summary>
    /// 检查某个点是否在这个地板面上
    /// </summary>
    /// <param name="checkPos"></param>
    /// <returns></returns>
    public bool IsPosOnGround(Vector3 checkPos)
    {
        if (transform.childCount == 0) return false;

        for (int i=0;i<transform.childCount;i++)
        {
            var ret  = transform.GetChild(i).TryGetComponent<MeshRenderer>(out MeshRenderer groundMesh);
            if (ret == false) continue;
            if (groundMesh.bounds.Contains(checkPos)) return true;
        }

        return false;
    }


#if UNITY_EDITOR

    public void GenerateMapIndexs()
    {
        _occupyMapIndexs = new int[] { };
        if (CustomMap.Ins == null) return;
        if (CustomMap.Ins.gridCellWidth <= 0 || CustomMap.Ins.gridCellHeight <= 0 || CustomMap.Ins.gridColCount <= 0 || CustomMap.Ins.gridRowCount <= 0) return;
        if (transform.childCount == 0) return;
        _occupyMapIndexs = new int[transform.childCount];
        for (int i=0;i<transform.childCount;i++)
        {
            var cellTPos = transform.GetChild(i).position;
            int tgtColIndex = Mathf.FloorToInt(cellTPos.x / CustomMap.Ins.gridCellWidth);
            int tgtRowIndex = Mathf.FloorToInt(cellTPos.z / CustomMap.Ins.gridCellHeight);
            var realIndex = tgtColIndex * CustomMap.Ins.gridRowCount + tgtRowIndex;
            transform.GetChild(i).name = realIndex.ToString();
            _occupyMapIndexs[i] = realIndex;
            Debug.Log(realIndex);
        }
    }

    private void OnDrawGizmos()
    {
        
        return;
        if (_occupyMapIndexs == null || _occupyMapIndexs.Length == 0) return;
        if (transform.childCount == 0) return;
        if (EditorApplication.isPlaying) return;


        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 9;
        myStyle.alignment = TextAnchor.MiddleCenter;
        myStyle.fontStyle = FontStyle.Italic;
        myStyle.normal.textColor = Color.green;

        for (int i = 0; i < transform.childCount; i++)
        {
            var cellTPos = transform.GetChild(i).position;
            Handles.Label(cellTPos,transform.GetChild(i).name,myStyle);
        }


    }

#endif




}
