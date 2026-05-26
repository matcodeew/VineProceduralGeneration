using UnityEngine;

namespace PVG
{
    public class ParticlePhysicsState
    {
        public Vector3 position;

        public Vector3 velocity;

        public Vector3 acceleration;

        public Vector3 accumulatedForce;

        public float mass = 1f;
    }
}