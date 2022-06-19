using Data;
using EventChannels;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Selection
{
    public class NewPlacementManager<T> 
    {
        private bool _selected;
        private Vector3? _position;
        private GameObject _blueprint;
        private GameObject _blueprintPrefab;
        private bool _eventRead = false;

        public void Update()
        {
            if (!(Input.touchCount > 0))
            {
                if (_blueprint != null && _eventRead)
                {
                    Object.Destroy(_blueprint);
                    _blueprint = null;
                    Debug.Log("Blueprint was deleted bc no touch");
                }
                return;
            }
            
            //Debug.Log("There was a touch!");

            if (EventSystem.current.IsPointerOverRaycastTargetUI())
            {
                if (_blueprint != null)
                {
                    Object.Destroy(_blueprint);
                    _blueprint = null;
                }
                return;
            }
            
            if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                Debug.Log("TOUCH ENDED OR WAS CANCELLED");
                if (_blueprint != null)
                {
                    StateEventChannel.RaiseConfirmedEvent();
                    _eventRead = false;
                }
                return;
            }
            
            var touchPoint = Input.GetTouch(0).position;
            var ray = Camera.main!.ScreenPointToRay(touchPoint);

            if (!Physics.Raycast(ray, out var hit))
            {
                _position = null;
                if (_blueprint != null)
                {
                    Object.Destroy(_blueprint);
                    _blueprint = null;
                }
                return;
            }
            
            var selection = hit.transform;
            
            var component = selection.GetComponent<T>();
            if (component == null) return;

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _blueprint = Object.Instantiate(_blueprintPrefab);
                _position = hit.point;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                if (_blueprint == null) _blueprint = Object.Instantiate(_blueprintPrefab);
                _position = hit.point;
            }
            
            if (_position != null)
            {
                _blueprint.transform.position = _position.Value;
                _blueprint.transform.localRotation = Quaternion.Euler(-90, Camera.main.transform.localEulerAngles.y, 0);
            }
        }

        private Vector3? GetPosition()
        {
            return _position;
        }

        public void SetBlueprint(GameObject blueprint)
        {
            _blueprintPrefab = blueprint;
            
            if (_blueprint == null) return;
            
            Object.Destroy(_blueprint);
            _blueprint = Object.Instantiate(_blueprintPrefab);
        }

        public GameObject GetBlueprint()
        {
            _eventRead = true;
            var returnCopy = Object.Instantiate(_blueprint);
            Object.Destroy(_blueprint);
            _blueprint = null;
            return returnCopy;
        }
    }
}