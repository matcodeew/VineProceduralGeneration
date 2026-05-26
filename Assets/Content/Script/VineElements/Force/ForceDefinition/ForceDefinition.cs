using UnityEngine;

namespace PVG
{
    //[CreateAssetMenu(fileName = "ForceDefinition", menuName = "SriptableObject/Force/")]
    public abstract class ForceDefinition : ScriptableObject
    {
        [Header("Debug")]
        public string forceName;
        public Color debugColor;

        [Range(0, 1)] public float priority;
        public bool isEnable;

        public float forceMultipliyer;

        public abstract ForceInstance CreateInstance(Particle _p, WorldContext _worldCtx);
    }
}
