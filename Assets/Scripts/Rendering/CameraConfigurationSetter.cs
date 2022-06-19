using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


namespace Rendering
{
    public class CameraConfigurationSetter : MonoBehaviour
    {
        [SerializeField] private ARCameraManager cameraManager;
        private XRCameraConfiguration _config;

        private void OnEnable() => cameraManager.frameReceived += OnReceived;

        private void OnDisable() => cameraManager.frameReceived -= OnReceived;

        private void OnReceived(ARCameraFrameEventArgs obj)
        {
            if (!cameraManager.descriptor.supportsCameraConfigurations) return;
            if (!(cameraManager.subsystem is {running: true}))
            {
                Debug.Log("EITHER THERE WAS NO SUBSYSTEM OR IT IS ALREADY RUNNING");
            }
            
            using var configs = cameraManager.GetConfigurations(Allocator.Temp);
            
            Debug.Log($"{configs.Length} possible configs");
            var count = 0;
            
            foreach (var config in configs)
            {
                //cameraManager.subsystem.currentConfiguration = config;
                //cameraManager.currentConfiguration = config;
                Debug.Log("------------POSSIBLE CONFIG (" + ++count + ") ----------");
                Debug.Log(config);
            }

            //cameraManager.currentConfiguration = configs[];
            Debug.Log("------------CURRENT CONFIG (" + 6 + ") ----------");
            Debug.Log(cameraManager.currentConfiguration);

            cameraManager.frameReceived -= OnReceived;
        }
    }
}