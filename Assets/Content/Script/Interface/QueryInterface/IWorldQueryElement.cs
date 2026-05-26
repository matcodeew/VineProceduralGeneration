using UnityEngine;

namespace PVG
{
    public interface IWorldQueryElement
    {
        public void Init(WorldContext world);
        public Vector3 GetPosition { get; set; }
    }
}