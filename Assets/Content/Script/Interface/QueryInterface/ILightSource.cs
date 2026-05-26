namespace PVG
{
    public interface ILightSource : IWorldQueryElement
    {
        public float Intensity { get; set; }
    }
}