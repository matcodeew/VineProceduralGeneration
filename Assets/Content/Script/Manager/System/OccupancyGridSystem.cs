using System.Collections.Generic;
using UnityEngine;

namespace PVG
{
    public class OccupancyGridSystem
    {
        [Header("Occupancy Grid")]
        public bool drawGrid = true;
        public Color emptyCellColor = new Color(0f, 1f, 0f, 0.15f);
        public Color occupiedCellColor = new Color(1f, 0f, 0f, 0.5f);
        public float densityColorScale = 1f; 


        public readonly Dictionary<Vector3Int, float> cells = new();

        public float cellSize;

        public OccupancyGridSystem(float _cellSize)
        {
            cellSize = Mathf.Max(0.001f, _cellSize);
        }

        public void Deposit(Vector3 _position, float _value)
        {
            //if (cellSize <= 0){
            //    cellSize = _cellSize;
            //}

            Vector3Int cell = WorldToCell(_position);

            if (cells.ContainsKey(cell))
                cells[cell] += _value;
            else
                cells[cell] = _value;
        }

        public float Sample(Vector3 position)
        {
            Vector3Int cell = WorldToCell(position);

            return cells.TryGetValue(cell, out float density)
                ? density
                : 0f;
        }

        public void Clear()
        {
            cells.Clear();
        }

        public Vector3Int WorldToCell(Vector3 position)
        {
            return new Vector3Int(
                Mathf.FloorToInt(position.x / cellSize),
                Mathf.FloorToInt(position.y / cellSize),
                Mathf.FloorToInt(position.z / cellSize)
            );
        }
    }
}