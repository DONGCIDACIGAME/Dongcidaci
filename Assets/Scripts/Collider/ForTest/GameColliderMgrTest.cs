using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameColliderMgrTest : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (EditorApplication.isPlaying == false) return;

        if (GameColliderManager.Ins == null) return;
        var _tempTestColliders = GameColliderManager.Ins.tempTestColliders;

        if (_tempTestColliders == null || _tempTestColliders.Count == 0) return;
        float lineThickness = 2f;
        Handles.color = Color.green;
        foreach (var collider in _tempTestColliders)
        {
            var colliderVertexs = collider.Convex2DShape.GetVertexs();
            if (colliderVertexs == null || colliderVertexs.Length == 0) continue;

            for (int i = 0; i < colliderVertexs.Length; i++)
            {
                int nextIndex = i + 1;
                if (nextIndex >= colliderVertexs.Length)
                {
                    //last point
                    nextIndex = 0;
                }

                Handles.DrawDottedLine(new Vector3(colliderVertexs[i].x, 0, colliderVertexs[i].y), new Vector3(colliderVertexs[nextIndex].x, 0, colliderVertexs[nextIndex].y), lineThickness);
            }

        }

    }


#endif
}
