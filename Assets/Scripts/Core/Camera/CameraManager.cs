using UnityEngine;

namespace GameEngine
{
    public class CameraManager : ModuleManager<CameraManager>
    {
        private Camera MainCam;
        private Camera UICam;

        public void SetMainCam(Camera cam)
        {
            if(cam == null)
            {
                Log.Error(LogLevel.Critical, "SetMainCam Failed, target cam is null!");
                return;
            }

            MainCam = cam;
        }

        public Camera GetMainCam()
        {
            return MainCam;
        }

        public void SetUICamera(Camera camera)
        {
            UICam = camera;
        }

        public Camera GetUICam()
        {
            return UICam;
        }

        public void MainCameraMoveTo(Vector3 pos)
        {
            if(MainCam != null)
            {
                MainCam.transform.position = pos;
            }
        }

        public void MainCameraMoveTo(Vector3 pos, float duration)
        {
            if(duration == 0)
            {
                MainCameraMoveTo(pos);
            }
            else
            {

            }
        }

        public void MainCamZoomTo(float size)
        {
            if (MainCam != null)
            {
                MainCam.orthographicSize = size;
            }
        }

        public void MainCamZoomTo(float size, float duration)
        {
            if(duration == 0)
            {
                MainCamZoomTo(size);
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
            UICam = GameObject.Find("_UI_CAMERA").GetComponent<Camera>();
        }

        public override void Dispose()
        {
            MainCam = null;
            UICam = null;
        }
    }
}
