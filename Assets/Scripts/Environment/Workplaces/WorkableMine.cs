using System.Linq;
using Units.Boids.Allies;
using UnityEngine;

namespace Environment.Workplaces
{
    public class WorkableMine : WorkablePlot
    {
        public int resourceAmount;
        
        protected override void FixedUpdate()
        {
            if (resourceAmount <= 0 && Crops.Count <= 0)
            {
                gameObject.SetActive(false);
                
                foreach (var worker in workers.ToList())
                {
                    worker.UnassignFromWorkspace();
                    worker.SetBoidState(WorkerBoidState.GoingToWork);
                }

                foreach (var worker in workerQueue.ToList())
                {
                    worker.UnassignFromQueue();
                    worker.SetBoidState(WorkerBoidState.GoingToWork);
                }
                return;
            }

            base.FixedUpdate();
        }
        protected override bool AddNewCrop()
        {
            if (resourceAmount <= 0) return false;
            resourceAmount -= 1;
            base.AddNewCrop();
            return true;
        }
    }
}