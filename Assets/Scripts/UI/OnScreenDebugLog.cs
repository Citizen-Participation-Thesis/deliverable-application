using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class OnScreenDebugLog : MonoBehaviour
    {
        private static TMP_Text _tmpText;
        private const int Length = 16;
        private static readonly string[] Messages = new string[Length];
        private static int _counter;

        private void Awake()
        {
            _tmpText = gameObject.GetComponent<TMP_Text>();
            Application.logMessageReceived += WriteToScreen;
        }

        private static void WriteToScreen(string logString, string stackTrace, LogType type)
        {
            var newDebugLog = "";

            for (var i = 0; i < Messages.Length - 1; i++)
            {
                Messages[i] = Messages[i + 1];
            }

            Messages[Messages.Length - 1] = logString;

            for (var i = 0; i < Messages.Length; i++)
            {
                var message = Messages[i];
                if (message == default) continue;
                var count = _counter - Messages.Length + i + 1;
                newDebugLog += count + ": " + message + "\n";
            }

            _tmpText.SetText(newDebugLog);
            _counter++;

            //if (newDebugLog.Contains("Exception")) Application.logMessageReceived -= WriteToScreen;
        }
    }
}
