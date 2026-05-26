namespace PVG
{
    public interface IParticleQuery : IWorldQueryElement
    {
        public Particle GetParticle { get; }
    }
}