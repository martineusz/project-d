using System;
using System.Collections.Generic;
using Items;
using Units.Boids;
using UnityEngine;
using UnityEngine.Serialization;

namespace Environment.Workplaces
{
    public class WildPlot : MonoBehaviour, IWorkplace
    {
        public ResourceDataFactory resourceDataFactory;
        
        public int workerLimit = 3;
        public int stashLimit = 3;
        public int cropsLimit = 1;

        public float neediness = 1f; //growth = (W/maxW) ^ neediness
        
        [HideInInspector] public List<WorkerBoid> workers = new List<WorkerBoid>();
        [HideInInspector] public List<WorkerBoid> workerQueue = new List<WorkerBoid>();
        
        private List<Crop> _crops = new List<Crop>();
        private List<ResourceData> _resourceStash = new List<ResourceData>();

        private float _growthTimer = 0f;

        private void FixedUpdate()
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

            foreach (Crop crop in _crops)
            {
                crop.Grow(growthEfficiency);
                if (crop.Maturity >= 1 && _resourceStash.Count < stashLimit)
                {
                    _resourceStash.Add(crop.GiveResource());
                    cropsToRemove.Add(crop);
                    Debug.Log("Stashing crop");
                }
            }

            foreach (Crop crop in cropsToRemove)
            {
                Debug.Log("Removing crop");
                _crops.Remove(crop);
            }
        }

        private void RefillCrops()
        {
            while (_crops.Count < cropsLimit)
            {
                ResourceData newResData = resourceDataFactory.GenerateNewResource();
                Crop newCrop = new Crop(newResData);
                _crops.Add(newCrop);
                Debug.Log("adding new crop");
            }
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
            if (_resourceStash.Count > 0)
            {
                resource = _resourceStash[0];
                _resourceStash.RemoveAt(0);
                return true;
            }
            resource = null;
            return false;
        }

        public void AddWorker(WorkerBoid worker)
        {
            workers.Add(worker);
        }

        public void RemoveWorker(WorkerBoid worker)
        {
            workers.Remove(worker);
            UnqueueWorker(worker);
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
    }
}
