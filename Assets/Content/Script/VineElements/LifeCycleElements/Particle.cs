using System;
using UnityEngine;

namespace PVG
{
    public class Particle : IUpdatableProcess, IParticleQuery
    {
        public ParticleState state;
        public ParticlePhysicsState physics;
        public ForceSystem forceSystem;

        public Vector3 growthDirection = Vector3.up;

        private WorldContext _world;
        public int id;
        public Action<Particle> OnFinished { get; set; }
        public Vector3 GetPosition { get => physics.position; set => physics.position = value; }

        public Particle GetParticle => this;

        public Particle(VineSettings settings, Branch ownerBranch, WorldContext _worldCtx, int id)
        {
            _world = _worldCtx;

            state = new ParticleState(settings.maxLifeTime);
            physics = new ParticlePhysicsState();

            forceSystem = new ForceSystem(settings.forcesDef, this, _worldCtx);
            this.id = id;

            GetPosition = Vector3.zero;
            WorldQuerySystem.Register(this as IParticleQuery);
        }

        public void Tick(float deltaTime)
        {
            if (!state.isAlive)
                return;

            state.age += deltaTime;

            if (state.IsExpired)
            {
                Kill();
                return;
            }

            Simulate(deltaTime);
        }

        private void Simulate(float dt)
        {
            physics.accumulatedForce = Vector3.zero;

            forceSystem.ComputeForces(ref physics.accumulatedForce);

            physics.acceleration = physics.accumulatedForce / Mathf.Max(physics.mass, 0.0001f);

            physics.velocity += physics.acceleration * dt;

            growthDirection = physics.velocity.normalized;

            Vector3 nextPos = physics.position + physics.velocity * dt;

            physics.position = nextPos;

            float speed = physics.velocity.magnitude;
            physics.velocity = growthDirection * speed;
        }

        public void Kill()
        {
            WorldQuerySystem.Unregister(this as IParticleQuery);
            state.isAlive = false;
            OnFinished?.Invoke(this);
        }

        public bool IsFinished() => !state.isAlive;

        public void Cancel() { }
        public void Start() { }

        public void Init(WorldContext world)
        {
        }
    }
}