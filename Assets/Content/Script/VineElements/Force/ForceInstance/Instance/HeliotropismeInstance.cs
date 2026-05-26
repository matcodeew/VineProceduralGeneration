using System.Collections.Generic;
using UnityEngine;

namespace PVG
{
    public class HeliotropismInstance : ForceInstance
    {
        private readonly HeliotropismDefinition helioDef;
        private List<ILightSource> lightSources = new List<ILightSource>();

        public HeliotropismInstance(Particle particle, HeliotropismDefinition definition, WorldContext _worldCtx) : base(particle, definition, _worldCtx)
        {
            helioDef = definition;
            lightSources = WorldQuerySystem.GetAll<ILightSource>();

            foreach(ILightSource lightSource in lightSources)
            {
                lightSource.Init(_worldCtx);
            }
        }

        public override Vector3 Evaluate()
        {
            if (lightSources.Count == 0)
            {
                return Vector3.zero;
            }

            Vector3 finalDirection = Vector3.zero;

            Vector3 particlePosition = particle.physics.position;

            foreach (ILightSource light in lightSources)
            {
                Vector3 toLight = light.GetPosition - particlePosition;

                float distance = toLight.magnitude;

                if (distance > helioDef.maxDistance)
                {
                    continue;
                }

                Vector3 direction = toLight.normalized;

                float attenuation = 1f - (distance / helioDef.maxDistance);

                finalDirection += direction * light.Intensity * attenuation;
            }

            if (finalDirection == Vector3.zero)
            {
                return Vector3.zero;
            }

            finalDirection.Normalize();

            return finalDirection * helioDef.strength * helioDef.forceMultipliyer;
        }

        public override Vector3 GetDebugDirection(Particle p)
        {
            return Evaluate();
        }
    }
}