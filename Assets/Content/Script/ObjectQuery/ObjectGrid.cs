using UnityEngine;
using System.Collections.Generic;

namespace PVG
{
    public class ObjectGrid : MonoBehaviour, IObjectGrid
    {
        public Vector3 GetPosition { get; set; }

        private WorldContext worldContext;
        private Collider coll;

        [SerializeField] private float cellSize = 1f;
        [SerializeField] private float gridOffset = 1f;
        [SerializeField] private bool drawGizmos = true;

        private List<Vector3> grids = new();

        public float CellSize => cellSize;
        public float GridOffset => gridOffset;
        public IReadOnlyList<Vector3> Cells => grids;
        public Collider Coll => coll;

        [ContextMenu("Create grid")]
        private void CreateGrid()
        {
            grids.Clear();

            coll = GetComponent<Collider>();
            if (coll == null)
            {
                Debug.LogWarning($"[ObjectGrid] No Collider found on {gameObject.name}");
                return;
            }

            Bounds bounds = coll.bounds;
            Vector3 expandedMin = bounds.min - Vector3.one * gridOffset;
            Vector3 expandedSize = bounds.size + Vector3.one * gridOffset * 2f;

            int stepsX = Mathf.CeilToInt(expandedSize.x / cellSize);
            int stepsY = Mathf.CeilToInt(expandedSize.y / cellSize);
            int stepsZ = Mathf.CeilToInt(expandedSize.z / cellSize);

            for (int iz = 0; iz < stepsZ; iz++)
                for (int iy = 0; iy < stepsY; iy++)
                    for (int ix = 0; ix < stepsX; ix++)
                    {
                        Vector3 cellCenter = expandedMin + new Vector3(
                            (ix + 0.5f) * cellSize,
                            (iy + 0.5f) * cellSize,
                            (iz + 0.5f) * cellSize
                        );

                        Vector3 closest = coll.ClosestPoint(cellCenter);
                        float dist = Vector3.Distance(closest, cellCenter);
                        bool isInsideOrOnSurf = dist < 0.001f;
                        bool isInOffsetShell = !isInsideOrOnSurf && dist <= gridOffset;

                        if (isInsideOrOnSurf && IsSurfaceCell(cellCenter)) grids.Add(cellCenter);
                        else if (isInOffsetShell) grids.Add(cellCenter);
                    }

            Debug.Log($"[ObjectGrid] {grids.Count} cells on {gameObject.name}");
        }

        private bool IsInsideCollider(Vector3 worldPoint)
        {
            Vector3 closest = coll.ClosestPoint(worldPoint);
            return Vector3.Distance(closest, worldPoint) < 0.001f;
        }

        private bool IsSurfaceCell(Vector3 cellCenter)
        {
            Vector3[] neighbours =
            {
                cellCenter + Vector3.right   * cellSize,
                cellCenter - Vector3.right   * cellSize,
                cellCenter + Vector3.up      * cellSize,
                cellCenter - Vector3.up      * cellSize,
                cellCenter + Vector3.forward * cellSize,
                cellCenter - Vector3.forward * cellSize,
            };
            foreach (Vector3 n in neighbours)
                if (!IsInsideCollider(n)) return true;
            return false;
        }


        public void Init(WorldContext world)
        {
            GetPosition = transform.position;
            worldContext = world;
            CreateGrid();
        }

        public bool CanSnap() => true;
        public void Snap() { }

        private void OnDrawGizmos()
        {
            if (!drawGizmos || grids == null || grids.Count == 0) return;
            if (coll == null) coll = GetComponent<Collider>();

            foreach (Vector3 cell in grids)
            {
                bool inside = coll != null && IsInsideCollider(cell);
                Gizmos.color = inside
                    ? Color.cyan
                    : Color.green;
                Gizmos.DrawWireCube(cell, Vector3.one * cellSize * 0.92f);
            }
        }
    }
}