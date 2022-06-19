using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace Models
{
    public class Temp: MonoBehaviour
    {
        private Renderer _renderer;

        private List<string> _materials;
        private List<int> _materialIndexMap;
        private Dictionary<string, List<KeyValuePair<string, Material>>> _materialAlternatives;

        private Material _defaultMaterial;
        private string _currentMaterial;
        
        private bool _slotSelected;
        
        private int _materialIndex;
        private int _materialAlternativeIndex;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            _materialIndex = 0;
            _materialAlternativeIndex = 0;
            _slotSelected = true;
            
            GeneratePlaceholderAlternatives();
            
            _currentMaterial = _materials[0];
            _defaultMaterial = _materialAlternatives[_currentMaterial][0].Value;
        }

        private void GeneratePlaceholderAlternatives()
        {
            _materials = new List<string>();
            _materialIndexMap = new List<int>();
            _materialAlternatives = new Dictionary<string, List<KeyValuePair<string, Material>>>();
            var materials = _renderer.materials;
            
            foreach (var materialsKey in Services.Get<DataManager>().Materials.Keys)
            {
                Debug.Log("KEY IN DM KEYS: " + materialsKey);
                foreach (var entry in Services.Get<DataManager>().Materials[materialsKey])
                {
                    Debug.Log("\t MATERIAL IN THAT KEY: " + entry.Key + " and non-null material: " + (entry.Value != null));
                }
            }

            for (var index = 0; index < materials.Length; index++)
            {
                var material = materials[index];
                _defaultMaterial = material;
                var alternativesDict = new List<KeyValuePair<string, Material>>();

                var mat = material.name;
                var parts = mat.Split('_');

                var myMats = Services.Get<DataManager>().Materials;
                var materialDesignation = parts[0].ToLower();

                var materialName = parts
                    .Skip(1)
                    .ToArray()
                    .Aggregate((i, j) => i + " " + j)
                    .Replace(" (Instance)", "");

                Debug.Log("on model material: " + materialDesignation);
                if (myMats.ContainsKey(materialDesignation))
                {
                    Debug.Log("was linked to material library, with name " + materialName);
                    
                    alternativesDict.Add(new KeyValuePair<string, Material>(materialName, material));

                    foreach (var material1 in myMats[materialDesignation])
                    {
                        alternativesDict.Add(new KeyValuePair<string, Material>(material1.Key, material1.Value));
                    }

                    _materials.Add(materialDesignation);
                    _materialIndexMap.Add(index);
                    _materialAlternatives.Add(materialDesignation, alternativesDict);
                }
            }

            foreach (var materialsKey in _materialAlternatives.Keys)
            {
                Debug.Log("KEY IN MA KEYS: " + materialsKey);
                foreach (var entry in _materialAlternatives[materialsKey])
                {
                    Debug.Log("\t MATERIAL IN THAT KEY: " + entry.Key + " and non-null material: " + (entry.Value != null));
                }
            }
            
            foreach (var index in _materialIndexMap)
            {
                Debug.Log("INDEX TRANS: " + index);
            }
        }

        private void Update()
        {
            // Lerp between changes or have some cool transition
            if (!_slotSelected)
                _renderer.materials[_materialIndex].color =
                    Color.Lerp(_defaultMaterial.color, Color.white, Mathf.PingPong(Time.time, 0.8f));
        }

        public string GetCurrentMaterial()
        {
            return _currentMaterial;
        }

        public int GetCurrentMaterialAlternativeIndex()
        {
            return _materialAlternativeIndex;
        }

        public int GetCurrentMaterialIndex()
        {
            return _materialIndex;
        }

        public void ChangeMaterial(int alternativeIndex)
        {
            Debug.Log("IN MATERIAL CHANGER CHANGE MATERIAL");
            _slotSelected = false;

            var materials = _renderer.materials;
            materials[_materialIndex].color = _defaultMaterial.color;
            
            Debug.Log("Default Material: " + _defaultMaterial);
            Debug.Log("Color reset on: " + materials[_materialIndex].name);
            Debug.Log("Default Material to be: " + materials[_materialIndexMap[alternativeIndex]] + " with defindex: " + alternativeIndex + " and transindex: " + _materialIndexMap[alternativeIndex]);
            
            _materialIndex = _materialIndexMap[alternativeIndex];
            _defaultMaterial = materials[_materialIndex];
            
            Debug.Log(_defaultMaterial.name);
            Debug.Log(materials[_materialIndex]);
            Debug.Log(materials[_materialIndex].name);
            
            _currentMaterial = materials[_materialIndex].name.Split('_')[0].ToLower();
        }

        public void ChangeMaterialAlternative(int alternativeIndex)
        {
            _slotSelected = true;
            _materialAlternativeIndex = alternativeIndex;
            
            Debug.Log("CHANGING CURRENT " 
                      + _currentMaterial + " TO ALT " 
                      + _materialAlternatives[_currentMaterial][alternativeIndex].Key + " WITH MAT " 
                      + _materialAlternatives[_currentMaterial][alternativeIndex].Value.name);
            
            Debug.Log("TARGET MATERIAL: " + _renderer.materials[_materialIndex].name);
            
            _renderer.materials[_materialIndex] = _materialAlternatives[_currentMaterial][alternativeIndex].Value;

            _defaultMaterial = _materialAlternatives[_currentMaterial][alternativeIndex].Value;
        }

        public void ResetValues()
        {
            _slotSelected = true;
            _renderer.materials[_materialIndex] = _defaultMaterial;
            
        }

        public List<string> GetMaterials()
        {
            return _materials;
        }

        public List<string> GetAlternativeMaterials(string materialName)
        {
            Debug.Log("FETCHING MATERIAL ALTERNATIVES FOR: " + materialName);
            return _materialAlternatives[materialName].Select(p => p.Key).ToList();
        }
    }
}