using System.Collections.Generic;
using System.Linq;
using Data;
using States;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ModeScrollView : MonoBehaviour
    {
        [SerializeField] private GameObject modeButtonPrefab;
        [SerializeField] private ScrollRect scrollRect;
        
        [SerializeField] private float snapSpeed;
        [SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;
        
        private float _gap;
    
        private bool _released;
        private float _nextSnap;
        private float _thresholdSpeed;
        private List<string> _alternatives;

        private void Start()
        {
            _alternatives = GetRelevantModes();
            
            //ModeButtonProvider.GenerateModeButtons(scrollRect.content.transform, _alternatives);
            var modeButtons = Services.Get<DataManager>().ModeButtons;
            for (var i = 0; i < _alternatives.Count; i++)
            {
                var alternative = _alternatives[i];
                if (!modeButtons.TryGetValue(alternative, out var buttonSprite)) continue;
                
                Debug.Log(alternative + " has button");
                
                var modeButton = Instantiate(modeButtonPrefab, scrollRect.content.transform);
                modeButton.GetComponent<ModeButton>().SetData(i, alternative, buttonSprite, SetNextSnap);
            }
            
            // Pad so that the midline of the first and last buttons are centered on the screen.
            // Subtract half the width of the button prefab
            horizontalLayoutGroup.padding.left = UIConstants.VirtualWidth / 2 - 70; 
            horizontalLayoutGroup.padding.right = UIConstants.VirtualWidth / 2 - 70;
                        
            _gap = 1f / (_alternatives.Count - 1f);
            _released = true;

            var startIndex = GetStartIndex();
            
            scrollRect.horizontalNormalizedPosition = startIndex * _gap; 
            _nextSnap = scrollRect.horizontalNormalizedPosition;

            Debug.Log("The first index is " + startIndex);
        }

        private void SetNextSnap(int nextSnap)
        {
            Debug.Log("NEXT SNAP SET: " + nextSnap);
            _nextSnap = nextSnap * _gap;
            _released = true;
            
            Services.Get<StateManager>().ChangeState(_alternatives[nextSnap]);
        }

        private float GetStartIndex()
        {
            var stateManager = Services.Get<StateManager>();
            
            if (stateManager.PreviousStateData == null) return 0;
            var currentIndex = _alternatives.FindIndex(n => n == stateManager.PreviousStateData.className);
            
            return currentIndex < 0 ? 0 : currentIndex;
        }

        private void FixedUpdate()
        {
            if (_released)
            {
                scrollRect.horizontalNormalizedPosition =
                    Mathf.Lerp(scrollRect.horizontalNormalizedPosition, _nextSnap, snapSpeed);
            }
        }

        private List<string> GetRelevantModes()
        {
            var stateManager = Services.Get<StateManager>();
            var relevantStates = stateManager.stateData.Where(s => s.IsRelevant()).ToList();
            var relevantStrings = relevantStates.Select(state => state.modeName).ToList();
            return relevantStrings;
        }

        public void OnBeginDrag()
        {
            _released = false;
        }

        public void OnEndDrag()
        {
            var quot = (int) (scrollRect.horizontalNormalizedPosition / _gap);
            var rem = scrollRect.horizontalNormalizedPosition % _gap;
            var index = 0;
            
            if (rem > _gap * 0.5f)
            {
                _nextSnap = (quot + 1) * _gap;
                index = quot + 1;
            }
            else if (rem < _gap * 0.5f)
            {
                _nextSnap = quot * _gap;
                index = quot;
            }
            
            _nextSnap = Mathf.Clamp(_nextSnap, 0, 1);
            _released = true;

            index = Mathf.Clamp(index, 0, _alternatives.Count - 1);
            
            Services.Get<StateManager>().ChangeState(_alternatives[index]);
        }
    }
}
