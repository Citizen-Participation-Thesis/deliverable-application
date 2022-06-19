using System.Collections.Generic;
using System.Linq;
using Models;
using ScriptableObjects.StateData;
using States;
using UnityEngine;

namespace Data
{
    public static class StateExtensions
    {
        public static State GetFreshState(this StateData data, StateManager stateManager)
        {
            Debug.Log($"Getting new state by string: {data.className}");
            
            return data.className switch
            {
                nameof(MaterialState) => new MaterialState(stateManager),
                nameof(PlacementState) => new PlacementState(stateManager),
                nameof(PhotoState) => new PhotoState(stateManager),
                nameof(PositionState) => new PositionState(stateManager),
                nameof(SwapperState) => new SwapperState(stateManager),
                nameof(RemoveState) => new RemoveState(stateManager),
                _ => default
            };
        }

        public static bool IsRelevant(this StateData data)
        {
            if (!data.isModeEntrypoint) return false;
            if (data.sprite == null) return false;
            
            if (data.className == nameof(PhotoState)) return true;
            
            //Debug.Log($"Checking scene for game objects with {data.componentClassName} for {data.className}");

            return data.componentClassName switch
            {
                nameof(MaterialChanger) => RelevanceChecker(Services.Get<ModelManager>().GetModelsWithComponent<MaterialChanger>()),
                nameof(Ground) => RelevanceChecker(Services.Get<ModelManager>().GetModelsWithComponent<Ground>()),
                nameof(Movable) => RelevanceChecker(Services.Get<ModelManager>().GetModelsWithComponent<Movable>()),
                nameof(Placeable) => RelevanceChecker(Services.Get<ModelManager>().GetModelsWithComponent<Placeable>()),
                nameof(ObjectSwapper) => RelevanceChecker(Services.Get<ModelManager>().GetModelsWithComponent<ObjectSwapper>()),
                _ => false
            };
        }

        private static bool RelevanceChecker(IReadOnlyCollection<Transform> transforms)
        {
            var value = transforms != null 
                        && transforms.Any(
                            t =>
                            {
                                if (t.GetComponent<Placeable>() != null)
                                {
                                    Debug.Log("\tPlaceable special case is not null!");
                                    return true;
                                }
                                if (t.GetComponent<Renderer>() == null)
                                {
                                    Debug.Log("\tRenderer is null!");
                                    return false;
                                }
                                if (t.GetComponent<Renderer>().enabled) return true;
                                Debug.Log("\tRenderer is disabled!");
                                return false;
                            });
            return value;
        }
    }
}