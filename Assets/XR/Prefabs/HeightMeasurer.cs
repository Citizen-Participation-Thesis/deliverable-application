using System;
using UnityEngine;

namespace XR.Prefabs
{
    public class HeightMeasurer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sRenderer;

        private void Update()
        {
            sRenderer.transform.rotation *= Quaternion.Euler(0, 0, -5);
        }
    }
}