using UnityEngine;

namespace Selection
{
    public interface IHitResponse
    {
        public void OnHit(RaycastHit hit);
        public void OnMiss();
    }
}