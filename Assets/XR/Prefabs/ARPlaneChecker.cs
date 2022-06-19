using System;
using UI;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace XR.Prefabs
{
    public class ARPlaneChecker : MonoBehaviour
    {
        [SerializeField] private float targetArea;
        [SerializeField] private FillBar fillBar;
        [SerializeField] private Button button;
        
        [SerializeField] private ARPlaneManager planeManager;
        [SerializeField] private ARRaycastManager raycastManager;
        
        [SerializeField] private GameObject qrPrompt;
        
        private void OnEnable() => planeManager.planesChanged += PlanesChanged;
        private void OnDisable() => planeManager.planesChanged -= PlanesChanged;

        private void PlanesChanged(ARPlanesChangedEventArgs obj)
        {
            var area = 0f;

            foreach (var plane in planeManager.trackables)
            {
                area += plane.size.x * plane.size.y;   
            }
            
            var frac = area / targetArea;

            if (frac > 1f)
            {
                gameObject.SetActive(false);
                qrPrompt.SetActive(true);
                button.gameObject.SetActive(true);

                return;
            }
            
            fillBar.SetTargetValue(frac);
        }
    }
}