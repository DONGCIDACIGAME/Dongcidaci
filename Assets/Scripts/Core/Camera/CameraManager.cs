using UnityEngine;

namespace GameEngine
{
    public class CameraManager : ModuleManager<CameraManager>
    {
        private Camera MainCam;
        private Camera UICam;

        public Camera GetMainCam()
        {
            return MainCam;
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

        public override void OnGameUpdate(float deltaTime)
        {

        }

        public override void OnLateUpdate(float deltaTime)
        {

        }

        public override void Initialize()
        {
            GameNodeCenter.Ins.InitializeCameraNodes();
            MainCam = GameNodeCenter.Ins.MainCamNode.GetComponent<Camera>();
            UICam = GameNodeCenter.Ins.UICamNode.GetComponent<Camera>();
        }

        public override void Dispose()
        {
            GameNodeCenter.Ins.DisposeCameraNodes();
            MainCam = null;
            UICam = null;
        }
    }
}
