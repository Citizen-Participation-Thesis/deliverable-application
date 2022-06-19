using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;

namespace XR.Prefabs
{
    /// <summary>
    /// This plane visualizer demonstrates the use of a feathering effect
    /// at the edge of the detected plane, which reduces the visual impression
    /// of a hard edge.
    /// </summary>
    [RequireComponent(typeof(ARPlaneMeshVisualizer), typeof(MeshRenderer), typeof(ARPlane))]
    public class ARFeatheredPlaneMeshVisualizer : MonoBehaviour
    {
        [FormerlySerializedAs("m_FeatheringWidth")]
        [Tooltip("The width of the texture feathering (in world units).")]
        [SerializeField]
        float mFeatheringWidth = 0.2f;

        /// <summary>
        /// The width of the texture feathering (in world units).
        /// </summary>
        public float FeatheringWidth
        {
            get => mFeatheringWidth;
            set => mFeatheringWidth = value;
        }

        public void Awake()
        {
            m_PlaneMeshVisualizer = GetComponent<ARPlaneMeshVisualizer>();
            m_FeatheredPlaneMaterial = GetComponent<MeshRenderer>().material;
            m_Plane = GetComponent<ARPlane>();
        }

        private void OnEnable()
        {
            m_Plane.boundaryChanged += ARPlane_boundaryUpdated;
        }

        public void OnDisable()
        {
            m_Plane.boundaryChanged -= ARPlane_boundaryUpdated;
        }

        void ARPlane_boundaryUpdated(ARPlaneBoundaryChangedEventArgs eventArgs)
        {
            GenerateBoundaryUVs(m_PlaneMeshVisualizer.mesh);
        }

        /// <summary>
        /// Generate UV2s to mark the boundary vertices and feathering UV coords.
        /// </summary>
        /// <remarks>
        /// The <c>ARPlaneMeshVisualizer</c> has a <c>meshUpdated</c> event that can be used to modify the generated
        /// mesh. In this case we'll add UV2s to mark the boundary vertices.
        /// This technique avoids having to generate extra vertices for the boundary. It works best when the plane is
        /// is fairly uniform.
        /// </remarks>
        /// <param name="mesh">The <c>Mesh</c> generated by <c>ARPlaneMeshVisualizer</c></param>
        public void GenerateBoundaryUVs(Mesh mesh)
        {
            var vertexCount = mesh.vertexCount;

            // Reuse the list of UVs
            s_FeatheringUVs.Clear();
            if (s_FeatheringUVs.Capacity < vertexCount) { s_FeatheringUVs.Capacity = vertexCount; }

            mesh.GetVertices(s_Vertices);

            var centerInPlaneSpace = s_Vertices[s_Vertices.Count - 1];
            var uv = new Vector3(0, 0, 0);
            var shortestUVMapping = float.MaxValue;

            // Assume the last vertex is the center vertex.
            for (var i = 0; i < vertexCount - 1; i++)
            {
                var vertexDist = Vector3.Distance(s_Vertices[i], centerInPlaneSpace);

                // Remap the UV so that a UV of "1" marks the feathering boudary.
                // The ratio of featherBoundaryDistance/edgeDistance is the same as featherUV/edgeUV.
                // Rearrange to get the edge UV.
                var uvMapping = vertexDist / Mathf.Max(vertexDist - FeatheringWidth, 0.001f);
                uv.x = uvMapping;

                // All the UV mappings will be different. In the shader we need to know the UV value we need to fade out by.
                // Choose the shortest UV to guarentee we fade out before the border.
                // This means the feathering widths will be slightly different, we again rely on a fairly uniform plane.
                if (shortestUVMapping > uvMapping) { shortestUVMapping = uvMapping; }

                s_FeatheringUVs.Add(uv);
            }

            m_FeatheredPlaneMaterial.SetFloat("_ShortestUVMapping", shortestUVMapping);

            // Add the center vertex UV
            uv.Set(0, 0, 0);
            s_FeatheringUVs.Add(uv);

            mesh.SetUVs(1, s_FeatheringUVs);
            mesh.UploadMeshData(false);
        }

        static List<Vector3> s_FeatheringUVs = new List<Vector3>();

        static List<Vector3> s_Vertices = new List<Vector3>();

        ARPlaneMeshVisualizer m_PlaneMeshVisualizer;

        ARPlane m_Plane;

        Material m_FeatheredPlaneMaterial;
    }
}
