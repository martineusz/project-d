using Units.Boids;

namespace Environment.Workplaces
{
    public interface IWorkplace
    {
        public abstract void AddWorker(WorkerBoid worker);
        public abstract void RemoveWorker(WorkerBoid worker);
    }
}