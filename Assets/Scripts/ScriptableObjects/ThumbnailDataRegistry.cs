using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/ThumbnailDataRegistry")]
    public class ThumbnailDataRegistry : ScriptableObject
    {
        public List<ThumbnailData> thumbnailData;
    }
}