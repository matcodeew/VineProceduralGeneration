namespace PVG
{
    public interface IObjectGrid : IWorldQueryElement
    {
        public bool CanSnap();
        public void Snap();
    }
}