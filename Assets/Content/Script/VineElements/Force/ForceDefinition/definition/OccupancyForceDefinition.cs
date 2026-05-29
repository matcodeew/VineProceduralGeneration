using UnityEngine;

namespace PVG
{
    [CreateAssetMenu(fileName = "OccupancyForce", menuName = "ScriptableObjects/Force/OccupancyForce")]
    public class OccupancyForceDefinition : ForceDefinition
    {
        [Header("Sampling")]
        public float SampleDistance = 1f;
        [Min(1)] public int SampleCount = 16;

        [Header("Force")]
        public float Strength = 1f;
        [Range(0f, 10f)] public float DensityMultiplier = 1f;

        [Header("Deposit")]
        public float DepositValue = 1f;

        public override ForceInstance CreateInstance(Particle _p, WorldContext _worldCtx)
        {
            return new OccupancyAvoidanceInstance(_p, this, _worldCtx); 
        }
    }
}