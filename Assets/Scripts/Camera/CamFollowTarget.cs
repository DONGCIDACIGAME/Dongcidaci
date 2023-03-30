using UnityEngine;

public class CamFollowTarget : MonoBehaviour
{
    private GameObject mFollowTarget;
    private Vector3 mTargetLastPos;
    public float CamDis;
    public Vector3 CamDir;

    public void SetFollowTarget(GameObject target, Vector3 dir, float dis)
    {
        mFollowTarget = target;
        CamDis = dis;
        CamDir = dir;
        if(mFollowTarget != null)
        {
            mTargetLastPos = mFollowTarget.transform.position;
        }
        InitializeCam();
    }

    [ContextMenu("Initialize Cam")]
    public void InitializeCam()
    {
        if (mFollowTarget != null)
        {
            transform.position = -CamDir.normalized * CamDis + mFollowTarget.transform.position;
            transform.rotation = Quaternion.LookRotation(CamDir);
        }
    }

    public void OnUpdate(float deltaTime)
    {
        if (mFollowTarget == null)
            return;

        Vector3 posChange = mFollowTarget.transform.position - mTargetLastPos;

        transform.position += posChange;

        mTargetLastPos = mFollowTarget.transform.position;
    }
}
