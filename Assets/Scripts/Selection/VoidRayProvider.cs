using UnityEngine;

namespace Selection
{
    public class VoidRayProvider: IRayProvider
    {
        public Ray CreateRay()
        {
            return default;
        }
    }
}