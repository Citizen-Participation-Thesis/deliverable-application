using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UI
{
    public class ScreenshotManager : Singleton<ScreenshotManager>
    {
        [SerializeField] private Canvas ui;
        [SerializeField] private Canvas screenshotCanvas;
        [SerializeField] private ModeScrollView scrollView;
        [SerializeField] private RawImage rawImage;
        [SerializeField] private TMP_InputField inputField;
        
        private WebCamDevice _camDevice;
        private Texture2D _tempTexture;
        private byte[] _tempBytes;
        private byte[] _tempCamBytes;

        private void Awake()
        {
            // _camDevice = WebCamTexture.devices[0];
            // Debug.Log("Found following cam device " + _camDevice.name);
        }

        private void OnDestroy()
        {
            Destroy(_tempTexture);
        }

        private IEnumerator CaptureScreen()
        {
            yield return new WaitForEndOfFrame();
            
            // Capture the screen for preview in the app
            _tempTexture = ScreenCapture.CaptureScreenshotAsTexture();
            
            //_tempTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
            //_tempTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            _tempTexture.Apply();
            rawImage.texture = _tempTexture;
            
            // Capture the raw camera feed also?

            ui.gameObject.SetActive(true);
        }
        
        public void ExitCapture()
        {
            Debug.Log("EXITING CAMERA CAPTURE");
            scrollView.gameObject.SetActive(true);
            rawImage.texture = default;
            inputField.text = "";
            screenshotCanvas.gameObject.SetActive(false);
            Destroy(_tempTexture);
        }
        
        public void SaveToDevice()
        {
            Debug.Log("Saving screenshot and comment to " + Application.persistentDataPath);

            var timeString = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            var cleanedTime = Regex.Replace(timeString, @"[^\d]", "");

            var input = inputField.text;
            var jsonString = $"{{ img: {cleanedTime}.png, text: {input} }}"; 
            // Maybe also write out the scene info, time spent in the application, etc.
            
            
            File.WriteAllText(Application.persistentDataPath + "/" + cleanedTime + ".json", jsonString);

            _tempBytes = _tempTexture.EncodeToPNG();
            File.WriteAllBytes(Application.persistentDataPath + "/" + cleanedTime + ".png", _tempBytes);
        }

        public void TakeScreenshot()
        {
            ui.gameObject.SetActive(false);

            StartCoroutine(CaptureScreen());
            
            // Set correct canvas active in the now inactive UI
            screenshotCanvas.gameObject.SetActive(true);
        }
        
        /*
        [SerializeField] private Canvas canvas;
        [SerializeField] private RawImage rawImage;
        [SerializeField] private Button cancelButton;

        private Texture2D _tempTexture;
    
        private void Start()
        {
            cancelButton.onClick.AddListener(CancelScreenshot);
        }

        private IEnumerator GrabScreenshotBytes()
        {
            yield return new WaitForEndOfFrame();
            _tempTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        
            _tempTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            _tempTexture.Apply();
        
            rawImage.texture = _tempTexture;
            canvas.gameObject.SetActive(true);
        }

        public void TakeScreenshot()
        {
            canvas.gameObject.SetActive(false);
            StartCoroutine(nameof(GrabScreenshotBytes));
            //image.gameObject.SetActive(true);
            rawImage.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(true);
        }

        private void CancelScreenshot()
        {
            rawImage.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(false);
            // rawImage.texture = default;
            // Destroy(_tempTexture)
        }
        */
    }
}
