using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class CustomMap : MonoBehaviour
{
    public static CustomMap Ins;

    public int gridColCount = 10;
    public int gridRowCount = 10;

    public float gridCellWidth = 2;
    public float gridCellHeight = 2;


    public class GridCell
    {
        public Vector3 ldV3;
        public Vector3 rdV3;
        public Vector3 ruV3;
        public Vector3 luV3;
        public int cellMapIndex = -1;
        public Vector3 anchorPos;

        public GridCell(Vector3 ldV3,Vector3 rdV3,Vector3 ruV3,Vector3 luV3,int index)
        {
            this.ldV3 = ldV3;
            this.rdV3 = rdV3;
            this.ruV3 = ruV3;
            this.luV3 = luV3;
            this.cellMapIndex = index;
            this.anchorPos = new Vector3(ldV3.x + (rdV3.x-ldV3.x)/2, 0, ldV3.z + (luV3.z - ldV3.z)/2);
        }

    }


    private List<GridCell> gridCells = new List<GridCell>();

    public void CaculateGridCells()
    {
        gridCells.Clear();
        if (gridColCount <= 0 || gridRowCount <= 0 || gridCellWidth <= 0 || gridCellHeight <= 0) return;

        int mapIndex = 0;
        for (int i =0;i<gridColCount;i++)
        {
            for (int j= 0;j<gridRowCount;j++)
            {
                var ldV3 = new Vector3(i*gridCellWidth,0,j*gridCellHeight);
                var rdV3 = new Vector3((i+1) * gridCellWidth, 0, j * gridCellHeight);
                var ruV3 = new Vector3((i + 1) * gridCellWidth, 0, (j+1) * gridCellHeight);
                var luV3 = new Vector3(i * gridCellWidth, 0, (j+1) * gridCellHeight);

                var cell = new GridCell(ldV3,rdV3,ruV3,luV3,mapIndex);
                gridCells.Add(cell);
                mapIndex++;
            }
        }
    }

    private void OnEnable()
    {
        Ins = this;
        CaculateGridCells();
        EditorApplication.update += Update;
    }

    private void OnDisable()
    {
        EditorApplication.update -= Update;
    }


    private void OnDestroy()
    {
        EditorApplication.update -= Update;
    }

    public bool isAutoSnap = false;


    private void Update()
    {
        if (isAutoSnap == false || EditorApplication.isPlaying) return;

        //Debug.Log("check update");

        GameObject[] selectedGameObjects = Selection.gameObjects;

        if (selectedGameObjects != null && selectedGameObjects.Length > 0)
        {
            for (int i = 0; i < selectedGameObjects.Length; ++i)
            {
                SnapSelectedObjectToGrid(selectedGameObjects[i]);
            }
        }


    }

    private void SnapSelectedObjectToGrid(GameObject selectedGameObject)
    {
        if (selectedGameObject != null && PrefabUtility.IsPartOfAnyPrefab(selectedGameObject))
        {
            selectedGameObject = PrefabUtility.GetOutermostPrefabInstanceRoot(selectedGameObject);
        }

        if (selectedGameObject == null)
        {
            return;
        }

        string layerName = "SnapTileLayer";
        if (selectedGameObject.layer == LayerMask.NameToLayer(layerName) || string.IsNullOrEmpty(layerName))
        {
            var tgtCell = FindNearestGridCell(selectedGameObject);
            if (tgtCell == null) return;

            selectedGameObject.transform.position = new Vector3(tgtCell.anchorPos.x, selectedGameObject.transform.position.y, tgtCell.anchorPos.z);
        }

    }

    // bug
    private GridCell FindNearestGridCell(GameObject selectedGameObject)
    {
        //Debug.Log("x" + selectedGameObject.transform.position.x + "z"+selectedGameObject.transform.position.z);
        int tgtColIndex = Mathf.FloorToInt(selectedGameObject.transform.position.x / gridCellWidth);
        int tgtRowIndex = Mathf.FloorToInt(selectedGameObject.transform.position.z / gridCellHeight);
        //Debug.Log("col"+tgtColIndex + "row" + tgtRowIndex);
        if (tgtColIndex >= gridColCount || tgtColIndex < 0) return null;
        if (tgtRowIndex >= gridRowCount || tgtRowIndex < 0) return null;

        var realIndex = tgtColIndex * gridRowCount + tgtRowIndex;
        //Debug.Log(realIndex);
        if (realIndex >= gridCells.Count) return null;
        //Debug.Log(gridCells[realIndex].anchorPos);
        return gridCells[realIndex];
    }


    private void SaveGridIndexToGround()
    {

    }




#if UNITY_EDITOR

    public bool drawGrid = false;
    public float lineThickness = 2f;

    private void OnDrawGizmos()
    {
        if (drawGrid == false) return;

        if (gridColCount <= 0 || gridRowCount <= 0 || gridCellWidth <= 0 || gridCellHeight <= 0) return;

        //画竖线
        for (int i = 0; i <= gridColCount; i++)
        {
            var startV3 = new Vector3(i*gridCellWidth,0,0);
            var endV3 = new Vector3(i * gridCellWidth, 0, gridRowCount*gridCellHeight);
            Handles.DrawDottedLine(startV3, endV3, lineThickness);
        }

        //画横线
        for (int i = 0; i <= gridRowCount; i++)
        {
            var startV3 = new Vector3(0, 0, i*gridCellHeight);
            var endV3 = new Vector3(gridColCount * gridCellWidth, 0, i * gridCellHeight);
            Handles.DrawDottedLine(startV3, endV3, lineThickness);
        }


    }


#endif


}
