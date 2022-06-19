using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Data
{
    public static class EventSystemExtensions
    {
        public static bool IsPointerOverRaycastTargetUI(this EventSystem eventSystem) 
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y)
            };
            
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0; 
        }
    }
}