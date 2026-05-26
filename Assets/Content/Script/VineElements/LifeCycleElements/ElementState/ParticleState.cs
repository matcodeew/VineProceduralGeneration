namespace PVG
{
    public class ParticleState
    {
        public bool isAlive = true;

        public float age;
        public float maxAge;

        public bool IsExpired => age >= maxAge;

        public ParticleState(float _maxAge)
        {
            maxAge = _maxAge;
        }
    }
}