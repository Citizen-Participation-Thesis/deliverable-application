using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
/*
namespace UI
{
    [RequireComponent(typeof(EventTrigger))]
    public class ContentScrollView : MonoBehaviour
    {
        [SerializeField] private List<GameObject> content;
        [SerializeField] private GameObject pipRow;
        [SerializeField] private TMP_Text debug;
    
        private List<GameObject> _pips;
        private int _selected = 0;

        private static readonly Vector3 NormalScale = new Vector3(1.0f, 1.0f, 1.0f);
        private static readonly Vector3 LargeScale = new Vector3(1.3f, 1.3f, 1.3f);

        private void Start()
        {
            AddTrigger();
            AddPips();
        }

        private void AddTrigger()
        {
            var trigger = GetComponent<EventTrigger>();
            var entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.EndDrag,
            };

            entry.callback.AddListener((data) => {OnEndDragDelegate((PointerEventData) data);});
            trigger.triggers.Add(entry);
        }

        private void AddPips()
        {
            _pips = new List<GameObject>();
            foreach (var entry in content)
            {
                var pip = Instantiate(entry, pipRow.transform, false);
                pip.transform.localScale = NormalScale;
                _pips.Add(pip);
            }
            _pips[_selected].transform.localScale = LargeScale;
        }

        private void OnStartDragDelegate(PointerEventData data)
        {
            debug.SetText("" + data.position.x);
        
        }

        private void OnEndDragDelegate(PointerEventData data)
        {
        
            if (data.delta.x > 0)
            {
                _pips[_selected].transform.localScale = NormalScale;
                _selected = _selected - 1 > 0 ? _selected - 1 : 0;
                _pips[_selected].transform.localScale = LargeScale;
            
            }
            else if (data.delta.x < 0)
            {
                _pips[_selected].transform.localScale = NormalScale;
                _selected = _selected + 1 < content.Count ? _selected + 1 : content.Count - 1;
                _pips[_selected].transform.localScale = LargeScale;
            
            }
        
            debug.SetText("" + data.delta.x);
        }
    }
}
*/