using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PVG
{
    public class VineProcessManager : MonoBehaviour
    {
        public List<Branch> branches = new();

        public VineSettings settings;

        [ContextMenu("Create Branch")]
        public void TEST_CreateBranch()
        {
            Branch branch = new Branch();

            branches.Add(branch);

            Debug.Log("Create Branch");
        }


        [ContextMenu("Create Particle")]
        public void TEST_CreateParticle()
        {
            CreateParticle();
        }
        public void CreateParticle(int id = 0)
        {
            if (branches.Count <= 0)
            {
                Branch newbranch = new Branch();
                branches.Add(newbranch);
                Debug.Log("Create Branch");
            }

            Branch branch = branches[0];

            Particle particle = new Particle(settings, branch, new WorldContext(), id);
            particle.OnFinished += OnParticleKill;
            GetComponent<ParticleDebugger>().targetParticles.Add(particle);
            branch.AddParticle(particle);
        }

        private void OnParticleKill(Particle p)
        {
            GetComponent<ParticleDebugger>().targetParticles.Remove(p);
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
                CreateParticle(i);
                yield return new WaitForSeconds(0.01f);
            }
        }


        [ContextMenu("Tick Simulation")]
        public void TEST_TickSimulation()
        {
            canStartTickSimulation = !canStartTickSimulation;
        }

        private bool canStartTickSimulation = false;
        private void Update()
        {
            if (canStartTickSimulation == false)
                return;

            if (branches.Count <= 0)
                return;
            foreach (Branch branch in branches)
            {
                if (branch.particles.Count <= 0)
                    continue;
                foreach (Particle particle in branch.particles)
                {
                    particle.Tick(Time.deltaTime);
                }
            }
        }
    }
}