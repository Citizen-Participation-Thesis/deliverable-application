using UnityEngine;

namespace Selection
{
    public class RayCastBasedComponentSelector<T> : ISelector
    {
        private Transform _selection;
        
        public Transform GetSelection()
        {
            return _selection;
        }

        public RaycastHit Check(Ray ray)
        {
            _selection = null;
            
            if (!Physics.Raycast(ray, out var hit)) return default;
            
            var selection = hit.transform;
            var component = selection.GetComponent<T>();

            if (component == null) return default;

            _selection = selection;
            return hit;
        }
    }
}