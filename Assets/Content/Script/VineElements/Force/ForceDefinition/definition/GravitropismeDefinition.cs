using UnityEngine;

namespace PVG
{

    [CreateAssetMenu(fileName = "Gravitropisme", menuName = "ScriptableObjects/Force/Gravitropisme")]
    public class GravitropismeDefinition : ForceDefinition
    {
        [Header("Gravity params")]
        [Space(5)]
        public float gravityValue = 9.81f;
        public Vector3 gravityDirection = Vector3.down;
        public override ForceInstance CreateInstance(Particle _p, WorldContext _worldCtx)
        {
            return new GravitropismeInstance(_p, this, _worldCtx);
        }
    }
}