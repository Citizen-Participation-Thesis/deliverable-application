using Data;
using EventChannels;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Selection
{
    public class TouchManager<T>
    {
        private bool _selected;
        
        public void Update()
        {
            if (!(Input.touchCount > 0)) return;

            if (EventSystem.current.IsPointerOverRaycastTargetUI())
            {
                return;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                Debug.Log("TOUCH ENDED OR WAS CANCELLED");
                //OnTouchUp();
                var touchPoint = Input.GetTouch(0).position;
                var ray = Camera.main!.ScreenPointToRay(touchPoint);

                if (!Physics.Raycast(ray, out var hit)) return;
                
                var selection = hit.transform;
                var component = selection.GetComponent<T>();

                if (component == null)
                {
                    if (!_selected) return;
                    
                    FocusEventChannel.RaiseUnfocusedEvent();
                    _selected = false;
                    
                    return;
                }
                
                FocusEventChannel.RaiseFocusedEvent(selection);
                _selected = true;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Debug.Log("TOUCH BEGAN");
                //OnTouchDown();
            }
        }
    }
}