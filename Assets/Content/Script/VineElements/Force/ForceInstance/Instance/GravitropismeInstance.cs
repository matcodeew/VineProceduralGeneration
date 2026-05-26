using UnityEngine;

namespace PVG
{
    public class GravitropismeInstance : ForceInstance
    {
        private readonly GravitropismeDefinition gravityDef;

        public GravitropismeInstance(Particle particle, GravitropismeDefinition def, WorldContext _worldCtx) : base(particle, def, _worldCtx)
        {
            gravityDef = def;   
        }

        public override Vector3 Evaluate()
        {
            return
                gravityDef.gravityDirection.normalized
                * gravityDef.gravityValue
                * def.forceMultipliyer;
        }

        public override Vector3 GetDebugDirection(Particle p)
        {
            return Evaluate();
        }
    }
}