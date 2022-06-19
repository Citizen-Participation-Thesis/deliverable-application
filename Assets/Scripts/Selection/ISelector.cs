using UnityEngine;

namespace Selection
{
    public interface ISelector
    {
        Transform GetSelection();
        RaycastHit Check(Ray ray);
    }
}