using UnityEngine;
using System.Collections.Generic;

namespace PVG
{
    public class GridMerger : MonoBehaviour
    {

        [SerializeField] private float touchDistance = 0.1f;

        [SerializeField] private float mergeOffset = 1f;

        [SerializeField] private float expansionCellSize = 1f;

        [SerializeField] private bool drawGizmos = true;

        private List<List<ObjectGrid>> mergedGroups = new();

        private List<HashSet<Vector3Int>> groupCells = new();

        private static readonly Color[] GroupColors =
        {
            new Color(1f,  0.3f, 0.3f, 0.5f),
            new Color(0.3f,1f,  0.3f, 0.5f),
            new Color(0.3f,0.5f,1f,  0.5f),
            new Color(1f,  0.9f, 0.2f,0.5f),
            new Color(0.9f,0.3f,1f,  0.5f),
            new Color(0.2f,1f,  0.9f,0.5f),
        };


        [ContextMenu("Evaluate Grids")]
        public void Evaluate()
        {
            mergedGroups.Clear();
            groupCells.Clear();

            ObjectGrid[] all = FindObjectsByType<ObjectGrid>(FindObjectsSortMode.None);
            if (all.Length == 0)
            {
                Debug.LogWarning("[GridMerger] No ObjectGrid found in scene.");
                return;
            }

            var adjacency = new HashSet<int>[all.Length];
            for (int i = 0; i < all.Length; i++) adjacency[i] = new HashSet<int>();

            for (int i = 0; i < all.Length; i++)
                for (int j = i + 1; j < all.Length; j++)
                {
                    if (GridsTouch(all[i], all[j]))
                    {
                        adjacency[i].Add(j);
                        adjacency[j].Add(i);
                        Debug.Log($"[GridMerger] {all[i].name} <-> {all[j].name} touch");
                    }
                }

            int[] group = new int[all.Length];
            for (int i = 0; i < group.Length; i++) group[i] = i;

            int Find(int x)
            {
                while (group[x] != x) { group[x] = group[group[x]]; x = group[x]; }
                return x;
            }
            void Union(int a, int b) { group[Find(a)] = Find(b); }

            for (int i = 0; i < all.Length; i++)
                foreach (int j in adjacency[i])
                    Union(i, j);

            var groupMap = new Dictionary<int, List<ObjectGrid>>();
            for (int i = 0; i < all.Length; i++)
            {
                int root = Find(i);
                if (!groupMap.ContainsKey(root)) groupMap[root] = new List<ObjectGrid>();
                groupMap[root].Add(all[i]);
            }

            foreach (var kvp in groupMap)
            {
                List<ObjectGrid> members = kvp.Value;
                mergedGroups.Add(members);

                float cs = members[0].CellSize;

                var rawCells = new HashSet<Vector3Int>();
                foreach (ObjectGrid og in members)
                    foreach (Vector3 cell in og.Cells)
                        rawCells.Add(SnapToGrid(cell, cs));

                var expanded = ExpandCells(rawCells, mergeOffset, cs);
                groupCells.Add(expanded);
            }

            Debug.Log($"[GridMerger] {mergedGroups.Count} group(s) built.");
        }

        private bool GridsTouch(ObjectGrid a, ObjectGrid b)
        {
            if (a.Cells.Count == 0 || b.Cells.Count == 0) return false;

            float cs = Mathf.Min(a.CellSize, b.CellSize);
            float thresh = cs + touchDistance;

            Vector3 centerA = GridCenter(a);
            Vector3 centerB = GridCenter(b);
            float maxRadA = GridRadius(a);
            float maxRadB = GridRadius(b);
            if (Vector3.Distance(centerA, centerB) > maxRadA + maxRadB + thresh)
                return false;

            foreach (Vector3 ca in a.Cells)
                foreach (Vector3 cb in b.Cells)
                    if (Vector3.Distance(ca, cb) <= thresh)
                        return true;

            return false;
        }

        private static Vector3 GridCenter(ObjectGrid og)
        {
            Vector3 sum = Vector3.zero;
            foreach (Vector3 c in og.Cells) sum += c;
            return og.Cells.Count > 0 ? sum / og.Cells.Count : og.transform.position;
        }

        private static float GridRadius(ObjectGrid og)
        {
            Vector3 center = GridCenter(og);
            float maxSq = 0f;
            foreach (Vector3 c in og.Cells)
            {
                float sq = (c - center).sqrMagnitude;
                if (sq > maxSq) maxSq = sq;
            }
            return Mathf.Sqrt(maxSq);
        }

        private static Vector3Int SnapToGrid(Vector3 pos, float cs)
        {
            return new Vector3Int(
                Mathf.RoundToInt(pos.x / cs),
                Mathf.RoundToInt(pos.y / cs),
                Mathf.RoundToInt(pos.z / cs)
            );
        }

        private static Vector3 GridToWorld(Vector3Int g, float cs) =>
            new Vector3(g.x * cs, g.y * cs, g.z * cs);

        private HashSet<Vector3Int> ExpandCells(HashSet<Vector3Int> source, float offset, float cs)
        {
            if (offset <= 0f) return new HashSet<Vector3Int>(source);

            int layers = Mathf.CeilToInt(offset / cs);

            var current = new HashSet<Vector3Int>(source);
            var frontier = new HashSet<Vector3Int>(source);

            Vector3Int[] dirs =
            {
                new( 1,0,0), new(-1,0,0),
                new(0, 1,0), new(0,-1,0),
                new(0,0, 1), new(0,0,-1),
            };

            for (int layer = 0; layer < layers; layer++)
            {
                var nextFrontier = new HashSet<Vector3Int>();
                foreach (Vector3Int cell in frontier)
                    foreach (Vector3Int d in dirs)
                    {
                        Vector3Int neighbour = cell + d;
                        if (!current.Contains(neighbour))
                        {
                            current.Add(neighbour);
                            nextFrontier.Add(neighbour);
                        }
                    }
                frontier = nextFrontier;
                if (frontier.Count == 0) break;
            }

            return current;
        }


        public int GroupCount => mergedGroups.Count;

        public IEnumerable<Vector3> GetGroupCells(int i)
        {
            if (i < 0 || i >= groupCells.Count) yield break;
            float cs = mergedGroups[i].Count > 0 ? mergedGroups[i][0].CellSize : expansionCellSize;
            foreach (Vector3Int g in groupCells[i])
                yield return GridToWorld(g, cs);
        }

        public IReadOnlyList<ObjectGrid> GetGroupMembers(int i) => mergedGroups[i];

        private void OnDrawGizmosSelected()
        {
            if (!drawGizmos || groupCells == null) return;

            for (int g = 0; g < groupCells.Count; g++)
            {
                float cs = mergedGroups[g].Count > 0
                    ? mergedGroups[g][0].CellSize
                    : expansionCellSize;

                Color col = GroupColors[g % GroupColors.Length];

                Gizmos.color = col;
                foreach (Vector3Int gi in groupCells[g])
                    Gizmos.DrawWireCube(GridToWorld(gi, cs), Vector3.one * cs * 0.88f);

                Gizmos.color = Color.white;
                var members = mergedGroups[g];
                for (int m = 0; m < members.Count - 1; m++)
                    Gizmos.DrawLine(members[m].transform.position,
                                    members[m + 1].transform.position);
            }
        }
    }
}