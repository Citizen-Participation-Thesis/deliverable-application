using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace Models
{
    public class ModelManager : Singleton<ModelManager>
    {
        public GameObject container;

        public List<Transform> GetModelsWithComponent<T>() where T : MonoBehaviour
        {
            return container
                .GetComponentsInChildren<T>()
                .Select(c => c.transform)
                .ToList();
        }
        
        
    }
}
