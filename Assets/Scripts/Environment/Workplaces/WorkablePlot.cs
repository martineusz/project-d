using System;
using System.Collections.Generic;
using Environment.Miscellaneous;
using Items;
using Units.Boids;
using Units.Boids.Allies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Environment.Workplaces
{
    public class WorkablePlot : MonoBehaviour, IWorkplace
    {
        public ResourceDataFactory resourceDataFactory;
        
        public int workerLimit = 3;
        public int stashLimit = 3;
        public int cropsLimit = 1;

        public float neediness = 1f; //growth = (W/maxW) ^ neediness
        
        [HideInInspector] public List<WorkerBoid> workers = new List<WorkerBoid>();
        [HideInInspector] public List<WorkerBoid> workerQueue = new List<WorkerBoid>();
        
        protected List<Crop> Crops = new List<Crop>();
        protected List<ResourceData> ResourceStash = new List<ResourceData>();

        private float _growthTimer = 0f;

        protected virtual void FixedUpdate()
        {
            RefillCrops();
            
            _growthTimer += Time.fixedDeltaTime;
            if (_growthTimer >= 1f)
            {
                HandleGrowth();
                _growthTimer = 0f;
            }
        }

        private void HandleGrowth()
        {
            float growthEfficiency = Mathf.Pow(workers.Count / (float)workerLimit, neediness);

            List<Crop> cropsToRemove = new List<Crop>();

            foreach (Crop crop in Crops)
            {
                crop.Grow(growthEfficiency);
                if (crop.Maturity >= 1 && ResourceStash.Count < stashLimit)
                {
                    ResourceStash.Add(crop.GiveResource());
                    cropsToRemove.Add(crop);
                    Debug.Log("Stashing crop");
                }
            }

            foreach (Crop crop in cropsToRemove)
            {
                Crops.Remove(crop);
            }
        }

        private void RefillCrops()
        {
            while (Crops.Count < cropsLimit)
            {
                if (!AddNewCrop()) break;
            }
        }

        protected virtual bool AddNewCrop()
        {
            ResourceData newResData = resourceDataFactory.GenerateNewResource();
            Crop newCrop = new Crop(newResData);
            Crops.Add(newCrop);
            return true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (workers.Count >= workerLimit) return;
            WorkerBoid workerBoid = other.GetComponent<WorkerBoid>();
            if (workerBoid != null && !workers.Contains(workerBoid) && workerBoid.TargetWorkplace == transform)
            {
                workerBoid.SetBoidState(WorkerBoidState.Working);
                AddWorker(workerBoid);
            }
        }
        
        public bool TryTakeResource(out ResourceData resource)
        {
            if (ResourceStash.Count > 0)
            {
                resource = ResourceStash[0];
                ResourceStash.RemoveAt(0);
                return true;
            }
            resource = null;
            return false;
        }

        public void AddWorker(WorkerBoid worker)
        {
            UnqueueWorker(worker);
            workers.Add(worker);
        }

        public void RemoveWorker(WorkerBoid worker)
        {
            workers.Remove(worker);
        }

        public bool QueueWorker(WorkerBoid worker)
        {
            if (workerQueue.Count < workerLimit - workers.Count)
            {
                workerQueue.Add(worker);
                return true;
            }
            
            return false;
        }

        public void UnqueueWorker(WorkerBoid worker)
        {
            workerQueue.Remove(worker);
        }

        public bool IsQueueFull()
        {
            return workerQueue.Count >= workerLimit - workers.Count;
        }
    }
}
