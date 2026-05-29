using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PVG
{
    public class VineProcessManager : MonoBehaviour
    {
        public List<Branch> branches = new();
        public VineSettings settings;
        public WorldContext sharedWorld;

        [Header("Spawn Settings")]
        public int maxParticles = 200;
        private int _totalAlive = 0;
        private int _nextId = 0;

        private void Awake()
        {
            sharedWorld = new WorldContext();
        }

        [ContextMenu("Create Branch")]
        public void TEST_CreateBranch()
        {
            Branch branch = new Branch();
            branches.Add(branch);
        }

        [ContextMenu("Create Particle")]
        public void TEST_CreateParticle()
        {
            CreateParticle(Vector3.zero, Vector3.up, GetOrCreateBranch());
        }

        [ContextMenu("Create 50 Particle")]
        public void TEST_Create50Particle()
        {
            StartCoroutine(TEST_MakeSimulation());
        }

        private IEnumerator TEST_MakeSimulation()
        {
            TEST_TickSimulation();
            for (int i = 0; i < 50; i++)
            {
                CreateParticle(Vector3.zero, Vector3.up, GetOrCreateBranch());
                yield return new WaitForSeconds(0.01f);
            }
        }

        public Particle CreateParticle(Vector3 position, Vector3 direction, Branch branch)
        {
            if (_totalAlive >= maxParticles) return null;

            Particle particle = new Particle(position, direction, settings, branch, sharedWorld, _nextId++);
            particle.OnFinished += OnParticleKill;

            GetComponent<ParticleDebugger>().targetParticles.Add(particle);
            branch.AddParticle(particle);
            _totalAlive++;
            return particle;
        }

        private void OnParticleKill(Particle p)
        {
            GetComponent<ParticleDebugger>().targetParticles.Remove(p);
            _totalAlive--;

            if (_totalAlive >= maxParticles) return;

            int childCount = Random.Range(0, 4);
            for (int i = 0; i < childCount; i++)
            {
                Vector3 spread = Random.insideUnitSphere * 0.4f;
                Vector3 childDir = (p.growthDirection + spread).normalized;
                CreateParticle(p.GetPosition, childDir, GetOrCreateBranch());
            }
        }

        private Branch GetOrCreateBranch()
        {
            if (branches.Count == 0)
            {
                Branch b = new Branch();
                branches.Add(b);
            }
            return branches[0];
        }

        [ContextMenu("Tick Simulation")]
        public void TEST_TickSimulation()
        {
            canStartTickSimulation = !canStartTickSimulation;
        }

        private bool canStartTickSimulation = false;

        private void Update()
        {
            if (!canStartTickSimulation || branches.Count == 0) return;

            foreach (Branch branch in branches)
            {
                if (branch.particles.Count == 0) continue;

                foreach (Particle particle in new List<Particle>(branch.particles))
                {
                    particle.Tick(Time.deltaTime);
                }
            }
        }
    }
}