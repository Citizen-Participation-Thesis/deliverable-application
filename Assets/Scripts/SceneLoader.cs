using Data;
using States;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Button startButton;

    private void Awake()
    {
        Load();
    }

    private async void Load()
    {
        Debug.Log("Awaiting load materials");
        await Services.Get<DataManager>().LoadMaterials();
        Debug.Log("\tMaterials loaded");
        
        Debug.Log("Loading buttons");
        Services.Get<DataManager>().LoadModeButtons();
        Debug.Log("\tButtons loaded");
        
        startButton.GetComponentInChildren<TMP_Text>().SetText("Start");
        startButton.enabled = true;
        startButton.interactable = true;
        
        Debug.Log("EVERYTHING WAS LOADED");
    }
}