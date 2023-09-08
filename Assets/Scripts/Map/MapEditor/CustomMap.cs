using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using LitJson;
using System.Text;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class CustomMap : MonoBehaviour
{
    public static CustomMap Ins;

    [Header("地图基础网格信息")]
    public int gridColCount = 10;
    public int gridRowCount = 10;
    public float gridCellWidth = 2;
    public float gridCellHeight = 2;
    public bool drawGridGizmos = false;
    private List<GridCell> gridCells = new List<GridCell>();

    public class GridCell
    {
        public Vector3 ldV3;
        public Vector3 rdV3;
        public Vector3 ruV3;
        public Vector3 luV3;
        public int cellMapIndex = -1;
        public Vector3 anchorPos;

        public GridCell(Vector3 ldV3, Vector3 rdV3, Vector3 ruV3, Vector3 luV3, int index)
        {
            this.ldV3 = ldV3;
            this.rdV3 = rdV3;
            this.ruV3 = ruV3;
            this.luV3 = luV3;
            this.cellMapIndex = index;
            this.anchorPos = new Vector3(ldV3.x + (rdV3.x - ldV3.x) / 2, 0, ldV3.z + (luV3.z - ldV3.z) / 2);
        }

    }

    public void CaculateGridCells()
    {
        gridCells.Clear();
        if (gridColCount <= 0 || gridRowCount <= 0 || gridCellWidth <= 0 || gridCellHeight <= 0) return;

        int mapIndex = 0;
        for (int i = 0; i < gridColCount; i++)
        {
            for (int j = 0; j < gridRowCount; j++)
            {
                var ldV3 = new Vector3(i * gridCellWidth, 0, j * gridCellHeight);
                var rdV3 = new Vector3((i + 1) * gridCellWidth, 0, j * gridCellHeight);
                var ruV3 = new Vector3((i + 1) * gridCellWidth, 0, (j + 1) * gridCellHeight);
                var luV3 = new Vector3(i * gridCellWidth, 0, (j + 1) * gridCellHeight);

                var cell = new GridCell(ldV3, rdV3, ruV3, luV3, mapIndex);
                gridCells.Add(cell);
                mapIndex++;
            }
        }
    }


    [Header("导航信息")]
    [Range(2,5)]
    public int naviSubLevel = 4;
    public bool drawNaviGrids = false;
    private List<NaviGridCell> naviCells = new List<NaviGridCell>();

    public class NaviGridCell
    {
        public Vector3 ldV3;
        public Vector3 rdV3;
        public Vector3 ruV3;
        public Vector3 luV3;
        public int xIndex = -1;
        public int yIndex = -1;
        public Vector3 anchorPos;

        /// <summary>
        /// 该导航网格的最大通过半径
        /// </summary>
        public float maxPassRadius;

        /// <summary>
        /// 该导航网格的优先级
        /// 如果网格上存在负面事件，那么该优先级会更低
        /// </summary>
        public int priority;

        /// <summary>
        /// 是否被事件障碍物阻挡着
        /// </summary>
        public bool isEventBlocked;
    }

    public void CaculateNaviGridCells()
    {
        naviCells.Clear();
        if (gridColCount <= 0 || gridRowCount <= 0 || gridCellWidth <= 0 || gridCellHeight <= 0) return;

        int mapIndex = 0;
        for (int i = 0; i < gridColCount* naviSubLevel; i++)
        {
            for (int j = 0; j < gridRowCount*naviSubLevel; j++)
            {
                var ldV3 = new Vector3(i * gridCellWidth, 0, j * gridCellHeight);
                var rdV3 = new Vector3((i + 1) * gridCellWidth, 0, j * gridCellHeight);
                var ruV3 = new Vector3((i + 1) * gridCellWidth, 0, (j + 1) * gridCellHeight);
                var luV3 = new Vector3(i * gridCellWidth, 0, (j + 1) * gridCellHeight);

                var cell = new GridCell(ldV3, rdV3, ruV3, luV3, mapIndex);
                gridCells.Add(cell);
                mapIndex++;
            }
        }
    }



    //private static string MapJsonSavePath = Application.dataPath;
    public GameMapData mapData;


    

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

    [Space]
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


    public void SaveGridIndexToGround()
    {
        var groundLayerT = GameObject.Find("_GROUND_LAYER").transform;
        if (groundLayerT == null)
        {
            Debug.Log("未能找到地板层");
            return;
        }
        else
        {
            Debug.Log("找到地板层");
        }

        if (groundLayerT.childCount == 0) return;
        for (int i=0;i<groundLayerT.childCount;i++)
        {
            groundLayerT.GetChild(i).TryGetComponent<MapGroundView>(out MapGroundView groundView);
            if (groundView == null)
            {
                continue;
            }

            groundView.GenerateMapIndexs();
        }

    }

    public void SaveMapDataToDisk()
    {
        if (mapData.mapName == string.Empty) return;

        SaveMapEventData();

        string filePath = Path.Combine(PathDefine.MAP_DATA_DIR_PATH,mapData.mapName + ".json");
        Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        FileInfo file = new FileInfo(filePath);
        StreamWriter sw = file.CreateText();

        //string saveJsonStr = JsonUtility.ToJson(this.mapData, true);
        StringBuilder sb = new StringBuilder();
        JsonWriter jr = new JsonWriter(sb);
        jr.PrettyPrint = true;
        jr.IndentValue = 4;//缩进空格个数
        JsonMapper.ToJson(this.mapData, jr);
        string saveJsonStr = sb.ToString();
        Debug.Log("开始写入地图数据 ----");
        sw.Write(saveJsonStr);
        sw.Close();
        sw.Dispose();
        Debug.Log("写入地图数据完成 ----");

    }

    public void SaveMapEventData()
    {
        if(this.mapData.mapEventDatas == null)
        {
            this.mapData.mapEventDatas = new List<MapEventData>();
        }
        else
        {
            this.mapData.mapEventDatas.Clear();
        }
        

        var mapEventLayerT = GameObject.Find("_EVENT_LAYER").transform;
        if (mapEventLayerT == null)
        {
            Debug.Log("未能找到事件层");
            return;
        }
        else
        {
            Debug.Log("找到事件层");
        }

        if (mapEventLayerT.childCount == 0) return;
        for (int i = 0; i < mapEventLayerT.childCount; i++)
        {
            mapEventLayerT.GetChild(i).TryGetComponent<MapEventDataConfig>(out MapEventDataConfig eventConfig);
            if (eventConfig == null)
            {
                continue;
            }

            eventConfig.SyncPositionInfo();
            eventConfig.SyncCustomDictData();
            this.mapData.mapEventDatas.Add(eventConfig.configData);
        }
    }





#if UNITY_EDITOR

    
    public float lineThickness = 2f;

    private void OnDrawGizmos()
    {
        if (drawGridGizmos == false) return;

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
