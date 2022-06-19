using UnityEngine;

namespace Selection
{
    public class ScreenCenterRayProvider: IRayProvider
    {
        private readonly Vector3 _screenCenter;

        public ScreenCenterRayProvider()
        {
            _screenCenter = new Vector3(Screen.safeArea.width / 2, Screen.safeArea.height / 2, 0);
        }
        
        public Ray CreateRay()
        {
            return Camera.main!.ScreenPointToRay(_screenCenter);
        }
    }
}