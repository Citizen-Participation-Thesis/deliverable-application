using UnityEngine;

namespace Selection
{
    public class BlueprintHitResponse: IHitResponse
    {
        private GameObject _blueprint;
        private GameObject _blueprintGameObject;
        private RaycastHit _lastHitPoint;
        
        private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
        private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
        private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");
        private static readonly int Color1 = Shader.PropertyToID("_Color");

        public BlueprintHitResponse(GameObject gameObject)
        {
            _blueprint = gameObject;
        }

        public void SetBlueprint(GameObject gameObject)
        {
            _blueprint = gameObject;
            Object.Destroy(_blueprintGameObject);
        }

        public GameObject GetBlueprintGameObject()
        {
            return _blueprintGameObject;
        }

        public void OnHit(RaycastHit hit)
        {
            _lastHitPoint = hit;

            if (_blueprint == null) return;
            if (_blueprintGameObject == null)
            {
                _blueprintGameObject = PrepareGameObject();
            }
            
            _blueprintGameObject.transform.position = hit.point;
            _blueprintGameObject.transform.localRotation = Quaternion.Euler(0, Camera.main.transform.localEulerAngles.y, 0); 
        }

        public void OnMiss()
        {
            if (_blueprintGameObject == null) return;
            
            Object.Destroy(_blueprintGameObject);
        }

        private GameObject PrepareGameObject()
        {
            var model = Object.Instantiate(_blueprint, _lastHitPoint.point, Quaternion.Euler(-90, 0, 0));
            
            // Set the materials to transparent
            
            var materials = model.GetComponent<MeshRenderer>().materials;
            var transparentMaterials = new Material[materials.Length];
            
            for (var index = 0; index < materials.Length; index++)
            {
                var material = materials[index];
                var color = new Color(material.color.r, material.color.g, material.color.b, 0.5f);

                // Adapted from https://forum.unity.com/threads/change-rendering-mode-via-script.476437/
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetFloat(SrcBlend, (float) UnityEngine.Rendering.BlendMode.One);
                material.SetFloat(DstBlend, (float) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetFloat(ZWrite, 0.0f);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");

                material.SetColor(Color1, color);
                Debug.Log("SETTING COLOR TO SOMEHTING TRANSPARENT");

                transparentMaterials[index] = material;
            }

            model.GetComponent<MeshRenderer>().materials = transparentMaterials;
            Debug.Log("MATERIALS SET TO TRANSPARENT");

            return model;
        }
    }
}