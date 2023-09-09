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

    //private static string MapJsonSavePath = Application.dataPath;
    public GameMapData mapData;



    #region Base Grid Info
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
    #endregion

    #region Navigation Info
    [Header("导航信息")]
    [Range(2,10)]
    public int naviSubLevel = 4;
    public bool drawNaviGrids = false;
    //private List<NaviGridCell> naviCells = new List<NaviGridCell>();

    public void CaculateNaviGridCells()
    {
        if (mapData == null) return;
        mapData.naviCells.Clear();
        if (gridColCount <= 0 || gridRowCount <= 0 || gridCellWidth <= 0 || gridCellHeight <= 0) return;

        // 获取所有的地板信息MapGroundView
        List<MapGroundView> grounds = new List<MapGroundView>();
        var groundLayerT = GameObject.Find("_GROUND_LAYER").transform;
        if (groundLayerT == null)
        {
            Debug.Log("未能找到地板层");
            return;
        }
        else
        {
            Debug.Log("找到地板层");
            if (groundLayerT.childCount == 0) return;
            
            for (int i = 0; i < groundLayerT.childCount; i++)
            {
                groundLayerT.GetChild(i).TryGetComponent<MapGroundView>(out MapGroundView groundView);
                if (groundView == null)
                {
                    continue;
                }
                grounds.Add(groundView);
            }
        }

        // 获取所有mapblock的碰撞信息
        //List<GameColliderView> mapBlockColliders = new List<GameColliderView>();
        List<IConvex2DShape> mapBlockShapes = new List<IConvex2DShape>();
        var mapBlockLayerT = GameObject.Find("_BLOCK_LAYER").transform;
        if (mapBlockLayerT == null)
        {
            Debug.Log("未能找到障碍层");
            return;
        }
        else
        {
            Debug.Log("找到障碍层");
            if (mapBlockLayerT.childCount > 0)
            {
                for (int i = 0; i < mapBlockLayerT.childCount; i++)
                {
                    mapBlockLayerT.GetChild(i).TryGetComponent<GameColliderView>(out GameColliderView blockCollider);
                    if (blockCollider == null)
                    {
                        continue;
                    }
                    var shape = GameColliderHelper.GetRegularShapeWith(blockCollider.shapeType,blockCollider.offset, blockCollider.size);
                    shape.AnchorPos = blockCollider.transform.position;
                    shape.AnchorAngle = blockCollider.transform.eulerAngles.y;
                    mapBlockShapes.Add(shape);
                }
            }

        }

        int naviColCount = gridColCount * naviSubLevel;
        int naviRowCount = gridRowCount * naviSubLevel;
        float naviCellWidth = gridCellWidth / (float)naviSubLevel;
        float naviCellHeight = gridCellHeight / (float)naviSubLevel;

        for (int i = 0; i < naviColCount; i++)
        {
            for (int j = 0; j < naviRowCount; j++)
            {
                var ldV3 = new Vector3(i * naviCellWidth, 0, j * naviCellHeight);
                var rdV3 = new Vector3((i + 1) * naviCellWidth, 0, j * naviCellHeight);
                var ruV3 = new Vector3((i + 1) * naviCellWidth, 0, (j + 1) * naviCellHeight);
                var luV3 = new Vector3(i * naviCellWidth, 0, (j + 1) * naviCellHeight);
                
                var naviCellPos = new Vector3(ldV3.x + (rdV3.x - ldV3.x) / 2, 0, ldV3.z + (luV3.z - ldV3.z) / 2);
                bool isNeeded = false;

                //检查是否在地板上
                foreach (var groundView in grounds)
                {
                    if (groundView.IsPosOnGround(naviCellPos))
                    {
                        Debug.Log("找到有效的导航网格");
                        isNeeded = true;
                        break;
                    }
                }
                if (isNeeded == false) continue;

                // 这个导航的格子在地面上
                // 判断这个格子是否和障碍物相交
                if (mapBlockShapes.Count > 0)
                {
                    foreach (var blockShape in mapBlockShapes)
                    {
                        var naviShape = GameColliderHelper.GetRegularShapeWith(Convex2DShapeType.Rect, Vector2.zero, new Vector2(naviCellWidth, naviCellHeight));
                        naviShape.AnchorPos = naviCellPos;
                        if (GameColliderHelper.CheckCollideSAT(naviShape, blockShape))
                        {
                            // 这个格子和障碍物产生了碰撞，不可用
                            // 剔除这个导航的格子
                            isNeeded = false;
                            break;
                        }
                    }
                }
                if (isNeeded == false) continue;

                // 检查事件层


                var newNaviCell = new NaviGridCell();
                newNaviCell.sizeX = naviCellWidth;
                newNaviCell.sizeY = naviCellHeight;
                newNaviCell.xIndex = i;
                newNaviCell.yIndex = j;
                newNaviCell.anchorPosX = naviCellPos.x;
                newNaviCell.anchorPosY = naviCellPos.y;
                newNaviCell.anchorPosZ = naviCellPos.z;

                // 计算这个点上的最大通过大小
                // 这里可能需要加上事件层的
                newNaviCell.maxPassRadius = 10000f;
                foreach (var colliderShape in mapBlockShapes)
                {
                    var maxDis = GameColliderHelper.GetMinBoundDisFromPointToShape(naviCellPos, colliderShape);
                    if (maxDis<newNaviCell.maxPassRadius)
                    {
                        newNaviCell.maxPassRadius = maxDis;
                    }
                }

                mapData.naviCells.Add(newNaviCell);

            }
        }
    }

    #endregion

    
    
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

    /**
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
    */

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
        if (gridColCount <= 0 || gridRowCount <= 0 || gridCellWidth <= 0 || gridCellHeight <= 0) return;
        if (drawGridGizmos)
        {
            //画竖线
            for (int i = 0; i <= gridColCount; i++)
            {
                var startV3 = new Vector3(i * gridCellWidth, 0, 0);
                var endV3 = new Vector3(i * gridCellWidth, 0, gridRowCount * gridCellHeight);
                Handles.DrawDottedLine(startV3, endV3, lineThickness);
            }

            //画横线
            for (int i = 0; i <= gridRowCount; i++)
            {
                var startV3 = new Vector3(0, 0, i * gridCellHeight);
                var endV3 = new Vector3(gridColCount * gridCellWidth, 0, i * gridCellHeight);
                Handles.DrawDottedLine(startV3, endV3, lineThickness);
            }
        }

        
        // 绘制导航网格的信息
        if (drawNaviGrids)
        {
            if (mapData != null && mapData.naviCells!=null && mapData.naviCells.Count > 0)
            {
                foreach (var naviCell in mapData.naviCells)
                {
                    /**
                    Handles.DrawAAConvexPolygon(new Vector3[] { 
                        naviCell.ldV3 + new Vector3(0.1f, 0, 0.1f), 
                        naviCell.luV3 + new Vector3(0.1f,0,-0.1f), 
                        naviCell.ruV3 + new Vector3(-0.1f,0,-0.1f), 
                        naviCell.rdV3 + new Vector3(-0.1f,0,0.1f)});
                    */

                    Handles.DrawWireDisc(new Vector3(naviCell.anchorPosX,naviCell.anchorPosY,naviCell.anchorPosZ),Vector3.up, 0.1f);
                    //Handles.Label(naviCell.anchorPos,naviCell.maxPassRadius.ToString());
                }
            }
        }
        
        
    }


#endif


}
