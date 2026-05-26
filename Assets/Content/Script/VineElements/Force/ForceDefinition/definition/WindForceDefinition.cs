using UnityEngine;

namespace PVG
{
    [CreateAssetMenu(fileName = "WindForce", menuName = "ScriptableObjects/Force/WindForce")]
    public class WindForceDefinition : ForceDefinition
    {
        [Header("Wind Settings")]

        [Tooltip("Direction principale du vent")]
        public Vector3 windDirection = Vector3.right;

        [Tooltip("Force moyenne appliquée")]
        public float windStrength = 1f;

        [Tooltip("Variation pseudo-aléatoire du vent")]
        public float turbulenceStrength = 0.5f;

        [Tooltip("Vitesse d'évolution du bruit")]
        public float turbulenceFrequency = 1f;
        public override ForceInstance CreateInstance(Particle _p, WorldContext _worldCtx)
        {
            return new WindForceInstance(_p, this, _worldCtx);
        }
    }
}