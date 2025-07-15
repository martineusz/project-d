using System.Linq;
using UnityEngine;

namespace Environment.Workplaces
{
    public class WorkableMine : WorkablePlot
    {
        public int resourceAmount;
        
        protected override void FixedUpdate()
        {
            Debug.Log("Crops count: " + Crops.Count);
            Debug.Log("Resource left: " + resourceAmount);
            Debug.Log("Resource stash count: " + ResourceStash.Count);
            Debug.Log("Workers count: " + workers.Count);
            
            if (resourceAmount <= 0 && Crops.Count <= 0)
            {
                Debug.Log("Triggering worker unassignment...");
                foreach (var worker in workers.ToList())
                {
                    Debug.Log("Unassigning form w...");
                    worker.UnassignFromWorkspace();
                }

                foreach (var worker in workerQueue.ToList())
                {
                    Debug.Log("Unassigning form q...");
                    
                    worker.UnassignFromQueue();
                }
                enabled = false;
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