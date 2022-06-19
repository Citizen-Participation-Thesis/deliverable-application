using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Selection
{
    public class TouchRayProvider : IRayProvider
    {
        public Ray CreateRay()
        {
            if (!Input.GetMouseButton(0)) return default;
            
            if (IsPointerOverRaycastTargetUI())
            {
                Debug.Log("EVENT SYSTEM::OVER GAME OBJECT");
                return default;
            }
            
            var touchPoint = Input.GetTouch(0).position;
            return Camera.main!.ScreenPointToRay(touchPoint);
        }

        private static bool IsPointerOverRaycastTargetUI() 
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };
            
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0; 
        }
    }
}