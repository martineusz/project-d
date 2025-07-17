using System.Collections.Generic;
using Units.Boids;
using Units.Boids.Allies;
using UnityEngine;

namespace Environment.Workplaces
{
    public class StartBeacon : MonoBehaviour, IWorkplace
    {
        [HideInInspector] public List<WorkerBoid> workers = new List<WorkerBoid>();
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            WorkerBoid workerBoid = other.GetComponent<WorkerBoid>();
            if (workerBoid != null && !workers.Contains(workerBoid) && workerBoid.TargetWorkplace == transform)
            {
                workerBoid.SetBoidState(WorkerBoidState.Working);
                AddWorker(workerBoid);
            }
        }
        
        
        public void AddWorker(WorkerBoid worker)
        {
            workers.Add(worker);
        }

        public void RemoveWorker(WorkerBoid worker)
        {
            workers.Remove(worker);
        }
        
        public bool QueueWorker(WorkerBoid worker)
        {
            // StartBeacon does not queue workers, they are added directly upon entering the trigger
            return true;
        }

        public void UnqueueWorker(WorkerBoid worker)
        {
            // Not applicable for StartBeacon
        }

        public bool IsQueueFull()
        {
            return false;
        }
    }
}