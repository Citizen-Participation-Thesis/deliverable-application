using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Data
{
    public static class MaterialLoader
    {
        public static async Task<Dictionary<string, List<KeyValuePair<string, Material>>>> LoadMaterials()
        {
            var dictionary = new Dictionary<string, List<KeyValuePair<string, Material>>>();
            var handle = Addressables.LoadAssetsAsync<Material>("Materials", material => { });
            
            Debug.Log("Material asset loading handle created");
            
            await handle.Task;

            Debug.Log("Material asset loading handle awaited");
            
            switch (handle.Status)
            {
                case AsyncOperationStatus.Failed:
                    Debug.Log("LOADING MATERIALS FAILED");
                    return default;
                case AsyncOperationStatus.Succeeded:
                    Debug.Log("LOADING MATERIALS SUCCEEDED");
                    break;
                case AsyncOperationStatus.None:
                    Debug.Log("NONE STATUS ON LOADING MATERIALS");
                    return default;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Debug.Log("Handle status logged");

            var materials = handle.Result;
            
            Debug.Log("Handle result extracted");
            
            foreach (var material in materials)
            {
                var (designation, name) = ExtractMaterialDesignationAndName(material.name);

                if (dictionary.ContainsKey(designation))
                {
                    dictionary[designation].Add(new KeyValuePair<string, Material>(name, material));
                }
                else
                {
                    dictionary.Add(designation, new List<KeyValuePair<string, Material>>
                    {
                        new KeyValuePair<string, Material>(name, material)
                    });
                }
            }
            
            Debug.Log("Material dictionary returned");
            return dictionary;
        }

        public static (string, string) ExtractMaterialDesignationAndName(string materialRawName)
        {
            //Debug.Log("Extracting material name and designation");
            
            if (!materialRawName.Contains("_")) return ("", "");
            
            var parts = materialRawName.Split('_');
            var designation = parts[0];
            var name = parts
                .Skip(1)
                .ToArray()
                .Aggregate((i, j) => i + " " + j)
                .Replace(" (Instance)", "");
            
            //Debug.Log("Completed with " + designation + " and " + name);
            
            return (designation, name);
        }
    }
}