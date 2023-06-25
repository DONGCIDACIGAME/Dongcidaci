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
            var lines = GameColliderHelper.GetRectangleLines(collider);

            for (int i = 0; i < 4; i++)
            {
                //var line = lines[i,2];
                var v0 = lines[i,0];
                var v1 = lines[i, 1];
                Handles.DrawDottedLine(new Vector3(v0.x,0,v0.y), new Vector3(v1.x,0,v1.y), lineThickness);
            }

            // 绘制旋转角度的label
            var pos = GameColliderHelper.GetRealPosition(collider.AnchorPos,collider.AnchorAngle,collider.Offset,collider.Size);
            var angle = GameColliderHelper.GetRealRotateAngle(collider.AnchorAngle,collider.Offset.x,collider.Offset.y);
            Handles.Label(new Vector3(pos.x,0,pos.y),angle.ToString());
        }






    }


#endif
}
