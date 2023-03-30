using UnityEngine;

namespace GameEngine
{
    public class CameraManager : ModuleManager<CameraManager>
    {
        private Camera MainCam;

        public void ChangeMainCam(Camera cam)
        {
            if(cam == null)
            {
                Log.Error(LogLevel.Critical, "ChangeMainCam Failed, target cam is null!");
                return;
            }

            MainCam = cam;
        }

        public Camera GetMainCam()
        {
            return MainCam;
        }

        public void CameraMoveTo(Vector3 pos)
        {
            if(MainCam != null)
            {
                MainCam.transform.position = pos;
            }
        }

        public Vector3 GetCamPos()
        {
            if (MainCam == null)
                return Vector3.zero;

            return MainCam.transform.position;
        }

        public void CameraMoveTo(Vector3 pos, float duration)
        {
            if(duration == 0)
            {
                CameraMoveTo(pos);
            }
            else
            {

            }
        }

        public float GetCamSize()
        {
            if (MainCam == null)
                return 0;

            return MainCam.orthographicSize;
        }

        public void CamZoomTo(float size)
        {
            if (MainCam != null)
            {
                MainCam.orthographicSize = size;
            }
        }

        public void CamZoomTo(float size, float duration)
        {
            if(duration == 0)
            {
                CamZoomTo(size);
            }
            else
            {

            }
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void OnLateUpdate(float deltaTime)
        {

        }

        public override void Initialize()
        {
            MainCam = GameObject.Find("_MAIN_CAMERA").GetComponent<Camera>();
        }

        public override void Dispose()
        {
            MainCam = null;
        }
    }
}
