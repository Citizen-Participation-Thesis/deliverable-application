using Data;
using Models;
using States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject FirstTutorialPage;
    [SerializeField] private GameObject modeBar;

    private float _timer = 3.0f;
    private Renderer _renderer;
    private bool _started = false;

    void Start()
    {
        var building = Services.Get<ModelManager>().container;
        for (int i = 0; i < building.gameObject.transform.childCount; i++)
        {
            if(building.transform.GetChild(i).GetComponent<Movable>() != null)
            {
                Debug.Log("Movable "+building.transform.GetChild(i).name);
                _renderer = building.gameObject.transform.GetChild(i).GetComponent<Renderer>();
                return;
            }
        }
    }

    void Update()
    {

        if (_renderer.isVisible && _started == false)
        {
            _timer = 2.0f;
            _started = true;
        }
        if(_timer != 3.0f && _timer > 0.0f)
        {
            _timer -= Time.deltaTime;
        }
        if(_timer != 3.0f && _timer <= 0.0f)
        {
            StartTutorial();
        }
    }

    void StartTutorial()
    {
        _timer = 3.0f;
        FirstTutorialPage.SetActive(true);
    }

    public void TutorialEnd()
    {
        Services.Get<StateManager>().SetInitialState();
        modeBar.SetActive(true);
        Destroy(this.gameObject);
    }
}
