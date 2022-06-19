using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Rendering
{
    public class LightEstimation : MonoBehaviour
    {
        [SerializeField] private ARCameraManager arCameraManager;
        private Light _currentLight;

        private void Awake()
        {
            _currentLight = GetComponent<Light>();
        }

        private void OnEnable()
        {
            arCameraManager.frameReceived += ArCameraManagerOnFrameReceived;
        }
        
        private void OnDisable()
        {
            arCameraManager.frameReceived -= ArCameraManagerOnFrameReceived;
        }

        private void ArCameraManagerOnFrameReceived(ARCameraFrameEventArgs args)
        {
            if (args.lightEstimation.averageBrightness.HasValue)
            {
                _currentLight.intensity = args.lightEstimation.averageBrightness.Value;
            }
            if (args.lightEstimation.averageColorTemperature.HasValue)
            {
                _currentLight.colorTemperature = args.lightEstimation.averageColorTemperature.Value;
            }
            if (args.lightEstimation.colorCorrection.HasValue)
            {
                _currentLight.color = args.lightEstimation.colorCorrection.Value;
            }
        }

    }
}