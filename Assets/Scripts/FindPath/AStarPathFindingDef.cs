using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

  /** AroundNode
    * @brief 当前节点周围 格的索引号
    * @remark 
    */
public    struct AroundNode 
    {
	    //!节点周围格的索引值
	    public int m_aroundNodeIndex;
	    //!节点周围格的hope值
	    public int m_hope;
    };

    /** A_StarPathFinding
    * @brief  节点信息
    * @remark 
    */

 public class A_Star_Node 
    {	
        //! F 值 f(n)是节点n的综合优先级
        public int   m_movementCost;
        //! node 三维坐标
        public float x;
        public float y;
        public float z;
        //! H 值；h(n)是节点n距离终点的预计代价 用于启发算法
        public int   m_score;
        //! G 值 g(n) 是节点n距离起点的代价
        public int   m_hope;
        //! 父节点ID
        public int   m_fatherId;
        //! 标记节点在哪个列表
        public int	  m_flag;
        //!标记节点能否站立
        public bool  m_isArises;
        //!节点周围格 定长为8
        public AroundNode[] m_aroundNode = new AroundNode[8];
    };

public class FVector 
{};

    enum ENMaxCount
    {
        enMaxCount = 300,
        enMaxPathCount = 3000,
    };

public class AStarPathFinding
{
  
    class NodeComparator : IComparer<A_Star_Node>
    {
        public int Compare(A_Star_Node x, A_Star_Node y)
        {
            return x.m_movementCost.CompareTo(y.m_movementCost);
        }
    };

    /*
    	void Enqueue(TItem node, TPriority priority);
	    TItem Dequeue();
	    void Clear();
	    bool Contains(TItem node);
	    void Remove(TItem node);
	    void UpdatePriority(TItem node, TPriority priority);
	    TItem First { get; }
	    int Count { get; }
    */
   
    



	//!构造函数，作用是得到地图信息
	public AStarPathFinding(ref CollisionTile collsionTile) {}
    ~AStarPathFinding() {}
	
	//!寻路函数，负责寻找路径
	public bool FindPath(ref Vector3 srcPostion, ref Vector3 dstPosition, ref List<int> path, uint maxStepCount = 1500){
		int endNodeIndex;

		//启示格子和目标格子的二维转一维索引
		int endLengthIndex = 0, endWidthIndex = 0, ArrayLengthIndex = 0, ArrayWidthIndex = 0;

		float p_ins = 0.0f;
		if (!m_collisionTile.GetLocation(srcPostion.x, srcPostion.z,ref p_ins,ref p_ins,ref p_ins,ref ArrayLengthIndex,ref ArrayWidthIndex))
		{
			return false;
		}

		//是否直接是引用类型？
		A_Star_Node node = m_nodeList[ArrayWidthIndex + m_collisionTile.GetHeight() * ArrayLengthIndex];

		if (!m_collisionTile.GetLocation(dstPosition.x, dstPosition.z, ref m_destinationPos.x, ref p_ins, ref m_destinationPos.z, ref endLengthIndex, ref endWidthIndex))
		{
			return false;
		}

		endNodeIndex = endWidthIndex + m_collisionTile.GetHeight() * endLengthIndex;
		
		if (endNodeIndex > m_nodeList.Count || 
			endNodeIndex < 0)
			return false;
		
		m_openCount = 0;

		// 把起始格加到开启列表
		this.OpenNote(ArrayLengthIndex, ArrayWidthIndex, 0, 0, 0, 0);

		uint stepCount = 0;

		while (m_openCount > 0 && stepCount < maxStepCount)
		{
			stepCount++;
			int LengthArrayIndex = 0, WidthArrayIndex = 0;

			// ！从开启列表中取出当前格。
			m_keepNode = m_priorityQueue.Dequeue();

			// ！计算当前格在节点列表中的索引
			m_collisionTile.GetLocation(m_keepNode.x, m_keepNode.z, ref p_ins,ref  p_ins,ref  p_ins,ref LengthArrayIndex,ref WidthArrayIndex);

			// ！获得父ID。
			m_fatherId = WidthArrayIndex + m_collisionTile.GetHeight() * LengthArrayIndex;

			// ！把节点放到关闭列表中
			this.CloseNote(m_fatherId);

			// ！遍历当前格的周围格子。把没有在开启列表的放到开启列表，已经在开启列表的比较移动耗费是否更低，如果是就替换开启列表中的移动耗费和父ID，否则什么都不做
			for (int i = 0; i < 8; i++)
			{
				if (m_keepNode.m_aroundNode[i].m_aroundNodeIndex != -1)
				{
					A_Star_Node note = m_nodeList[m_keepNode.m_aroundNode[i].m_aroundNodeIndex];
					int movementCost;
					int score;
					int hope = m_keepNode.m_aroundNode[i].m_hope;

					if (note.m_flag != m_closeFlag && note.m_isArises)
					{
						hope += m_keepNode.m_hope;
						score = (int)((Math.Abs(m_destinationPos.x - note.x) * 10) + (Math.Abs(m_destinationPos.z - note.z) * 10));
						// ！移动耗费
						movementCost = hope + score;
						//! 判断格子是否在开启列表中
						bool isCotainInOpen = this.IsOpen(ref note);
						// ！不在开启列表
						if (!isCotainInOpen)
						{
							m_collisionTile.GetLocation(note.x, note.z, ref p_ins,ref  p_ins,ref  p_ins,ref LengthArrayIndex,ref WidthArrayIndex);
							this.OpenNote(LengthArrayIndex, WidthArrayIndex, score, movementCost, m_fatherId, hope);
						}

						// ！在开启列表
						else if (note.m_movementCost > movementCost)
						{
							note.m_movementCost = movementCost;
							note.m_fatherId = m_fatherId;
							note.m_score = score;
							note.m_hope = hope;
							m_priorityQueue.UpdatePriority(note, movementCost);
						}
					}
				}
			}

			//! 如果找到目标，返回path
			if (m_keepNode == m_nodeList[endNodeIndex])
			{
				this.GetAStarPath(ref node, m_fatherId,ref path);
				m_priorityQueue.Clear();

				if (path.Count == 0)
				{
					return false;
				}
				return true;
			}
		}
		m_priorityQueue.Clear();
		path.Clear();
		return false;
	}
	//! 获得节点位置
	public  bool GetNodePosition(int nIndex,ref Vector3 position){
		if (nIndex >= 0 &&
			nIndex < m_nodeList.Count)
		{
			position.x = m_nodeList[nIndex].x;
			position.y = m_nodeList[nIndex].y;
			position.z = m_nodeList[nIndex].z;
			return true;
		}
		return false;
	}
	//!把一个节点放到开启列表中 参数1是节点。参数2 是曼哈顿距离，参数3 是 最终评分，参数4 是 节点父ID
	//曼哈顿距离 参考https://blog.csdn.net/weixin_43135178/article/details/115426550
	public void OpenNote(int nLengthArray, int nWidthArray, int p_score, int p_cost, int p_fatherId, int p_hope){
		if (nWidthArray + m_collisionTile.GetHeight() * nLengthArray < m_nodeList.Count)
			{
				A_Star_Node node = m_nodeList[nWidthArray + m_collisionTile.GetHeight() * nLengthArray];
				node.m_score = p_score;
				node.m_movementCost = p_cost;
				node.m_fatherId = p_fatherId;
				node.m_hope = p_hope;
				m_priorityQueue.Enqueue(node, p_cost);
			}
		m_openCount++;
	}
	//!把一个节点放到关闭列表中
	public void CloseNote(int nIndex)
	{
		m_openCount--;
		if (m_openCount < 0)
		{
			m_openCount = 0;
			return;
		}
		m_nodeList[nIndex].m_flag = m_closeFlag;

	}
	//! 判断节点是否在开启列表中
	public bool  IsOpen(ref A_Star_Node node)
	{
		return m_priorityQueue.Contains(node);
	}

	//! 获得节点周围的节点
	public void GetAround(int arrayIndex, AroundNode[] aroundNode){}
	//! 获得最终的路径
	public void GetAStarPath(ref A_Star_Node startNode, int fatherId, ref List<int> path){}
	//////////////////////////////////////////////////////////////////////////
	//!获得路径点
	public void GetAStarPathPoints(ref List<int> path){}
	//////////////////////////////////////////////////////////////////////////
	//!  检测能否向目标点移动
	public float GetFloorVectorProportion(A_Star_Node startPostion,A_Star_Node moveVector)
	{
		return 0.0f;
	}
	//!获取Tile的对象
	public  CollisionTile GetCollisionTileObj(){
		return m_collisionTile;
	}
	
	//! 节点列表 存放所有节点信息
	public List<A_Star_Node> m_nodeList;
	//! 标记节点是否在关闭列表中
	public int   m_closeFlag;

	//! 开启列表。用来存放要被检测的节点
    private SimplePriorityQueue<A_Star_Node> m_priorityQueue;
	//! 用来控制循环。m_openCount ==0 的时候说明没有找到路
	private  int  m_openCount;
	//! 节点的父节点ID
	private int  m_fatherId;
	//! 当前被选中的节点
	private A_Star_Node m_keepNode; 
	//! 目标位置
	private A_Star_Node m_destinationPos;

	private CollisionTile            m_collisionTile;
	//////////////////////////////////////////////////////////////////////////
	//!当前路径
	List<int> m_currentPathPoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
