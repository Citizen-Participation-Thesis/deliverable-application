using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ThumbnailRow : MonoBehaviour
    {
        [SerializeField] public TMP_Text title;
        [SerializeField] public GameObject content;
        [SerializeField] private Thumbnail thumbnailPrefab;

        private List<ThumbnailData> _thumbnailData;
        private int _childSkip;
        private void OnEnable()
        {
            _thumbnailData = Services.Get<DataManager>().ThumbnailDataRegistry.thumbnailData;
            _childSkip = 0;
            // Epic animation

        }

        private void OnDisable()
        {
            // Epic animation
        }

        public void Clear()
        {
            // Doing this backwards in case something scuffs up the indexing
            // e.g. i_0 is destroyed, now i_1 becomes i_0 and the list is shorter
            var childCount = content.transform.childCount;
            Debug.Log($"Clearing {childCount} children");
            if (childCount <= 0)
            {
                Debug.Log("\t Returning early, because there were no children");
                return;
            }
            for (var i = childCount-1; i >= 0; i--)
            {
                //Debug.Log($"\t\t clearing child {i}");
                Destroy(content.transform.GetChild(i).gameObject);
            }

            //Debug.Log($"\t {content.transform.childCount} children remain.");
        }
        
        public void Fill(List<string> alternatives, Action<int> action)
        {
            Debug.Log("Trying to fill bar (" + action.Method.Name + ") " + " with " + alternatives.Count + " thumbnails.");
            for (var i = 0; i < alternatives.Count; i++)
            {
                var thumbnailData = _thumbnailData.FirstOrDefault(data => data.title == alternatives[i]);
                if (thumbnailData == default) continue; 
                var thumbnail = Instantiate(thumbnailPrefab, content.transform);
                thumbnail.SetData(i, thumbnailData.title, thumbnailData.thumbnailImage, action);
            }

            var childCount = content.transform.childCount;
            
            // The thumbnails from earlier fills are not cleared immediately after calling Destroy,
            // so the indexing must account for the previously 
            Debug.Log($"\t Ended up with {childCount}, where {childCount - alternatives.Count} are stale.");
            
            _childSkip = childCount - alternatives.Count;
        }

        public void HighlightThumbnail(int thumbnailIndex)
        {
            //Debug.Log($"{title.text}");
            var childCount = content.transform.childCount;
            //Debug.Log($"Trying to highlight thumbnail {thumbnailIndex + _childSkip}th of {childCount-1}_0");
            for (var i = _childSkip; i < childCount; i++)
            {
                var thumbnail = content.transform.GetChild(i).GetComponent<Thumbnail>();
                thumbnail.SetHighlight(i == thumbnailIndex + _childSkip);
                if (i == thumbnailIndex + _childSkip) Debug.Log($"\t Highlighting thumbnail {i}");
            }

            _childSkip = 0;
        }
    }
}