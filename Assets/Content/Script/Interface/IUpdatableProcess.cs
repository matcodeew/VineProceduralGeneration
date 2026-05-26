namespace PVG
{
    public interface IUpdatableProcess : IGenerationStep
    {
        public void Tick(float _deltaTime);
        public void Cancel();
    }
}