namespace PVG
{
    public interface IParticleQuery : IWorldQueryElement
    {
        public Particle GetSelf { get; }
    }
}