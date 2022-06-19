using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/ThumbnailData")]
    [Serializable]
    public class ThumbnailData : ScriptableObject
    {
        public string title;
        public Sprite thumbnailImage;
    }
}