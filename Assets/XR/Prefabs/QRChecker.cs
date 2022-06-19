using Data;
using Models;
using States;
using UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace XR.Prefabs
{
    public class QRChecker : MonoBehaviour
    {
        [SerializeField] private GameObject worldPrefab;
        [SerializeField] private FillBar fillBar;
        
        [SerializeField] private ARTrackedImageManager trackedImageManager;
        
        [SerializeField] private float targetTime;
        [SerializeField] private Button button;
        [SerializeField] private GameObject modeBar;
        [SerializeField] private GameObject tutorialMenu;


        private bool _prefabPlaced;
        private float _timer;
        private TrackableId _trackableId;

        private void OnEnable()
        {
            trackedImageManager.trackedImagesChanged += OnChanged;
            _timer = targetTime;
            _prefabPlaced = false;
        }

        private void OnDisable() => trackedImageManager.trackedImagesChanged -= OnChanged;

        private void Update()
        {
            if (_prefabPlaced) return;
            if (!ImageIsTracked()) return;

            _timer -= Time.deltaTime;
            
            fillBar.SetTargetValue(1f - _timer/targetTime);

            if (!(_timer <= 0.0f)) return;
            
            Debug.Log("TIMER BELOW 0");
            
            PlacePrefab();
            
            gameObject.SetActive(false);
            button.gameObject.SetActive(false);
        }

        private bool ImageIsTracked()
        {
            if (_trackableId == default) return false;
            return trackedImageManager.trackables[_trackableId].trackingState == TrackingState.Tracking;
        }

        private void PlacePrefab()
        {
            var latestTrackable = trackedImageManager.trackables[_trackableId];
            var trackableTransform = latestTrackable.transform;
            var prefab = Instantiate(
                worldPrefab, 
                new Vector3(trackableTransform.position.x, trackableTransform.position.y-0.70f, trackableTransform.position.z),
                Quaternion.Euler(new Vector3(0, trackableTransform.eulerAngles.y, 0)));

            /* For a QR placement 0.93 m above ground placed vertically
             
            var p = Instantiate(
                worldPrefab, 
                new Vector3(trackableTransform.position.x, -0.93f, trackableTransform.position.z),
                Quaternion.Euler(-90, 0, 0),
                trackableTransform
                );
            */

            
            _prefabPlaced = true;
            
            /*
            Debug.Log($"\tPrefab: {prefab.name}" );
            Debug.Log($"\t with center in global position {trackableTransform.position}");
            Debug.Log($"\t with global rotation {trackableTransform.rotation}");
            
            Debug.Log("CAMERA CONFIGURATION PER AR CAMERA MANAGER");
            Debug.Log(Camera.main.GetComponent<ARCameraManager>().currentConfiguration);
            Debug.Log("CAMERA PER CAMERA");
            Debug.Log($"\t ASPECT: {Camera.main.aspect.ToString()}");
            Debug.Log($"\t FOV: {Camera.main.fieldOfView.ToString()}");
            Debug.Log($"\t Projection Matrix: {Camera.main.projectionMatrix.ToString()}");
            Debug.Log($"\t PIXEL HEIGHT, WIDTH: {Camera.main.pixelHeight}, {Camera.main.pixelWidth}");
            */
            
            trackableTransform.gameObject.SetActive(false);
            trackedImageManager.trackedImagesChanged -= OnChanged;
            trackedImageManager.enabled = false;

            Services.Get<ModelManager>().container = prefab;

            tutorialMenu.SetActive(true);
        }

        private void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach (var newImage in eventArgs.added)
            {
                Debug.Log("NEW IMAGE TRACKED");
                _trackableId = newImage.trackableId;
            }
        }
    }
}