using Units.Boids;
using Units.Boids.Allies;

namespace Environment.Workplaces
{
    public interface IWorkplace
    {
        public abstract void AddWorker(WorkerBoid worker);
        public abstract void RemoveWorker(WorkerBoid worker);
        public abstract bool QueueWorker(WorkerBoid worker);
        public abstract void UnqueueWorker(WorkerBoid worker);
        public abstract bool IsQueueFull();
    }
}