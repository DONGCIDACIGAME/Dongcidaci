using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
public class MavMeshOnLoad : MonoBehaviour
{
    private NavMeshSurface surface;//NavMeshSurface组件
                                   // Start is called before the first frame update

    public void GetSurfaceComponent()
    {
        surface = GetComponent<NavMeshSurface>();
    }


    /// <summary>
    /// 计算导航网格
    /// </summary>
    public void Bake()
    {
        surface.BuildNavMesh();
    }
}

