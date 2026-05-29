using UnityEngine;

namespace PVG
{
    [CreateAssetMenu(fileName = "Heliotropism", menuName = "ScriptableObjects/Force/Heliotropism")]
    public class HeliotropismDefinition : ForceDefinition
    {
        [Header("Heliotropism")]
        public float strength = 1f;

        public float maxDistance = 25f;

        public override ForceInstance CreateInstance(Particle particle, WorldContext _worldCtx)
        {
            return new HeliotropismInstance(particle, this, _worldCtx);   
        }
    }
}