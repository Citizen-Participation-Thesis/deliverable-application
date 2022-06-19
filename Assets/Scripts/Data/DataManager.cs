using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using ScriptableObjects;
using ScriptableObjects.StateData;
using UnityEngine;

namespace Data
{
    public class DataManager : Singleton<DataManager>
    {
        public List<GameObject> ModelData;

        // Also a reference to the project JSON data
        public StateDataRegistry StateDataRegistry;
        public ThumbnailDataRegistry ThumbnailDataRegistry;
        public Dictionary<string, Sprite> ModeButtons { get; } = new Dictionary<string, Sprite>();

        public Dictionary<string, List<KeyValuePair<string, Material>>> Materials =
            new Dictionary<string, List<KeyValuePair<string, Material>>>();

        public async Task LoadMaterials()
        {
            // Blocks the thread. However, the Addressables system cannot be used from threads other
            // than the main thread apparently (?), so, for now, execution is blocked from here.
            Materials = await MaterialLoader.LoadMaterials();
        }

        public void LoadModeButtons()
        {
            foreach (var stateData in StateDataRegistry.stateData)
            {
                if (!stateData.isModeEntrypoint) continue;
                if (stateData.sprite == null) continue;

                var handle = stateData.sprite.LoadAssetAsync<Sprite>();

                handle.Completed += operationHandle => ModeButtons.Add(stateData.modeName, operationHandle.Result);
            }
        }

        public List<StateData> GetStateData()
        {
            return StateDataRegistry.stateData;
        }

        public Dictionary<string, List<GameObject>> GetPlaceableModels()
        {
            var placeableModels = new Dictionary<string, List<GameObject>>();
            var plants = new List<GameObject>();
            var furniture = new List<GameObject>();

            for (var index = 0; index < ModelData.Count; index++)
            {
                var model = ModelData[index];
                var component = model.GetComponent<Placeable>();
                if (component == null) continue;

                if (index < 3)
                {
                    plants.Add(model);
                    continue;
                }
                furniture.Add(model);
            }
            
            placeableModels.Add("Planter", plants);
            placeableModels.Add("Utemøbler", furniture);

            return placeableModels;
        }
    }
}