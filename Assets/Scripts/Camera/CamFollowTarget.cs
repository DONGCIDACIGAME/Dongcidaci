using UnityEngine;
using GameEngine;

public class CamFollowTarget : MonoBehaviour
{
    private GameObject mFollowTarget;
    private Vector3 mTargetLastPos;
    public Vector3 CamDir;

    public void SetFollowTarget(GameObject target)
    {
        mFollowTarget = target;
        if(mFollowTarget != null)
        {
            mTargetLastPos = mFollowTarget.transform.position;
        }
        CamDir = CameraManager.Ins.GetMainCam().transform.rotation.eulerAngles;
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
