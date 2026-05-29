using UnityEngine;

namespace PVG
{
    public class WindForceInstance : ForceInstance
    {
        private readonly WindForceDefinition definition;
        public WindForceInstance(Particle _particle, WindForceDefinition _def, WorldContext _worldCtx) : base(_particle, _def, _worldCtx)
        {
            definition = _def;
        }

        public override Vector3 Evaluate()
        {
            Vector3 baseWind = definition.windDirection.normalized * definition.windStrength;

            float time = Time.time * definition.turbulenceFrequency;

            float noiseX = Mathf.PerlinNoise(time + particle.id, particle.physics.position.y) - 0.5f;
            float noiseY = Mathf.PerlinNoise(particle.physics.position.x + particle.id, time) - 0.5f;
            float noiseZ = Mathf.PerlinNoise(particle.physics.position.z, time + particle.id) - 0.5f;

            Vector3 turbulence = new Vector3(noiseX, noiseY, noiseZ)
                * definition.turbulenceStrength;

            return (baseWind + turbulence) * definition.forceMultipliyer;
        }
    }
}