using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace Models
{
    public class MaterialChanger : MonoBehaviour, ICategoryData
    {
        private Dictionary<string, List<KeyValuePair<string, Material>>> _materialAlternativesDictionary;
        private List<string> _materialNames;
        private List<KeyValuePair<string, Material>> _currentMaterialAlternatives;

        private Renderer _renderer;

        private List<int> _materialIndices;
        private int _currentCategoryIndex;
        private int _currentAlternativeIndex;

        public event Action AlternativeSet;
        public event Action CategorySet;

        private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
        private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
        private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");
        private static readonly int Color1 = Shader.PropertyToID("_Color");

        private void OnEnable()
        {
            Debug.Log("IN MATERIAL CHANGER ENABLEEEED");
            _renderer = GetComponent<Renderer>();

            _materialIndices = new List<int>();
            _materialNames = new List<string>();
            _currentMaterialAlternatives = new List<KeyValuePair<string, Material>>();
            _materialAlternativesDictionary = new Dictionary<string, List<KeyValuePair<string, Material>>>();

            var libraryMaterials = Services.Get<DataManager>().Materials;
            var modelMaterials = _renderer.materials;

            // Check what types of alternatives for materials are applicable for the model.
            for (var i = 0; i < modelMaterials.Length; i++)
            {
                var material = modelMaterials[i];
                var (designation, defaultMaterial) = MaterialLoader.ExtractMaterialDesignationAndName(material.name);

                if (!libraryMaterials.ContainsKey(designation)) continue;
                
                _materialIndices.Add(i);
                _materialNames.Add(designation);
                _materialAlternativesDictionary.Add(designation, libraryMaterials[designation]);

                // If the material that is currently on the model is not part of the material library,
                // then it is added to the local material dictionary.
                var currentMaterialIndex = libraryMaterials[designation].FindIndex(p => p.Key == defaultMaterial);
                if (currentMaterialIndex >= 0) continue;
                
                var currentMaterial = new KeyValuePair<string, Material>(defaultMaterial, material);
                _materialAlternativesDictionary[designation].Insert(0, currentMaterial);
            }
            
            // Set defaults for indices
            _currentCategoryIndex = 0;
            _currentMaterialAlternatives = _materialAlternativesDictionary[_materialNames[_currentCategoryIndex]];
            
            // Set the current alternative
            var materialIndex = _materialIndices[_currentCategoryIndex];
            var (_, materialName) = MaterialLoader.ExtractMaterialDesignationAndName(_renderer.materials[materialIndex].name);
            
            Debug.Log($"NEW CATEGORY: {_materialNames[_currentCategoryIndex]}\n\t CURRENT MATERIAL: {materialName}");
            
            _currentAlternativeIndex = _currentMaterialAlternatives.FindIndex(p => p.Key == materialName);
            Debug.Log($"\t CURRENT ALTERNATIVE INDEX: " + _currentAlternativeIndex);
        }
        
        public List<string> GetAlternatives() => _currentMaterialAlternatives.Select(a => a.Key).ToList();
        public List<string> GetCategories() => _materialNames;
        public int GetCurrentAlternativeIndex() => _currentAlternativeIndex;
        public int GetCurrentCategoryIndex() => _currentCategoryIndex;

        public void SetCategory(int categoryIndex)
        {
            _currentCategoryIndex = categoryIndex;
            _currentMaterialAlternatives = _materialAlternativesDictionary[_materialNames[_currentCategoryIndex]];
            
            var materialIndex = _materialIndices[_currentCategoryIndex];
            var (_, materialName) = MaterialLoader.ExtractMaterialDesignationAndName(_renderer.materials[materialIndex].name);
            Debug.Log($"NEW CATEGORY: {_materialNames[categoryIndex]}\n\t CURRENT MATERIAL: {materialName}");
            _currentAlternativeIndex = _currentMaterialAlternatives.FindIndex(p => p.Key == materialName);
            Debug.Log($"\t CURRENT ALTERNATIVE INDEX: " + _currentAlternativeIndex);

            Debug.Log("Before invoke material category change");
            CategorySet?.Invoke();
            Debug.Log("After invoke material category change");
            //ApplyEffect();
        }

        public void SetAlternative(int alternativeIndex)
        {
            _currentAlternativeIndex = alternativeIndex;
            var newMaterial = new Material(_currentMaterialAlternatives[_currentAlternativeIndex].Value);

            var materials = _renderer.materials;
            var newMaterials = new Material[materials.Length];
            materials.CopyTo(newMaterials, 0);
            newMaterials[_materialIndices[_currentCategoryIndex]] = newMaterial;

            Debug.Log($"Set material {_materialIndices[_currentCategoryIndex]} " +
                      $"({_renderer.materials[_materialIndices[_currentCategoryIndex]].name}) " +
                      $"to {newMaterial.name}");
            _renderer.materials = newMaterials;
            
            AlternativeSet?.Invoke();
        }

        public void ResetEffects()
        {
            var modelMaterials = _renderer.materials;
            var freshMaterials = new Material[modelMaterials.Length];
            
            for (var i = 0; i < modelMaterials.Length; i++)
            {
                var material = modelMaterials[i];
                var (designation, defaultMaterial) = MaterialLoader.ExtractMaterialDesignationAndName(material.name);

                var freshMaterial = _materialAlternativesDictionary[designation].Find(p => p.Key == defaultMaterial).Value;
                freshMaterials[i] = freshMaterial;
            }
            
            _renderer.materials = freshMaterials;
        }

        public void ApplyEffect()
        {
            var modelMaterials = _renderer.materials;
            var affectedMaterials = new Material[modelMaterials.Length];

            for (var i = 0; i < modelMaterials.Length; i++)
            {
                var material = new Material(modelMaterials[i]);
                Debug.Log($"COPIED MATERIAL. \n\tORIGINAL NAME: {modelMaterials[i].name} \n\tCOPIED NAME: {material.name}");
                
                if (i == _currentAlternativeIndex)
                {
                    affectedMaterials[i] = material;
                    continue;
                }
                
                var color = new Color(material.color.r, material.color.g, material.color.b, 0.3f);

                // Adapted from https://forum.unity.com/threads/change-rendering-mode-via-script.476437/
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetFloat(SrcBlend, (float) UnityEngine.Rendering.BlendMode.One);
                material.SetFloat(DstBlend, (float) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetFloat(ZWrite, 0.0f);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.SetColor(Color1, color);
                
                affectedMaterials[i] = material;
            }

            _renderer.materials = affectedMaterials;
        }
    }
}