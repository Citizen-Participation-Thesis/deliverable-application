using UnityEngine;

namespace Selection
{
    public interface IRayProvider
    {
        public Ray CreateRay();
    }
}