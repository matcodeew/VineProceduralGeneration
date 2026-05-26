using UnityEngine;

namespace PVG
{
    public class LightObject : MonoBehaviour, ILightSource
    {
        [SerializeField] private float lightIntensity = 1f;

        private WorldContext _world;

        public float Intensity
        {
            get => lightIntensity;
            set => lightIntensity = value;
        }

        public Vector3 Position => transform.position;

        public Vector3 GetPosition { get; set; }

        public void Init(WorldContext world)
        {
            _world = world;
        }
        private void OnEnable()
        {
            WorldQuerySystem.Register<ILightSource>(this);
            GetPosition = transform.position;
        }
        private void OnDisable()
        {
            WorldQuerySystem.Unregister<ILightSource>(this);
        }
    }
}