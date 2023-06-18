using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[DisallowMultipleComponent]
public class CustomMap : MonoBehaviour
{
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
            this.anchorPos = new Vector3(ldV3.x + (rdV3.x-ldV3.x)/2, 0, ldV3.y + (luV3.z - ldV3.z)/2);
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


#if UNITY_EDITOR

    public bool drawGrid = false;
    public float lineThickness = 2f;

    private void OnDrawGizmos()
    {
        if (drawGrid == false) return;

        if (gridColCount <= 0 || gridRowCount <= 0 || gridCellWidth <= 0 || gridCellHeight <= 0) return;

        for (int i = 0; i <= gridColCount; i++)
        {
            var startV3 = new Vector3(i*gridCellWidth,0,0);
            var endV3 = new Vector3(i * gridCellWidth, 0, gridRowCount*gridCellHeight);
            Handles.DrawLine(startV3, endV3, lineThickness);
        }

        for (int i = 0; i <= gridRowCount; i++)
        {
            var startV3 = new Vector3(0, 0, i*gridCellHeight);
            var endV3 = new Vector3(gridColCount * gridCellWidth, 0, i * gridCellHeight);
            Handles.DrawLine(startV3, endV3, lineThickness);
        }


    }


#endif


}
