using System.Collections.Generic;
using UnityEngine;

namespace PVG
{
    public class ParticleDebugger : MonoBehaviour
    {
        [Header("Targets")]
        public List<Particle> targetParticles = new();

        [Header("Trail")]
        public int maxPoints = 100;
        public float pointRecordInterval = 0.05f;

        public Color trailColor;

        [Header("Forces Visualization")]
        public float forceScale = 0.1f;
        public bool drawForces = true;
        public bool drawTrail = true;
        public bool drawPosition = true;

        // Une liste de positions par particle
        private readonly Dictionary<Particle, List<Vector3>> _particleTrails = new();

        private float _timer;

        void Update()
        {
            if (targetParticles == null || targetParticles.Count == 0)
                return;

            RecordPositions();
        }

        void RecordPositions()
        {
            _timer += Time.deltaTime;

            if (_timer < pointRecordInterval)
                return;

            _timer = 0f;

            foreach (Particle particle in targetParticles)
            {
                if (particle == null)
                    continue;

                if (!_particleTrails.ContainsKey(particle))
                {
                    _particleTrails.Add(particle, new List<Vector3>());
                }

                List<Vector3> trail = _particleTrails[particle];

                trail.Add(particle.physics.position);

                if (trail.Count > maxPoints)
                {
                    trail.RemoveAt(0);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (targetParticles == null)
                return;

            foreach (Particle particle in targetParticles)
            {
                if (particle == null)
                    continue;

                if (drawPosition)
                    DrawCurrentPosition(particle);

                if (drawTrail)
                    DrawTrail(particle);

                if (drawForces)
                    DrawForces(particle);
            }
        }

        void DrawCurrentPosition(Particle particle)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(particle.physics.position, 0.05f);
        }

        void DrawTrail(Particle particle)
        {
            if (!_particleTrails.TryGetValue(particle, out List<Vector3> trail))
                return;

            Gizmos.color = trailColor;

            for (int i = 1; i < trail.Count; i++)
            {
                Gizmos.DrawLine(trail[i - 1], trail[i]);
            }
        }

        void DrawForces(Particle particle)
        {
            if (particle.forceSystem == null)
                return;

            if (particle.forceSystem.forces == null)
                return;

            foreach (ForceInstance force in particle.forceSystem.forces)
            {
                if (force == null || force.def == null)
                    continue;

                ForceDefinition def = force.def;

                Vector3 dir = force.GetDebugDirection(particle);

                Gizmos.color = def.debugColor;

                Gizmos.DrawLine(
                    particle.physics.position,
                    particle.physics.position + dir * forceScale
                );
            }
        }
    }
}