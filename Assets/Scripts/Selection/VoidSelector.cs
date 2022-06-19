using UnityEngine;

namespace Selection
{
    public class VoidSelector: ISelector
    {
        public Transform GetSelection()
        {
            return default;
        }

        public RaycastHit Check(Ray ray)
        {
            return default;
        }
    }
}