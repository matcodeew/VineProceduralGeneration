using System.Collections.Generic;
using UnityEngine;

namespace PVG
{
    public class OccupancyAvoidanceInstance : ForceInstance
    {
        private readonly OccupancyForceDefinition occupancyDef;

        private readonly List<Vector3> sampleDirections = new();

        public OccupancyAvoidanceInstance(Particle _particle, OccupancyForceDefinition _def, WorldContext _ctx) : base(_particle, _def, _ctx)
        {
            occupancyDef = _def;

            GenerateDirections();
        }
        private void GenerateDirections()
        {
            sampleDirections.Clear();

            for (int i = 0; i < occupancyDef.SampleCount; i++)
            {
                sampleDirections.Add(Random.onUnitSphere);
            }
        }

        public override Vector3 Evaluate()
        {
            Vector3 force = Vector3.zero;

            foreach (Vector3 dir in sampleDirections)
            {
                Vector3 samplePos = particle.GetPosition + dir * occupancyDef.SampleDistance;

                float density = worldContext.occupencySystem.Sample(samplePos);

                float weight = 1f / (1f + density * occupancyDef.DensityMultiplier);

                force += dir * weight;
            }

            if (force.sqrMagnitude < 0.0001f)
                return Vector3.zero;

            return force.normalized * occupancyDef.Strength * def.forceMultipliyer;
        }
    }
}